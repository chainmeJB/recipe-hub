using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Api.Response_Models;
using RecipeHub.Application.Dto.Enums;
using RecipeHub.Application.Interfaces;

namespace RecipeHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<UserResponseModel>>> GetAllUsers(CancellationToken ct)
        {
            var users = await _userService.GetAllUsersAsync(ct);
            return Ok(_mapper.Map<IReadOnlyList<UserResponseModel>>(users));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseModel>> GetUserById(int id, CancellationToken ct)
        {
            var user = await _userService.GetUserByIdAsync(id, ct);
            if (user is null)
                return NotFound();

            return Ok(_mapper.Map<UserResponseModel>(user));
        }

        [HttpGet("by-email")]
        [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseModel>> FindUserByEmail([FromQuery] string email, CancellationToken ct)
        {
            var user = await _userService.FindUserByEmailAsync(email, ct);
            if (user is null)
                return NotFound();

            return Ok(_mapper.Map<UserResponseModel>(user));
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPatch("{id:int}/role")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeRole(int id, [FromQuery] UserRole newRole, CancellationToken ct)
        {
            var updated = await _userService.ChangeRoleAsync(id, newRole, ct);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
        {
            var deleted = await _userService.DeleteUserAsync(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}