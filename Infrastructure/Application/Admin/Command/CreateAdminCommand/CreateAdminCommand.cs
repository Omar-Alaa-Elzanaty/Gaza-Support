using DataAccess.Interface;
using FluentValidation;
using Gaza_Support.Domains.Dtos.ResponseDtos;
using Gaza_Support.Domains.Models;
using Infrastructure.IServices;
using Mapster;
using MediatR;
using MongoDB.Driver;

namespace Infrastructure.Application.Admin.Command.CreateAdminCommand
{
    public class CreateAdminCommand : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
    }

    internal class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, BaseResponse<string>>
    {
        private readonly IunitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IValidator<CreateAdminCommand> _validator;

        public CreateAdminCommandHandler(IunitOfWork unitOfWork, IValidator<CreateAdminCommand> validator, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _authService = authService;
        }

        public async Task<BaseResponse<string>> Handle(CreateAdminCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if(!validationResult.IsValid)
            {
                return BaseResponse<string>.ValidationFailure(validationResult.Errors);
            }

            var user = command.Adapt<User>();
            _authService.CreatePasswordHash(user, "123@Abc");

            await _unitOfWork.UserRepo.AddAsync(user);

            return BaseResponse<string>.Success(data: user.Id);
        }
    }
}
