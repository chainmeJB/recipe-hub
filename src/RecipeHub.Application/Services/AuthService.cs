using RecipeHub.Application.Dto;
using RecipeHub.Application.Dto.Auth;
using RecipeHub.Application.Interfaces;

namespace RecipeHub.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken ct = default)
        {
            var user = await _userService.FindUserByEmailAsync(dto.Email, ct);
            if (user == null)
                return null;

            var passwordIsValid = await _userService.VerifyPasswordAsync(user.Id, dto.Password, ct);
            if (!passwordIsValid)
                return null;

            return CreateResponse(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken ct = default)
        {
            var existingUser = await _userService.FindUserByEmailAsync(dto.Email, ct);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var user = await _userService.RegisterUserAsync(new UserDto
            {
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                PasswordHash = dto.Password
            }, ct);

            return CreateResponse(user);
        }

        private AuthResponseDto CreateResponse(UserDto user)
        {
            var token = _jwtTokenGenerator.GenerateToken(user);

            user.PasswordHash = string.Empty;

            return new AuthResponseDto
            {
                AccessToken = token.AccessToken,
                ExpiresAtUtc = token.ExpiresAtUtc,
                User = user
            };
        }
    }
}
