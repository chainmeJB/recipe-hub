using RecipeHub.Application.Dto;

namespace RecipeHub.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<RecipeDto> CreateRecipeAsync(RecipeDto dto, CancellationToken ct = default);
        Task<RecipeDto?> GetRecipeByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<RecipeDto>> SearchRecipesByNameAsync(string query, CancellationToken ct = default);
        Task<IReadOnlyList<RecipeDto>> GetAllRecipesAsync(CancellationToken ct = default);
        Task<RecipeDto?> UpdateRecipeAsync(int id, RecipeDto dto, CancellationToken ct = default);
        Task<bool> DeleteRecipeAsync(int id, CancellationToken ct = default);
    }
}
