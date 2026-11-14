using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto;

namespace RecipeHub.Api.Request_Models
{
    public class RecipeIngredientRequestModel
    {
        public int IngredientId { get; set; }

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
