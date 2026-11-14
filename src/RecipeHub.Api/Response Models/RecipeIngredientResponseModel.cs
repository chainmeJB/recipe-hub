using RecipeHub.Application.Dto;

namespace RecipeHub.Api.Response_Models
{
    public class RecipeIngredientResponseModel
    {
        public int RecipeId { get; set; }
        public IngredientResponseModel Ingredient { get; set; } = null!;

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
