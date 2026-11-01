using RecipeHub.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<IngredientDto> CreateRecipeAsync(IngredientDto dto, CancellationToken ct = default);
        Task<IngredientDto?> GetRecipeByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<IngredientDto>> SearchByNameAsync(string query, CancellationToken ct = default);
        Task<IReadOnlyList<IngredientDto>> GetAllRecipesAsync(CancellationToken ct = default);
        Task<IngredientDto?> UpdateAsync(int id, IngredientDto dto, CancellationToken ct = default);
        Task<bool> DeleteRecipeAsync(int id, CancellationToken ct = default);
    }
}
