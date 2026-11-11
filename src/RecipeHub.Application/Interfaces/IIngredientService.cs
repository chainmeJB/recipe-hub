using RecipeHub.Application.Dto;

namespace RecipeHub.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IngredientDto> CreateIngredientAsync(IngredientDto dto, CancellationToken ct = default);
        Task<IngredientDto?> GetIngredientByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<IngredientDto>> SearchIngredientsByNameAsync(string query, CancellationToken ct = default);
        Task<IReadOnlyList<IngredientDto>> GetAllIngredientsAsync(CancellationToken ct = default);
        Task<IngredientDto?> UpdateIngredientAsync(int id, IngredientDto dto, CancellationToken ct = default);
        Task<bool> DeleteIngredientAsync(int id, CancellationToken ct = default);
    }
}
