using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Interfaces;
using RecipeHub.Infrastructure.Entities;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Application.Services
{
    public class CategoryService :ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper mapper;
        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategoryAsync(string name, CancellationToken ct = default)
        {
            var category = mapper.Map<Category>(new CategoryDto { Name = name });

            _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync(ct);

            return MapToDto(category);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken ct = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct);

            if (category == null)
            {
                return null;
            }
            else
            {
                return MapToDto(category);
            }
        }

        public async Task<CategoryDto?> SearchCategoryByNameAsync(string name, CancellationToken ct = default)
        {
            var category = await _categoryRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name, ct);

            if (category == null)
            {
                return null;
            }
            else
            {
                return MapToDto(category);
            }
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default)
        {
            var categories = await _categoryRepository.Query()
                .AsNoTracking()
                .ToListAsync(ct);

            var result = new List<CategoryDto>(categories.Count);
            foreach (var category in categories)
            {
                result.Add(MapToDto(category));
            }

            return result;
        }

        public async Task<bool> UpdateCategoryAsync(int id, string newName, CancellationToken ct = default)
        {
            var category = await _categoryRepository.Query(tracking: true)
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            if (category == null)
                return false;

            category.Name = newName;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken ct = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct);
            if (category == null)
                return false;

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveChangesAsync(ct);
            return true;
        }

        //Methot to return a CategoryDto as a result of mapping a Category entity to a CategoryDto object
        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
