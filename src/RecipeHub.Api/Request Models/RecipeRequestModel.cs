using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;

namespace RecipeHub.Api.Request_Models
{
    public class RecipeRequestModel
    {
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Instructions { get; set; } = string.Empty;

        public int Servings { get; set; } = 1;
        public int CookTimeMinutes { get; set; }

        public ICollection<ImageRequestModel> Images { get; set; } = new List<ImageRequestModel>();
        public ICollection<RecipeIngredientRequestModel> RecipeIngredients { get; set; } = new List<RecipeIngredientRequestModel>();
    }
}
