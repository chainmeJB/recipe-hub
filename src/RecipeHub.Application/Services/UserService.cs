using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Application.Dto;
using RecipeHub.Application.Dto.Enums;
using RecipeHub.Application.Interfaces;
using RecipeHub.Infrastructure.Entities;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterUserAsync(UserDto dto, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(dto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            user.CreatedAt = DateTime.UtcNow;

            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync(ct);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> VerifyPasswordAsync(int userId, string password, CancellationToken ct = default)
        {
            var user = await _userRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, ct);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<bool> ChangeRoleAsync(int userId, UserRole newRole, CancellationToken ct = default)
        {
            var user = await _userRepository.Query(tracking: true)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);

            if (user == null)
                return false;

            user.Role = (Infrastructure.Entities.Enums.UserRole)newRole;

            await _userRepository.SaveChangesAsync(ct);
            return true;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _userRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);

            if (user == null)
                return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userRepository.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), ct);

            if (user == null)
                return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IReadOnlyList<UserDto>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _userRepository.Query()
                .AsNoTracking()
                .ToListAsync(ct);

            return _mapper.Map<IReadOnlyList<UserDto>>(users);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UserDto dto, CancellationToken ct = default)
        {
            var user = await _userRepository.Query(tracking: true)
                .FirstOrDefaultAsync(u => u.Id == id, ct);

            if (user == null)
                return null;

            user.Email = dto.Email;
            user.DisplayName = dto.DisplayName;
            user.Role = (Infrastructure.Entities.Enums.UserRole)dto.Role;

            if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            }

            await _userRepository.SaveChangesAsync(ct);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(id, ct);
            if (user == null)
                return false;

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync(ct);
            return true;
        }
    }
}
