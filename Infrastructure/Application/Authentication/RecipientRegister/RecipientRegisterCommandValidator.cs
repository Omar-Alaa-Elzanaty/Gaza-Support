using FluentValidation;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Authentication.RecipientRegister
{
    public class RecipientRegisterCommandValidator:AbstractValidator<RecipientRegisterCommand>
    {
        public RecipientRegisterCommandValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().EmailAddress();
        }
    }
}
