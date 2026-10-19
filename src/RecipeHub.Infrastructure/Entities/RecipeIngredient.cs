using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Entities
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
