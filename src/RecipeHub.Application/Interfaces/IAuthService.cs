using RecipeHub.Application.Dto.Auth;

namespace RecipeHub.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken ct = default);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken ct = default);
    }
}
