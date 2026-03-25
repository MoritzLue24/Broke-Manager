using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Auth;
using Api.Services.Token;
using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ITokenService _tokenService;
        
        public AuthController(AppDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;

        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("Email is already in use");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new Api.Models.User 
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = "User" 
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Registration successful!" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginRequestDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null )
            {
                return Unauthorized("No user found with that email");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid password");
            }

            var token = _tokenService.CreateToken(user);
            
            
            return Ok(new { token , message = "Login successful!" });
        }
    }
}