using System;
using System.Collections.Generic;
using System.Text;
using RecipeHub.Infrastructure.Entities.Enums;

namespace RecipeHub.Infrastructure.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
