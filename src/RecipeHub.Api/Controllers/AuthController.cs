using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Api.Request_Models;
using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto.Auth;
using RecipeHub.Application.Interfaces;

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
        [ProducesResponseType(typeof(AuthResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<AuthResponseModel>> Register([FromBody] RegisterRequestModel model, CancellationToken ct)
        {
            try
            {
                var dto = _mapper.Map<RegisterRequestDto>(model);
                var authResult = await _authService.RegisterAsync(dto, ct);
                var response = _mapper.Map<AuthResponseModel>(authResult);

                return CreatedAtAction(nameof(GetCurrentUser), response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<AuthResponseModel>> Login([FromBody] LoginRequestModel model, CancellationToken ct)
        {
            var dto = _mapper.Map<LoginRequestDto>(model);
            var result = await _authService.LoginAsync(dto, ct);

            if (result is null)
                return Unauthorized(new { message = "Invalid email or password." });

            var response = _mapper.Map<AuthResponseModel>(result);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseModel>> GetCurrentUser(CancellationToken ct)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(userId, ct);
            if (user is null)
                return NotFound();

            var response = _mapper.Map<UserResponseModel>(user);
            return Ok(response);
        }
    }
}