using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Interfaces;
using RecipeHub.Infrastructure.Entities;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;
        private readonly IMapper _mapper;

        public RecipeService(IRepository<Recipe> recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<RecipeDto> CreateRecipeAsync(RecipeDto dto, CancellationToken ct = default)
        {
            var recipe = _mapper.Map<Recipe>(dto);
            recipe.CreatedAt = DateTime.UtcNow;
            recipe.UpdatedAt = null;

            _recipeRepository.Add(recipe);
            await _recipeRepository.SaveChangesAsync(ct);

            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id, CancellationToken ct = default)
        {
            var recipe = await _recipeRepository.Query()
                .AsNoTracking()
                .Include(r => r.Author)
                .Include(r => r.Category)
                .Include(r => r.Images)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id, ct);

            if (recipe == null)
                return null;

            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<IReadOnlyList<RecipeDto>> SearchRecipesByNameAsync(string query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllRecipesAsync(ct);

            var recipes = await _recipeRepository.Query()
                .AsNoTracking()
                .Where(r => r.Title.ToLower().Contains(query.ToLower()))
                .ToListAsync(ct);

            return _mapper.Map<IReadOnlyList<RecipeDto>>(recipes);
        }

        public async Task<IReadOnlyList<RecipeDto>> GetAllRecipesAsync(CancellationToken ct = default)
        {
            var recipes = await _recipeRepository.Query()
                .AsNoTracking()
                .ToListAsync(ct);

            return _mapper.Map<IReadOnlyList<RecipeDto>>(recipes);
        }

        public async Task<RecipeDto?> UpdateRecipeAsync(int id, RecipeDto dto, CancellationToken ct = default)
        {
            var recipe = await _recipeRepository.Query(tracking: true)
                .FirstOrDefaultAsync(r => r.Id == id, ct);

            if (recipe == null)
                return null;

            recipe.Title = dto.Title;
            recipe.Description = dto.Description;
            recipe.Instructions = dto.Instructions;
            recipe.Servings = dto.Servings;
            recipe.CookTimeMinutes = dto.CookTimeMinutes;
            recipe.CategoryId = dto.CategoryId;

            await _recipeRepository.SaveChangesAsync(ct);

            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<bool> DeleteRecipeAsync(int id, CancellationToken ct = default)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id, ct);
            if (recipe == null)
                return false;

            _recipeRepository.Remove(recipe);
            await _recipeRepository.SaveChangesAsync(ct);
            return true;
        }
    }
}
