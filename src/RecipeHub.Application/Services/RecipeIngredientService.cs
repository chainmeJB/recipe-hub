using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Interfaces;
using RecipeHub.Infrastructure.Entities;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Application.Services
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IRepository<RecipeIngredient> _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public RecipeIngredientService(IRepository<RecipeIngredient> recipeIngredientRepository, IMapper mapper)
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }

        public async Task<RecipeIngredientDto> AddIngredientToRecipeAsync(int recipeId, RecipeIngredientDto dto, CancellationToken ct = default)
        {
            if (dto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(dto.Quantity));

            if (string.IsNullOrWhiteSpace(dto.Unit))
                throw new ArgumentException("Unit must be provided.", nameof(dto.Unit));

            var recipeIngredient = _mapper.Map<RecipeIngredient>(dto);

            var alreadyExists = await _recipeIngredientRepository.Query()
                .AnyAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == recipeIngredient.IngredientId, ct);

            if (alreadyExists)
                throw new InvalidOperationException("Ingredient already exists in the recipe.");

            recipeIngredient.RecipeId = recipeId;

            _recipeIngredientRepository.Add(recipeIngredient);
            await _recipeIngredientRepository.SaveChangesAsync(ct);

            return _mapper.Map<RecipeIngredientDto>(recipeIngredient);
        }

        public async Task<IReadOnlyList<RecipeIngredientDto>> GetByRecipeIdAsync(int recipeId, CancellationToken ct = default)
        {
            var recipeIngredients = await _recipeIngredientRepository.Query()
                .AsNoTracking()
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync(ct);

            return _mapper.Map<IReadOnlyList<RecipeIngredientDto>>(recipeIngredients);
        }

        public async Task<RecipeIngredientDto?> UpdateIngredientInRecipeAsync(int recipeId, int ingredientId, RecipeIngredientDto dto, CancellationToken ct = default)
        {
            var recipeIngredient = await _recipeIngredientRepository.Query(tracking: true)
                .FirstOrDefaultAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == ingredientId, ct);

            if (recipeIngredient == null)
                return null;

            recipeIngredient.Quantity = dto.Quantity;
            recipeIngredient.Unit = dto.Unit;
            recipeIngredient.Note = dto.Note;

            await _recipeIngredientRepository.SaveChangesAsync(ct);

            return _mapper.Map<RecipeIngredientDto>(recipeIngredient);
        }

        public async Task<bool> RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId, CancellationToken ct = default)
        {
            var recipeIngredient = await _recipeIngredientRepository.Query(tracking: true)
                .FirstOrDefaultAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == ingredientId, ct);

            if (recipeIngredient == null)
                return false;

            _recipeIngredientRepository.Remove(recipeIngredient);
            await _recipeIngredientRepository.SaveChangesAsync(ct);
            return true;
        }
    }
}
