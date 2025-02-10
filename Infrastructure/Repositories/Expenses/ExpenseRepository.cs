using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ExpenseDto;
using Domain.DTO.UserAccount;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Expenses
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ExpenseTrackerDbContext context) :base(context)
        {
            
        }

        public async Task<List<Expense>> GetAllExpensesAsync(int userId)
        {
            return await DbSet.Where(e => e.UserId == userId)
                .ToListAsync();
        }
        public async Task<double> GetAverageDailyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = DbSet.AsQueryable();

            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value && e.Date <= toDate.Value);
            }

            var filteredExpenses = await query.ToListAsync();

            var totalExpenses = 0.0;
            var distinctDays = 0;

            foreach (var expense in filteredExpenses)
            {
                totalExpenses += expense.Amount;
                distinctDays += 1;
            }

            var averageDailyExpenses = distinctDays > 0 ? totalExpenses / distinctDays : 0;

            return averageDailyExpenses;
        }


        public async Task<double> GetAverageMonthlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = DbSet.AsQueryable();

            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value && e.Date <= toDate.Value);
            }

            var monthlyExpenses = await query
                .GroupBy(e => new { e.Date.Year, e.Date.Month }) 
                .Select(g => g.Sum(e => e.Amount))
                .ToListAsync();

            return monthlyExpenses.Any() ? monthlyExpenses.Average() : 0.0;
        }

        public async Task<Expense> GetLeastExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = DbSet.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(e => e.Date <= toDate.Value);
            }

            return await query.OrderBy(e => e.Amount).FirstOrDefaultAsync();
        }

        public async Task<Expense> GetMostExpensiveExpenseAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = DbSet.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(e => e.Date <= toDate.Value);
            }

            return await query.OrderByDescending(e => e.Amount).FirstOrDefaultAsync();
        }

        public async Task<double> GetTotalAmountByCategoryIdAsync(int categoryId)
        {
            var totalAmount = await DbSet.Where(expense => expense.CategoryId == categoryId)
                .SumAsync(expense => expense.Amount);

            return totalAmount;
        }
        public async Task<double> GetAverageYearlyExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = DbSet.AsQueryable();

            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value && e.Date <= toDate.Value);
            }

            var yearlyExpenses = await query
                .GroupBy(e => new { e.Date.Year })
                .Select(g => g.Sum(e => e.Amount))
                .ToListAsync();

            return yearlyExpenses.Any() ? yearlyExpenses.Average() : 0.0;
        }
        public async Task<double> GetTotalExpensesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = DbSet.AsQueryable();

            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value && e.Date <= toDate.Value);
            }

            return await query.SumAsync(e => e.Amount);
        }

        public async Task<UserExpenseDto> GetUserWithHighestTotalExpensesAsync()
        {
            var expenses = await DbSet
                .GroupBy(e => e.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalExpenses = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            var users = await Context.Users.ToListAsync();

            var result = (from e in expenses
                          join u in users on e.UserId equals u.Id
                          orderby e.TotalExpenses descending
                          select new UserExpenseDto
                          {
                              UserId = e.UserId,
                              Name = u.Name,
                              TotalExpenses = e.TotalExpenses
                          })
                          .FirstOrDefault();

            return result;
        }

        public async Task<string> GetMostFrequentCategoryAsync()
        {
            var expenses = await DbSet.Include(e => e.Category).ToListAsync();

            var mostFrequentCategory = expenses
                .GroupBy(e => e.Category.Name)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            return mostFrequentCategory ?? string.Empty;
        }
        public async Task<string> GetMonthWithHighestAverageDailySpendingAsync()
        {
            var expenses = await DbSet.ToListAsync();

            var monthlySpending = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSpending = g.Sum(e => e.Amount),
                    DaysInMonth = DateTime.DaysInMonth(g.Key.Year, g.Key.Month)
                })
                .Select(m => new
                {
                    m.Year,
                    m.Month,
                    AverageDailySpending = m.TotalSpending / m.DaysInMonth
                })
                .OrderByDescending(m => m.AverageDailySpending)
                .FirstOrDefault();

            if (monthlySpending != null)
            {
                return $"{new DateTime(monthlySpending.Year, monthlySpending.Month, 1):MMMM yyyy}";
            }

            return string.Empty;
        }
    }
}
