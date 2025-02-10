using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.UserAccount;
using FluentValidation;

namespace Services.Validators
{
    public class LoginDtoValidators : AbstractValidator<LoginDto>
    {
        public LoginDtoValidators()
        {
            RuleFor(x=>x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required");
        }
    }
}
