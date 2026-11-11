using RecipeHub.Application.Dto.Enums;
namespace RecipeHub.Application.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
    }
}
