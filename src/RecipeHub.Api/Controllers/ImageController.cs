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
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public ImageController(IImageService imageService, IRecipeService recipeService, IMapper mapper)
        {
            _imageService = imageService;
            _recipeService = recipeService;
            _mapper = mapper;
        }

        [HttpPost("recipes/{recipeId:int}/images")]
        [ProducesResponseType(typeof(ImageResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ImageResponseModel>> AddImageToRecipe(
            int recipeId,
            [FromBody] ImageRequestModel model,
            CancellationToken ct)
        {
            var accessResult = await EnsureCanManageRecipeAsync(recipeId, ct);
            if (accessResult is not null)
                return accessResult;

            var dto = _mapper.Map<ImageDto>(model);
            var created = await _imageService.AddImageToRecipeAsync(recipeId, dto, ct);

            return CreatedAtAction(
                nameof(GetImageById),
                new { id = created.Id },
                _mapper.Map<ImageResponseModel>(created));
        }

        [HttpPut("{imageId:int}/primary")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SetImagePrimary(
            int imageId,
            [FromQuery] int recipeId,
            CancellationToken ct)
        {
            var accessResult = await EnsureCanManageRecipeAsync(recipeId, ct);
            if (accessResult is not null)
                return accessResult;

            var updated = await _imageService.SetImagePrimaryAsync(imageId, recipeId, ct);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ImageResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageResponseModel>> GetImageById(int id, CancellationToken ct)
        {
            var image = await _imageService.GetImageByIdAsync(id, ct);
            if (image is null)
                return NotFound();

            return Ok(_mapper.Map<ImageResponseModel>(image));
        }

        [AllowAnonymous]
        [HttpGet("recipes/{recipeId:int}")]
        [ProducesResponseType(typeof(IReadOnlyList<ImageResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ImageResponseModel>>> GetImagesByRecipeId(
            int recipeId,
            CancellationToken ct)
        {
            var images = await _imageService.GetImagesByRecipeIdAsync(recipeId, ct);
            return Ok(_mapper.Map<IReadOnlyList<ImageResponseModel>>(images));
        }

        [HttpPatch("recipes/{recipeId:int}/positions")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateImagesPositions(
            int recipeId,
            [FromBody] IDictionary<int, int> positions,
            CancellationToken ct)
        {
            var accessResult = await EnsureCanManageRecipeAsync(recipeId, ct);
            if (accessResult is not null)
                return accessResult;

            try
            {
                await _imageService.UpdateImagesPositionsAsync(recipeId, positions, ct);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int id, CancellationToken ct)
        {
            var image = await _imageService.GetImageByIdAsync(id, ct);
            if (image is null)
                return NotFound();

            var accessResult = await EnsureCanManageRecipeAsync(image.RecipeId, ct);
            if (accessResult is not null)
                return accessResult;

            var deleted = await _imageService.DeleteImageAsync(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private async Task<ActionResult?> EnsureCanManageRecipeAsync(int recipeId, CancellationToken ct)
        {
            if (User.IsAdmin())
                return null;

            if (!User.TryGetUserId(out var userId))
                return Unauthorized();

            var recipe = await _recipeService.GetRecipeByIdAsync(recipeId, ct);
            if (recipe is null)
                return NotFound();

            if (recipe.AuthorId != userId)
                return Forbid();

            return null;
        }
    }
}
