using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO.Category;
using Domain.Entities;
using Infrastructure.Repositories.Categories;

namespace Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDTO)
        {
            var result = await _categoryRepository.CreateAsync(_mapper.Map<Category>(categoryCreateDTO));
            return _mapper.Map<CategoryDto>(result);
        }

        public Task DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto categoryUpdateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
