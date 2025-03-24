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
using MongoDB.Driver.Linq;

namespace Infrastructure.Application.Authentication.DonorRegister
{
    public class DonorRegisterCommand : IRequest<BaseResponse<string>>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    internal class RegisterCommandHandler : IRequestHandler<DonorRegisterCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IValidator<DonorRegisterCommand> _validator;

        public RegisterCommandHandler(
            IValidator<DonorRegisterCommand> validator,
            IunitOfWork unitOfWork,
            IAuthService authService)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<BaseResponse<string>> Handle(DonorRegisterCommand command, CancellationToken cancellationToken)
        {
            var validatorResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return BaseResponse<string>.ValidationFailure(validatorResult.Errors);
            }

            if (await _unitOfWork.UserRepo.Collection
                .Find(x => x.Email == command.Email).AnyAsync(cancellationToken))
            {
                return BaseResponse<string>.Failure("Email already exists.");
            }

            var donor = command.Adapt<Donor>();
            donor.Id = ObjectId.GenerateNewId().ToString();


            var roleId = await _unitOfWork.RoleRepo.Collection
                            .Find(x => x.Name == Roles.Donor)
                            .Project(x => x.Id)
                            .FirstOrDefaultAsync(cancellationToken);

            if (roleId == default)
            {
                return BaseResponse<string>.Failure("Role not found.", System.Net.HttpStatusCode.InternalServerError);
            }

            donor.RoleId = roleId;

            _authService.CreatePasswordHash(donor, command.Password);

            await _unitOfWork.UserRepo.AddAsync(donor);


            return BaseResponse<string>.Success(donor.Id, "Donor registered successfully");
        }
    }
}