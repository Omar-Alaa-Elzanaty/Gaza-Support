using DataAccess.Interface;
using FluentValidation;
using Gaza_Support.Domains.Contstants;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using Infrastructure.IServices;
using Mapster;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Application.Authentication.RecipientRegister
{
    public class RecipientRegisterCommand : IRequest<BaseResponse<string>>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string Password { get; set; }
    }

    internal class RecipientRegisterCommandHandler : IRequestHandler<RecipientRegisterCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IValidator<RecipientRegisterCommand> _validator;

        public RecipientRegisterCommandHandler(
            IunitOfWork unitOfWork,
            IValidator<RecipientRegisterCommand> validator,
            IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _authService = authService;
        }

        public async Task<BaseResponse<string>> Handle(RecipientRegisterCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseResponse<string>.ValidationFailure(validationResult.Errors);
            }

            var user = await _unitOfWork.UserRepo.Collection
                            .Find(x => x.Email == command.Email)
                            .FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                return BaseResponse<string>.Failure("Email already exists");
            }

            var recipient = command.Adapt<Recipient>();
            recipient.Id = ObjectId.GenerateNewId().ToString();


            var recipientRoleId = await _unitOfWork.RoleRepo.Collection
                                .Find(x => x.Name == Roles.Recipient)
                                .Project(x => x.Id)
                                .FirstOrDefaultAsync(cancellationToken);

            recipient.RoleId = recipientRoleId;

            _authService.CreatePasswordHash(recipient, command.Password);

            await _unitOfWork.UserRepo.AddAsync(recipient);

            return BaseResponse<string>.Success(recipient.Id, "Registered successfully");
        }
    }
}