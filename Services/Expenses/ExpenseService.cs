using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO.ExpenseDto;
using Domain.DTO.UserAccount;
using Domain.Entities;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Expenses;
using Infrastructure.Repositories.UserAccount;
using Microsoft.EntityFrameworkCore;

namespace Services.Expenses
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository,
            ICategoryRepository categoryRepository,
            IUserAccountRepository userAccountRepository,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
            _userAccountRepository = userAccountRepository;
            _mapper = mapper;
        }

        public async Task<List<ExpenseDto>> GetExpensesAsync(int userId)
        {
            var expenses = await _expenseRepository.GetAllExpensesAsync(userId);
            return _mapper.Map<List<ExpenseDto>>(expenses);
        }
        public async Task<ExpenseDto> CreateExpenseAsync(ExpenseCreateDto expenseCreateDto,int userId)
        {
            var category = await _categoryRepository.GetByIdAsync(expenseCreateDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            var expense = new Expense
            {
                Amount = expenseCreateDto.Amount,
                Note = expenseCreateDto.Note,
                CategoryId = expenseCreateDto.CategoryId,
                Date = DateTime.UtcNow,
                UserId = userId
            };

            var createdExpense = await _expenseRepository.CreateAsync(expense);
            return _mapper.Map<ExpenseDto>(createdExpense);
        }

        public async Task<ExpenseDto> UpdateExpenseAsync(int id, ExpenseUpdateDto expenseUpdateDto, int userId)
        {
            var existingExpense = await _expenseRepository.GetByIdAsync(id);

            if (existingExpense == null || existingExpense.UserId != userId)
            {
                return null;
            }

            var category = await _categoryRepository.GetByIdAsync(expenseUpdateDto.CategoryId);
            var totalCategoryExpenses = await _expenseRepository.GetTotalAmountByCategoryIdAsync(expenseUpdateDto.CategoryId);
            var updatedTotalCategoryExpenses = totalCategoryExpenses - existingExpense.Amount + expenseUpdateDto.Amount;
            if (updatedTotalCategoryExpenses > category.Budget)
            {
                throw new InvalidOperationException("Expense exceeds the available budget for this category.");
            }

            existingExpense = _mapper.Map(expenseUpdateDto, existingExpense);
            await _expenseRepository.UpdateAsync(existingExpense);

            return _mapper.Map<ExpenseDto>(existingExpense);
        }

        public async Task<bool> DeleteExpenseAsync(int id, int userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null || expense.UserId != userId)
            {
                return false;
            }

            await _expenseRepository.DeleteAsync(expense);

            return true;
        }
        public async Task<ExpenseDto> GetMostExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var expense = await _expenseRepository.GetMostExpensiveExpenseAsync(fromDate, toDate);
            return expense != null ? _mapper.Map<ExpenseDto>(expense) : null;
        }

        public async Task<ExpenseDto> GetLeastExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var expense = await _expenseRepository.GetLeastExpensiveExpenseAsync(fromDate,toDate);
            return expense != null ? _mapper.Map<ExpenseDto>(expense) : null;
        }
        public async Task<double> GetAverageDailyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _expenseRepository.GetAverageDailyExpensesAsync(fromDate, toDate);
        }
        public async Task<double> GetAverageMonthlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _expenseRepository.GetAverageMonthlyExpensesAsync(fromDate, toDate);
        }
        public async Task<double> GetAverageYearlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _expenseRepository.GetAverageYearlyExpensesAsync(fromDate, toDate);
        }
        public async Task<double> GetTotalExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _expenseRepository.GetTotalExpensesAsync(fromDate,toDate);
        }
        public async Task<UserExpenseDto> GetUserWithHighestTotalExpensesAsync()
        {
            return await _expenseRepository.GetUserWithHighestTotalExpensesAsync();
        }
        public async Task<string> GetMostFrequentCategory()
        {
            return await _expenseRepository.GetMostFrequentCategoryAsync();
        }

        public async Task<string> GetMonthWithHighestAverageDailySpendingAsync()
        {
            return await _expenseRepository.GetMonthWithHighestAverageDailySpendingAsync();
        }
    }
}
