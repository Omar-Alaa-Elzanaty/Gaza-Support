using DataAccess.Interface;
using DataAccess.Repo;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Queries.GetDonors
{
    public class GetDonorsQuery:IRequest<BaseResponse<List<GetDonorsQueryDto>>>
    {

    }

    internal class GetDonorsQueryHandler : IRequestHandler<GetDonorsQuery, BaseResponse<List<GetDonorsQueryDto>>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetDonorsQueryHandler(IunitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = httpContext;
        }

        public async Task<BaseResponse<List<GetDonorsQueryDto>>> Handle(GetDonorsQuery request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var donors = await _unitOfWork.SubscribeRepo.Collection
                        .AsQueryable()
                        .Join(_unitOfWork.DonorRepo.Collection.AsQueryable(),
                        subscribe => subscribe.DonorId,
                        donor => donor.Id,
                        (subscribe, donor) => donor)
                        .ProjectToType<GetDonorsQueryDto>()
                        .ToListAsync(cancellationToken);

            return BaseResponse<List<GetDonorsQueryDto>>.Success(donors);

        }
    }
}
