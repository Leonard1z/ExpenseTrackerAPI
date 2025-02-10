using System.Security.Claims;
using Domain.DTO.ExpenseDto;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Expenses;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IValidator<ExpenseCreateDto> _validator;

        public ExpenseController(IExpenseService expenseService, IValidator<ExpenseCreateDto> validator)
        {
            _expenseService = expenseService;
            _validator = validator;
        }

        [HttpGet("getAll")]
        [Authorize]
        public async Task<IActionResult> GetExpenses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token");
            }

            int userId = int.Parse(userIdClaim);

            try
            {
                var expenses = await _expenseService.GetExpensesAsync(userId);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create-expense")]
        [Authorize]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseCreateDto expenseCreateDto)
        {
            var validatorResult = await _validator.ValidateAsync(expenseCreateDto);
            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Unable to verify user identity. Please log in again.");
            }

            int userId = int.Parse(userIdClaim);
            try
            {
                var createdExpense = await _expenseService.CreateExpenseAsync(expenseCreateDto, userId);
                return Ok(new { Message = "Expense created successfully", Expense = createdExpense });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDto expenseUpdateDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Unable to verify user identity. Please log in again.");
            }

            int userId = int.Parse(userIdClaim);

            try
            {
                var updatedExpense = await _expenseService.UpdateExpenseAsync(id, expenseUpdateDto, userId);
                if (updatedExpense == null)
                {
                    return NotFound($"Expense with ID {id} not found.");
                }

                return Ok(new { Message = "Expense updated successfully", Expense = updatedExpense });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Unable to verify user identity. Please log in again.");
            }

            int userId = int.Parse(userIdClaim);

            try
            {
                var result = await _expenseService.DeleteExpenseAsync(id, userId);
                if (!result)
                {
                    return NotFound("Expense not found");
                }

                return Ok(new { Message = "Expense deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("most-expensive")]
        [Authorize]
        public async Task<IActionResult> GetMostExpensiveExpense(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var expense = await _expenseService.GetMostExpensiveExpenseAsync(fromDate, toDate);
            return expense != null ? Ok(expense) : NotFound("No expenses found.");
        }

        [HttpGet("least-expensive")]
        [Authorize]
        public async Task<IActionResult> GetLeastExpensiveExpense(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var expense = await _expenseService.GetLeastExpensiveExpenseAsync(fromDate,toDate);
            return expense != null ? Ok(expense) : NotFound("No expenses found.");
        }

        [HttpGet("average-daily-expense")]
        [Authorize]
        public async Task<IActionResult> GetAverageDailyExpenses(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var average = await _expenseService.GetAverageDailyExpensesAsync(fromDate, toDate);
            return Ok(new { AverageDailyExpenses = average });
        }
        [HttpGet("average-monthly-expense")]
        [Authorize]
        public async Task<IActionResult> GetAverageMonthlyExpenses(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var average = await _expenseService.GetAverageMonthlyExpensesAsync(fromDate, toDate);
            return Ok(new { AverageMonthlyExpenses = average });
        }
        [HttpGet("average-yearly-expense")]
        [Authorize]
        public async Task<IActionResult> GetAverageYearlyExpenses(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var average = await _expenseService.GetAverageYearlyExpensesAsync(fromDate, toDate);
            return Ok(new { AverageYearlyExpenses = average });
        }
        [HttpGet("total-expenses")]
        [Authorize]
        public async Task<IActionResult> GetTotalExpenses(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var total = await _expenseService.GetTotalExpensesAsync(fromDate,toDate);
            return Ok(new { TotalExpenses = total });
        }
        [HttpGet("highest-expense-user")]
        [Authorize]
        public async Task<IActionResult> GetUserWithHighestTotalExpenses()
        {
            var user = await _expenseService.GetUserWithHighestTotalExpensesAsync();

            if (user == null)
            {
                return NotFound("No expenses found.");
            }

            return Ok(user);
        }
        [HttpGet("most-frequent-category")]
        [Authorize]
        public async Task<IActionResult> GetMostFrequentCategory()
        {
            var category = await _expenseService.GetMostFrequentCategory();
            return Ok(new { most_frequent_category = category });
        }
        [HttpGet("month-with-highest-average-daily-spending")]
        public async Task<ActionResult<string>> GetMonthWithHighestAverageDailySpendingAsync()
        {
            var month = await _expenseService.GetMonthWithHighestAverageDailySpendingAsync();
            return Ok(new { monthWithHighestAverageDailySpending = month });
        }
    }
}
