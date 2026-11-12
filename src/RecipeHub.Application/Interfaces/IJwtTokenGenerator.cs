using RecipeHub.Application.Dto;
using RecipeHub.Application.Dto.Auth;

namespace RecipeHub.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        JwtTokenDto GenerateToken(UserDto user);
    }
}
