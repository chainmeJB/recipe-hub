using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Api.Request_Models;
using RecipeHub.Api.Response_Models;    
using RecipeHub.Application.Dto;
using RecipeHub.Application.Dto.Auth;
using RecipeHub.Application.Interfaces;
using AutoMapper;
namespace RecipeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IUserService userService, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseModel>> Register(RegisterRequestModel model, CancellationToken ct)
        {
            try
            {
                var dto = _mapper.Map<RegisterRequestDto>(model);
                return Ok(await _authService.RegisterAsync(dto, ct));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseModel>> Login(LoginRequestModel model, CancellationToken ct)
        {
            var dto = _mapper.Map<LoginRequestDto>(model);
        {
            var result = await _authService.LoginAsync(dto, ct);

            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(userId, ct);
            if (user == null)
                return NotFound();

            user.PasswordHash = string.Empty;
            return Ok(user);
        }
    }
}
