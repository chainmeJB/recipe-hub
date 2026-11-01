namespace RecipeHub.Application.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
    }
}
