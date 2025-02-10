using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ExpenseDto;
using Domain.DTO.UserAccount;
using Domain.Entities;

namespace Services.Expenses
{
    public interface IExpenseService
    {
        Task<List<ExpenseDto>> GetExpensesAsync(int userId);
        Task<ExpenseDto> CreateExpenseAsync(ExpenseCreateDto expenseCreateDto,int userId);
        Task<ExpenseDto> UpdateExpenseAsync(int Id,ExpenseUpdateDto expenseUpdateDto, int userId);
        Task<bool> DeleteExpenseAsync(int Id, int userId);
        Task<ExpenseDto> GetMostExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<ExpenseDto> GetLeastExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetAverageDailyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetAverageMonthlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetAverageYearlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<double> GetTotalExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<UserExpenseDto> GetUserWithHighestTotalExpensesAsync();
        Task<string> GetMostFrequentCategory();
        Task<string> GetMonthWithHighestAverageDailySpendingAsync();

    }
}
