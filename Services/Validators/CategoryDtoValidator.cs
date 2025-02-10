using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.Category;
using FluentValidation;

namespace Services.Validators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("CategoryName is required")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Category Description is required")
                .MaximumLength(255).WithMessage("Category Description cannot exceed 255 characters");

            RuleFor(x => x.Budget)
                .NotEmpty().WithMessage("Category Budget is required")
                .GreaterThan(0).WithMessage("Budget must be greater than zero");
        }
    }
}
