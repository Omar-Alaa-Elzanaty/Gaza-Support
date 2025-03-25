using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Queries.GetDonationsWithPagination
{
    public class GetDonationsWithPaginationQuery:IRequest<PaginatedResponse<GetDonationsWithPaginationQueryDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsRead { get; set; }
    }

    internal class GetDonationsQueryHandler : IRequestHandler<GetDonationsWithPaginationQuery, PaginatedResponse<GetDonationsWithPaginationQueryDto>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public GetDonationsQueryHandler(IunitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public async Task<PaginatedResponse<GetDonationsWithPaginationQueryDto>> Handle(GetDonationsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var imageBaseUrl = _configuration["FilesBaseUrl"];

            var donotions = await _unitOfWork.DonationRepo.Collection
                                .AsQueryable()
                                .Where(x => x.IsRead == query.IsRead)
                                .OrderByDescending(x => x.CreatedAt)
                                .Join(_unitOfWork.DonorRepo.Collection,
                                donotion => donotion.DonorId,
                                donor => donor.Id,
                                (donotion, donor) => new GetDonationsWithPaginationQueryDto()
                                {
                                    Ammount = donotion.Amount,
                                    Date = donotion.CreatedAt,
                                    FirstName = donor.FirstName,
                                    MiddleName = donor.MiddleName,
                                    LastName = donor.LastName,
                                    InvoiceImageUrl = imageBaseUrl + donotion.InvoiceImageUrl
                                }).ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return donotions;
        }
    }
}
