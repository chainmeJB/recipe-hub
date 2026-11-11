namespace RecipeHub.Application.Dto
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RecipeIngredientDto> RecipeIngredients { get; set; } = new List<RecipeIngredientDto>();
    }
}
