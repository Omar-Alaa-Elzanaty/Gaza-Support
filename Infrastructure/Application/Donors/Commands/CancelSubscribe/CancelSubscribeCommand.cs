using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Security.Claims;

namespace Infrastructure.Application.Donors.Commands.CancelSubscribe
{
    public class CancelSubscribeCommand : IRequest<BaseResponse<string>>
    {
        public string RecipientId { get; set; }
    }

    internal class CancelSubscribecommandHandler : IRequestHandler<CancelSubscribeCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public CancelSubscribecommandHandler(IunitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(CancelSubscribeCommand command, CancellationToken cancellationToken)
        {
            var donorId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (await _unitOfWork.SubscribeRepo.Collection
                .Find(x => x.SubscribeId == command.RecipientId && x.DonorId == donorId)
                .AnyAsync(cancellationToken))
            {
                return BaseResponse<string>.Failure("No subscription found.");
            }


            await _unitOfWork.SubscribeRepo.Collection
                .DeleteOneAsync(x => x.SubscribeId == command.RecipientId && x.DonorId == donorId, cancellationToken);

            return BaseResponse<string>.Success("Unsubscribed successfully.");
        }
    }
}
