using FluentValidation;

namespace Infrastructure.Application.Authentication.DonorRegister
{
    public class DonorRegisterCommandValidator:AbstractValidator<DonorRegisterCommand>
    {
        public DonorRegisterCommandValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().EmailAddress();
        }
    }
}
