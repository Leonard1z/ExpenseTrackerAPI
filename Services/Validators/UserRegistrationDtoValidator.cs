using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.UserAccount;
using FluentValidation;

namespace Services.Validators
{
    public class UserRegistrationDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegistrationDtoValidator()
        {
            RuleFor(x=>x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x=>x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[\d]").WithMessage("Password must contain at least one digit")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)_\+\-]").WithMessage("Password must contain at least one special character (!@#$%^&*()_+-).");
        }
    }
}
