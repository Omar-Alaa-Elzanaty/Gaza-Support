using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Queries.GetProfile
{
    public class GetRecipientProfileQuery:IRequest<BaseResponse<GetRecipientProfileQueryDto>>;

    internal class GetRecipientProifleQueryHandler : IRequestHandler<GetRecipientProfileQuery, BaseResponse<GetRecipientProfileQueryDto>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public GetRecipientProifleQueryHandler(
            IunitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public async Task<BaseResponse<GetRecipientProfileQueryDto>> Handle(GetRecipientProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var entity = await _unitOfWork.RecipientRepo.FindOneByAsync(userId);

            if (entity == null)
            {
                return BaseResponse<GetRecipientProfileQueryDto>.Failure("User not found.", HttpStatusCode.NotFound);
            }

            var user = entity.Adapt<GetRecipientProfileQueryDto>();

            return BaseResponse<GetRecipientProfileQueryDto>.Success(user);
        }
    }
}
