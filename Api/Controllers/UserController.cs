using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Api.Exceptions;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        public UserController()
        {
        }

        // TODO: Als admin
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpDelete("me")]
        public async Task<ActionResult> DeleteUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        // TODO: Als Admin
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }
    }
}
