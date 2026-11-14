using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;

namespace RecipeHub.Api.Request_Models
{
    public class RecipeRequestModel
    {
        public int AuthorId { get; set; }
        public UserResponseModel Author { get; set; } = null!;
        public CategoryResponseModel? Category { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Instructions { get; set; } = string.Empty;

        public int Servings { get; set; } = 1;
        public int CookTimeMinutes { get; set; }

        public ICollection<ImageResponseModel> Images { get; set; } = new List<ImageResponseModel>();
        public ICollection<RecipeIngredientResponseModel> RecipeIngredients { get; set; } = new List<RecipeIngredientResponseModel>();
    }
}
