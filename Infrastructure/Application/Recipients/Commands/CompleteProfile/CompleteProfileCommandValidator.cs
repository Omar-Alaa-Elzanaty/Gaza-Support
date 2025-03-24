using FluentValidation;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Commands.CompleteProfile
{
    public class CompleteProfileCommandValidator:AbstractValidator<CompleteProfileCommand>
    {
    }
}
