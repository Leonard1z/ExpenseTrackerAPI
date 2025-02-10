using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.Category;

namespace Services.Categories
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDTO);
        Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryUpdateDTO);
        Task DeleteCategoryAsync(int id);
    }
}
