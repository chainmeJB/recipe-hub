using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Api.Authorization;
using RecipeHub.Api.Request_Models;
using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Interfaces;

namespace RecipeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = AuthPolicies.User)]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeService recipeService, IMapper mapper)
        {
            _recipeService = recipeService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RecipeResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<RecipeResponseModel>> CreateRecipe(
            [FromBody] RecipeRequestModel model,
            CancellationToken ct)
        {
            if (!User.TryGetUserId(out var authorId))
                return Unauthorized();

            var dto = _mapper.Map<RecipeDto>(model);
            dto.AuthorId = authorId;

            var created = await _recipeService.CreateRecipeAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetRecipeById),
                new { id = created.Id },
                _mapper.Map<RecipeResponseModel>(created));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RecipeResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecipeResponseModel>> GetRecipeById(int id, CancellationToken ct)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id, ct);
            if (recipe is null)
                return NotFound();

            return Ok(_mapper.Map<RecipeResponseModel>(recipe));
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<RecipeResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<RecipeResponseModel>>> SearchRecipesByName(
            [FromQuery] string query,
            CancellationToken ct)
        {
            var recipes = await _recipeService.SearchRecipesByNameAsync(query, ct);
            return Ok(_mapper.Map<IReadOnlyList<RecipeResponseModel>>(recipes));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<RecipeResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<RecipeResponseModel>>> GetAllRecipes(CancellationToken ct)
        {
            var recipes = await _recipeService.GetAllRecipesAsync(ct);
            return Ok(_mapper.Map<IReadOnlyList<RecipeResponseModel>>(recipes));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(RecipeResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<RecipeResponseModel>> UpdateRecipe(
            int id,
            [FromBody] RecipeRequestModel model,
            CancellationToken ct)
        {
            var accessResult = await EnsureCanManageRecipeAsync(id, ct);
            if (accessResult is not null)
                return accessResult;

            var dto = _mapper.Map<RecipeDto>(model);
            dto.Id = id;

            var updated = await _recipeService.UpdateRecipeAsync(id, dto, ct);
            if (updated is null)
                return NotFound();

            return Ok(_mapper.Map<RecipeResponseModel>(updated));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecipe(int id, CancellationToken ct)
        {
            var accessResult = await EnsureCanManageRecipeAsync(id, ct);
            if (accessResult is not null)
                return accessResult;

            var deleted = await _recipeService.DeleteRecipeAsync(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private async Task<ActionResult?> EnsureCanManageRecipeAsync(int id, CancellationToken ct)
        {
            if (User.IsAdmin())
                return null;

            if (!User.TryGetUserId(out var userId))
                return Unauthorized();

            var recipe = await _recipeService.GetRecipeByIdAsync(id, ct);
            if (recipe is null)
                return NotFound();

            if (recipe.AuthorId != userId)
                return Forbid();

            return null;
        }
    }
}
