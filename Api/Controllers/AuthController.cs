using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Auth;
using Api.Services.Auth;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto registerDto)
        {
            var userClaims = await _authService.RegisterUserAsync(registerDto);
            return Ok(new { Token = _authService.GenJwtToken(userClaims) });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginRequestDto loginDto)
        {
            var userClaims = await _authService.ValidateUserAsync(loginDto);
            if (userClaims == null)
                return Unauthorized(new { Message = "Email or Password is incorrect" });
            return Ok(new { Token = _authService.GenJwtToken(userClaims) });
        }
    }
}