using System.Security.Claims;
using Domain.DTO.Category;
using Domain.DTO.ExpenseDto;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Categories;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CategoryCreateDto> _validator;
        public CategoryController(ICategoryService categoryService,IValidator<CategoryCreateDto> validator)
        {
            _categoryService = categoryService;
            _validator = validator;
        }

        [HttpPost("create-category")]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDTO)
        {

            var validatorResult = await _validator.ValidateAsync(categoryCreateDTO);
            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);
            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryCreateDTO);
                return Ok(new { Message = "Category created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
