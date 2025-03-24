using FluentValidation;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Donors.Commands
{
    public class CreateDonationCommandValidator:AbstractValidator<CreateDonationCommand>
    {
    }
}
