using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Auth;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto registerDto)
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginRequestDto loginDto)
        {
            await Task.CompletedTask;
            return NoContent();
        }
    }
}