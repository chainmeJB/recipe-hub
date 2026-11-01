namespace RecipeHub.Application.Dto
{
    public class RecipeDto
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public UserDto Author { get; set; } = null!;

        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Instructions { get; set; } = string.Empty;

        public int Servings { get; set; } = 1;
        public int CookTimeMinutes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();
        public ICollection<RecipeIngredientDto> RecipeIngredients { get; set; } = new List<RecipeIngredientDto>();
    }
}
