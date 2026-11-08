namespace RecipeHub.Application.Dto
{
    public class RecipeIngredientDto
    {
        public int RecipeId { get; set; }

        public int IngredientId { get; set; }
        public IngredientDto Ingredient { get; set; } = null!;

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
