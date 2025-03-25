using DataAccess.Interface;
using Gaza_Support.Domains.Dtos;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace Infrastructure.Application.Donors.Commands.CreateDonation
{
    public class CreateDonationCommand : IRequest<BaseResponse<string>>
    {
        public string RecipientId { get; set; }
        public double Amount { get; set; }
        public MediaFileDto DonationInvoiceImage { get; set; }
        public string? Note { get; set; }
    }

    internal class CreateDonatinoCoomand : IRequestHandler<CreateDonationCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateDonatinoCoomand(IunitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(CreateDonationCommand command, CancellationToken cancellationToken)
        {
            var isRecipientFound = await _unitOfWork.RecipientRepo.Collection
                                    .Find(x => x.Id == command.RecipientId)
                                    .AnyAsync(cancellationToken);

            if (isRecipientFound == false)
            {
                return BaseResponse<string>.Failure("Recipient not found");
            }

            var userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var donation = command.Adapt<Donation>();
            donation.DonorId = userId;
            donation.Id = ObjectId.GenerateNewId().ToString();
            donation.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.DonationRepo.AddAsync(donation);

            return BaseResponse<string>.Success();
        }
    }
}
