using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Infrastructure.Application.Admin.Queries.GetRecipientWitPagination;
using Infrastructure.Extensions;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Admin.Queries
{
    public class GetRecipientWithPaginationQuery:IRequest<PaginatedResponse<GetRecipientWithPaginationQueryDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool? IsVerified { get; set; }
    }

    internal class GetRecipientWithPaginationQueryHandler : IRequestHandler<GetRecipientWithPaginationQuery, PaginatedResponse<GetRecipientWithPaginationQueryDto>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetRecipientWithPaginationQueryHandler(IunitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<PaginatedResponse<GetRecipientWithPaginationQueryDto>> Handle(GetRecipientWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var baseImageUrl = _configuration["FilesBaseUrl"];

            var recipients = await _unitOfWork.RecipientRepo.Collection
                            .AsQueryable()
                            .Where(x => query.IsVerified != null && query.IsVerified.Value == x.IsVerified)
                            .ProjectToType<GetRecipientWithPaginationQueryDto>()
                            .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return recipients;
        }
    }
}
