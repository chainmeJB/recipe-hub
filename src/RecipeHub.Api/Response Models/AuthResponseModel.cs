
namespace RecipeHub.Api.Response_Models
{
    public class AuthResponseModel
    {
        public string? AccessToken { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public UserResponseModel User { get; set; } = null!;
    }
}
