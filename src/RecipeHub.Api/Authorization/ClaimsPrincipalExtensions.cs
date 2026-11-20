using System.Security.Claims;
using RecipeHub.Application.Dto.Enums;

namespace RecipeHub.Api.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal principal) =>
            principal.IsInRole(nameof(UserRole.Admin));

        public static bool TryGetUserId(this ClaimsPrincipal principal, out int userId)
        {
            var rawUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(rawUserId, out userId);
        }
    }
}
