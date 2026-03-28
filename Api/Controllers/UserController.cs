using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Users;


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

        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetUser()
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<ActionResult> DeleteUser()
        {
            await Task.CompletedTask;
            return NoContent();
        }

        // TODO: Als Admin
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            await Task.CompletedTask;
            return NoContent();
        }
    }
}
