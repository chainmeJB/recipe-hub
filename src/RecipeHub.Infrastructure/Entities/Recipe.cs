using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RecipeHub.Infrastructure.Entities
{
    public class Recipe
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Instructions { get; set; } = string.Empty;

        public int Servings { get; set; } = 1;
        public int CookTimeMinutes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
