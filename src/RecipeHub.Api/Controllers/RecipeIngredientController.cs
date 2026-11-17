using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Api.Request_Models;
using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Interfaces;

namespace RecipeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecipeIngredientController : ControllerBase
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly IMapper _mapper;

        public RecipeIngredientController(IRecipeIngredientService recipeIngredientService, IMapper mapper)
        {
            _recipeIngredientService = recipeIngredientService;
            _mapper = mapper;
        }

        [HttpPost("recipes/{recipeId:int}")]
        [ProducesResponseType(typeof(RecipeIngredientResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<RecipeIngredientResponseModel>> AddIngredientToRecipe(
            int recipeId,
            [FromBody] RecipeIngredientRequestModel model,
            CancellationToken ct)
        {
            var dto = _mapper.Map<RecipeIngredientDto>(model);
            dto.RecipeId = recipeId;

            try
            {
                var created = await _recipeIngredientService.AddIngredientToRecipeAsync(recipeId, dto, ct);
                return CreatedAtAction(
                    nameof(GetByRecipeId),
                    new { recipeId },
                    _mapper.Map<RecipeIngredientResponseModel>(created));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("recipes/{recipeId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<RecipeIngredientResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<RecipeIngredientResponseModel>>> GetByRecipeId(
            int recipeId,
            CancellationToken ct)
        {
            var ingredients = await _recipeIngredientService.GetByRecipeIdAsync(recipeId, ct);
            return Ok(_mapper.Map<IReadOnlyList<RecipeIngredientResponseModel>>(ingredients));
        }

        [HttpPut("recipes/{recipeId:int}/ingredients/{ingredientId:int}")]
        [ProducesResponseType(typeof(RecipeIngredientResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<RecipeIngredientResponseModel>> UpdateIngredientInRecipe(
            int recipeId,
            int ingredientId,
            [FromBody] RecipeIngredientRequestModel model,
            CancellationToken ct)
        {
            var dto = _mapper.Map<RecipeIngredientDto>(model);
            dto.RecipeId = recipeId;

            var updated = await _recipeIngredientService.UpdateIngredientInRecipeAsync(recipeId, ingredientId, dto, ct);
            if (updated is null)
                return NotFound();

            return Ok(_mapper.Map<RecipeIngredientResponseModel>(updated));
        }

        [HttpDelete("recipes/{recipeId:int}/ingredients/{ingredientId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveIngredientFromRecipe(
            int recipeId,
            int ingredientId,
            CancellationToken ct)
        {
            var removed = await _recipeIngredientService.RemoveIngredientFromRecipeAsync(recipeId, ingredientId, ct);
            if (!removed)
                return NotFound();

            return NoContent();
        }
    }
}
