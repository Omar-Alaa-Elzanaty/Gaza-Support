using DataAccess.Interface;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Security.Claims;

namespace Infrastructure.Application.Donors.Commands.AddSubscribe
{
    public class AddSubscribeCommand : IRequest<BaseResponse<string>>
    {
        public string RecipientId { get; set; }
    }

    internal class AddSubscribeCommandHandler : IRequestHandler<AddSubscribeCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddSubscribeCommandHandler(IunitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(AddSubscribeCommand query, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.RecipientRepo.Collection
                            .Find(x => x.Id == query.RecipientId)
                            .AnyAsync(cancellationToken))
            {
                return BaseResponse<string>.Failure("Recipient not found.");
            }

            var donorId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var subscribe = new Subscribe
            {
                DonorId = donorId,
                SubscribeId = query.RecipientId
            };


            await _unitOfWork.SubscribeRepo.AddAsync(subscribe);

            return BaseResponse<string>.Success("Subscribed successfully.");
        }
    }
}
