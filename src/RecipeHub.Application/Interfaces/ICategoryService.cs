using RecipeHub.Application.Dto;
namespace RecipeHub.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(string name, CancellationToken ct = default);
        Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken ct = default);
        Task<CategoryDto?> SearchCategoryByNameAsync(string name, CancellationToken ct = default);
        Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default);
        Task<bool> UpdateCategoryAsync(int id, string newName, CancellationToken ct = default);
        Task<bool> DeleteCategoryAsync(int id, CancellationToken ct = default);
    }
}
