using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Claims;

namespace Infrastructure.Application.Recipients.Queries.GetProfile
{
    public class GetRecipientProfileQuery : IRequest<BaseResponse<GetRecipientProfileQueryDto>>;

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
            var userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var entity = await _unitOfWork.RecipientRepo.FindOneByAsync(userId);

            if (entity == null)
            {
                return BaseResponse<GetRecipientProfileQueryDto>.Failure("User not found.", HttpStatusCode.NotFound);
            }

            var fileBaseUrl = _configuration["FilesBaseUrl"];

            var user = entity.Adapt<GetRecipientProfileQueryDto>();

            user.Images.ForEach(x => x = fileBaseUrl + x);
            user.CasePrivateVideoUrl = fileBaseUrl + user.CasePrivateVideoUrl;
            user.CasePublicVideoUrl = fileBaseUrl + user.CasePublicVideoUrl;
            user.NationalIdUrl = fileBaseUrl + user.NationalIdUrl;

            return BaseResponse<GetRecipientProfileQueryDto>.Success(user);
        }
    }
}
