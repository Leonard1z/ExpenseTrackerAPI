using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ExpenseDto;
using FluentValidation;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Expenses;

namespace Services.Validators
{
    public class ExpenseDtoValidator : AbstractValidator<ExpenseCreateDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseDtoValidator(ICategoryRepository categoryRepository, IExpenseRepository expenseRepository)
        {
            _categoryRepository = categoryRepository;
            _expenseRepository = expenseRepository;


            RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

            RuleFor(x => x.Note)
                .NotEmpty()
                .MaximumLength(255).WithMessage("Note cannot exceed 255 characters");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be valid");

            RuleFor(x => x)
              .MustAsync(async (dto, cancellationToken) => await ValidateCategoryBudget(dto, cancellationToken)) ?
              .WithMessage(x => GetCustomErrorMessage(x));
        }

        public async Task<bool> ValidateCategoryBudget(ExpenseCreateDto expenseCreateDto, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(expenseCreateDto.CategoryId);
            if (category == null)
                return false;

            var totalCategoryExpenses = await _expenseRepository.GetTotalAmountByCategoryIdAsync(expenseCreateDto.CategoryId);
            var availableBudget = category.Budget - totalCategoryExpenses;

            if (expenseCreateDto.Amount <= availableBudget)
            {
                return true;
            }
            return false;
        }

        private string GetCustomErrorMessage(ExpenseCreateDto expenseCreateDto)
        {
            var category =  _categoryRepository.GetByIdAsync(expenseCreateDto.CategoryId).Result;
            var totalCategoryExpenses =  _expenseRepository.GetTotalAmountByCategoryIdAsync(expenseCreateDto.CategoryId).Result;

            var availableBudget = category.Budget - totalCategoryExpenses;

            if (availableBudget == 0)
            {
                return "You have reached your budget limit. Your budget is 0.";
            }

            return availableBudget < expenseCreateDto.Amount
                ? $"You can only spend {availableBudget}"
                : "Expense exceeds the budget for the selected category";
        }

    }
}
