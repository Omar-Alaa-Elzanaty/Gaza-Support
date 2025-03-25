using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Infrastructure.Application.Admin.Queries.GetRecipientById
{
    public class GetRecipientByIdQuery : IRequest<BaseResponse<GetRecipientByIdQueryDto>>
    {
        public string RecipientId { get; set; }

        public GetRecipientByIdQuery(string recipientId)
        {
            RecipientId = recipientId;
        }
    }

    internal class GetRecipientByIdQueryHandler : IRequestHandler<GetRecipientByIdQuery, BaseResponse<GetRecipientByIdQueryDto>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetRecipientByIdQueryHandler(IunitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<GetRecipientByIdQueryDto>> Handle(GetRecipientByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.RecipientRepo.FindOneByAsync(query.RecipientId);

            if (entity == null)
            {
                return BaseResponse<GetRecipientByIdQueryDto>.Failure("User not found", HttpStatusCode.NotFound);
            }

            var recipient = entity.Adapt<GetRecipientByIdQueryDto>();

            var baseFileUrl = _configuration["FileBaseUrl"];

            recipient.CasePrivateVideoUrl = baseFileUrl + recipient.CasePrivateVideoUrl;
            recipient.CasePublicVideoUrl = baseFileUrl + recipient.CasePublicVideoUrl;
            recipient.NationalIdUrl = baseFileUrl + recipient.NationalIdUrl;
            recipient.Images.ForEach(x => x = baseFileUrl + x);

            return BaseResponse<GetRecipientByIdQueryDto>.Success(recipient);
        }
    }
}
