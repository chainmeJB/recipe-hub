using RecipeHub.Application.Dto;

namespace RecipeHub.Application.Interfaces
{
    public interface IRecipeIngredientService
    {
        Task<RecipeIngredientDto> AddIngredientToRecipeAsync(int recipeId, RecipeIngredientDto dto, CancellationToken ct = default);
        Task<IReadOnlyList<RecipeIngredientDto>> GetByRecipeIdAsync(int recipeId, CancellationToken ct = default);
        Task<RecipeIngredientDto?> UpdateIngredientInRecipeAsync(int recipeId, int ingredientId, RecipeIngredientDto dto, CancellationToken ct = default);
        Task<bool> RemoveIngredientFromRecipeAsync(int recipeId,int ingredientId,CancellationToken ct = default);
    }
}
