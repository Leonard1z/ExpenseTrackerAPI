using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ExpenseDto;
using Domain.DTO.UserAccount;
using Domain.Entities;

namespace Infrastructure.Repositories.Expenses
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<List<Expense>> GetAllExpensesAsync(int userId);
        Task<double> GetTotalAmountByCategoryIdAsync(int categoryId);
        Task<Expense> GetMostExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<Expense> GetLeastExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetAverageDailyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetAverageMonthlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetAverageYearlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetTotalExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<UserExpenseDto> GetUserWithHighestTotalExpensesAsync();
        Task<string> GetMostFrequentCategoryAsync();
        Task<string> GetMonthWithHighestAverageDailySpendingAsync();

    }
}
