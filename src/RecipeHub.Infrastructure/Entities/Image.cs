using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public string Url { get; set; } = string.Empty;
        public int Position { get; set; }
        public bool IsPrimary { get; set; }
    }
}
