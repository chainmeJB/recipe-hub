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
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly IMapper _mapper;

        public IngredientController(IIngredientService ingredientService, IMapper mapper)
        {
            _ingredientService = ingredientService;
            _mapper = mapper;
        }

        [Authorize(Policy = AuthPolicies.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(IngredientResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<IngredientResponseModel>> CreateIngredient(
            [FromBody] IngredientRequestModel model,
            CancellationToken ct)
        {
            var dto = _mapper.Map<IngredientDto>(model);
            var created = await _ingredientService.CreateIngredientAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetIngredientById),
                new { id = created.Id },
                _mapper.Map<IngredientResponseModel>(created));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(IngredientResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IngredientResponseModel>> GetIngredientById(int id, CancellationToken ct)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id, ct);
            if (ingredient is null)
                return NotFound();

            return Ok(_mapper.Map<IngredientResponseModel>(ingredient));
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IReadOnlyList<IngredientResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<IngredientResponseModel>>> SearchIngredientsByName(
            [FromQuery] string query,
            CancellationToken ct)
        {
            var ingredients = await _ingredientService.SearchIngredientsByNameAsync(query, ct);
            return Ok(_mapper.Map<IReadOnlyList<IngredientResponseModel>>(ingredients));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<IngredientResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<IngredientResponseModel>>> GetAllIngredients(CancellationToken ct)
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync(ct);
            return Ok(_mapper.Map<IReadOnlyList<IngredientResponseModel>>(ingredients));
        }

        [Authorize(Policy = AuthPolicies.Admin)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(IngredientResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<IngredientResponseModel>> UpdateIngredient(
            int id,
            [FromBody] IngredientRequestModel model,
            CancellationToken ct)
        {
            var dto = _mapper.Map<IngredientDto>(model);
            dto.Id = id;

            var updated = await _ingredientService.UpdateIngredientAsync(id, dto, ct);
            if (updated is null)
                return NotFound();

            return Ok(_mapper.Map<IngredientResponseModel>(updated));
        }

        [Authorize(Policy = AuthPolicies.Admin)]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredient(int id, CancellationToken ct)
        {
            var deleted = await _ingredientService.DeleteIngredientAsync(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}