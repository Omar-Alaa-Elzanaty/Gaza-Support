using DataAccess.Interface;
using FluentValidation;
using Gaza_Support.Domains.Dtos;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Enums;
using Infrastructure.IServices;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Security.Claims;

namespace Infrastructure.Application.Recipients.Commands.CompleteProfile
{
    public class CompleteProfileCommand : IRequest<BaseResponse<string>>
    {
        public string NationalId { get; set; }
        public string? NationalIdUrl { get; set; }
        public string? Story { get; set; }
        public MediaFileDto? CasePublicVideo { get; set; }
        public MediaFileDto? CasePrivateVideo { get; set; }
        public string? EgyptionMobileWallet { get; set; }
        public string? BankAccountIBAN { get; set; }
        public string? InstaPayUserName { get; set; }
        public string? LinkedInLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? GoFundMeLink { get; set; }
        public List<ContactDto> Contacts { get; set; }
        public List<PaymentMethodDto> PaymentMethods { get; set; }
    }

    public class ContactDto
    {
        public string ContactType { get; set; }
        public string Contact { get; set; }
    }

    public class PaymentMethodDto
    {
        public string PaymentName { get; set; }
        public string PaymentInfo { get; set; }
    }

    internal class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IMediaService _mediaService;
        private readonly IValidator<CompleteProfileCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public CompleteProfileCommandHandler(
            IunitOfWork unitOfWork,
            IMediaService mediaService,
            IValidator<CompleteProfileCommand> validator,
            IMapper mapper,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _mediaService = mediaService;
            _validator = validator;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<BaseResponse<string>> Handle(CompleteProfileCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseResponse<string>.ValidationFailure(validationResult.Errors);
            }

            var userId = _httpContext.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var recipient = await _unitOfWork.RecipientRepo.FindOneByAsync(userId);

            if (recipient == null)
            {
                return BaseResponse<string>.Failure("Account not found");
            }


            if (ReVerifiedCheck(command, recipient))
            {
                recipient.IsVerified = false;
            }

            if((command.CasePrivateVideo == null && recipient.CasePrivateVideoUrl != null)
                || (command.CasePrivateVideo != null && recipient.CasePrivateVideoUrl == null))
            {
                recipient.LastCheckPrivateVideo = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            _mapper.Map(command, recipient);

            recipient.CasePrivateVideoUrl = await _mediaService.SaveAsync(command.CasePrivateVideo, MediaTypes.Vidoe);
            recipient.CasePublicVideoUrl = await _mediaService.SaveAsync(command.CasePublicVideo, MediaTypes.Vidoe);

            await _unitOfWork.RecipientRepo.ReplaceAsync(recipient);

            return BaseResponse<string>.Success("Profile updated successfully.");
        }

        private static bool ReVerifiedCheck(CompleteProfileCommand command, Gaza_Support.Domains.Models.Recipient recipient)
        {
            return (command.CasePublicVideo == null && recipient.CasePublicVideoUrl != null)
                || (command.CasePublicVideo != null && recipient.CasePublicVideoUrl == null)
                || (command.CasePrivateVideo == null && recipient.CasePrivateVideoUrl != null)
                || (command.CasePrivateVideo != null && recipient.CasePrivateVideoUrl == null);
        }
    }
}
