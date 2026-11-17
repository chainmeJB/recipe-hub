using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Api.Request_Models;
using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Interfaces;

namespace RecipeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<CategoryResponseModel>> CreateCategory(
            [FromBody] CategoryRequestModel model,
            CancellationToken ct)
        {
            var created = await _categoryService.CreateCategoryAsync(model.Name, ct);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = created.Id },
                _mapper.Map<CategoryResponseModel>(created));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseModel>> GetCategoryById(int id, CancellationToken ct)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, ct);
            if (category is null)
                return NotFound();

            return Ok(_mapper.Map<CategoryResponseModel>(category));
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(CategoryResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseModel>> SearchCategoryByName(
            [FromQuery] string name,
            CancellationToken ct)
        {
            var category = await _categoryService.SearchCategoryByNameAsync(name, ct);
            if (category is null)
                return NotFound();

            return Ok(_mapper.Map<CategoryResponseModel>(category));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CategoryResponseModel>>> GetAllCategories(CancellationToken ct)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(ct);
            return Ok(_mapper.Map<IReadOnlyList<CategoryResponseModel>>(categories));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] CategoryRequestModel model,
            CancellationToken ct)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, model.Name, ct);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}