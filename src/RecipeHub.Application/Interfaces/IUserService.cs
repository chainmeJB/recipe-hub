using RecipeHub.Application.Dto;
using RecipeHub.Application.Dto.Enums;

namespace RecipeHub.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(UserDto dto, CancellationToken ct = default);
        Task<bool> VerifyPasswordAsync(int userId, string password, CancellationToken ct = default);
        Task<bool> ChangeRoleAsync(int userId, UserRole newRole, CancellationToken ct = default);
        Task<UserDto?> GetUserByIdAsync(int id, CancellationToken ct = default);
        Task<UserDto?> FindUserByEmailAsync(string email, CancellationToken ct = default);
        Task<IReadOnlyList<UserDto>> GetAllUsersAsync(CancellationToken ct = default);
        Task<UserDto?> UpdateUserAsync(int id, UserDto dto, CancellationToken ct = default);
        Task<bool> DeleteUserAsync(int id, CancellationToken ct = default);
    }
}
