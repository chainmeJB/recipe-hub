namespace RecipeHub.Application.Dto.Auth
{
    public class JwtTokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
