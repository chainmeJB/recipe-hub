using AutoMapper;
using RecipeHub.Infrastructure.Entities;
using RecipeHub.Infrastructure.Repositories;
using RecipeHub.Application.Dto;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Application.Interfaces;

namespace RecipeHub.Application.Services
{
    public class IngredienService : IIngredientService
    {
        readonly IRepository<Ingredient> _ingredientRepository;
        readonly IMapper _mapper;

        public IngredienService(IRepository<Ingredient> ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }
        public async Task<IngredientDto> CreateIngredientAsync(IngredientDto dto, CancellationToken ct = default)
        {
            var ingredient = _mapper.Map<Ingredient>(dto);
            _ingredientRepository.Add(ingredient);
            await _ingredientRepository.SaveChangesAsync(ct);

            return MapToDto(ingredient);
        }

        public async Task<IngredientDto?> GetIngredientByIdAsync(int id, CancellationToken ct = default)
        {
            var ingredient = await _ingredientRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, ct);

            if (ingredient == null)
                return null;
            else
                return MapToDto(ingredient);
        }

        public async Task<IReadOnlyList<IngredientDto>> SearchIngredientsByNameAsync(string query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllIngredientsAsync(ct);

            var ingredients = await _ingredientRepository.Query()
                .AsNoTracking()
                .Where(i => i.Name.ToLower().Contains(query.ToLower()))
                .ToListAsync(ct);

            var result = new List<IngredientDto>(ingredients.Count);
            foreach (var ingredient in ingredients)
            {
                result.Add(MapToDto(ingredient));
            }

            return result;
        }

        public async Task<IReadOnlyList<IngredientDto>> GetAllIngredientsAsync(CancellationToken ct = default)
        {
            var ingredients = await _ingredientRepository.Query()
                .AsNoTracking()
                .ToListAsync(ct);

            var result = new List<IngredientDto>(ingredients.Count);
            foreach (var ingredient in ingredients)
            {
                result.Add(MapToDto(ingredient));
            }

            return result;
        }

        public async Task<IngredientDto?> UpdateIngredientAsync(int id, IngredientDto dto, CancellationToken ct = default)
        {
            var ingredient = await _ingredientRepository.Query(tracking: true)
                .FirstOrDefaultAsync(i => i.Id == id, ct);

            if (ingredient == null)
                return null;

            ingredient.Name = dto.Name;

            _ingredientRepository.Update(ingredient);
            await _ingredientRepository.SaveChangesAsync(ct);

            return MapToDto(ingredient);
        }

        public async Task<bool> DeleteIngredientAsync(int id, CancellationToken ct = default)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id, ct);
            if (ingredient == null)
                return false;

            _ingredientRepository.Remove(ingredient);
            await _ingredientRepository.SaveChangesAsync(ct);
            return true;
        }

        private static IngredientDto MapToDto(Ingredient ingredient)
        {
            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }
    }
}
