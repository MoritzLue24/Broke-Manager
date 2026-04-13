using Api.Data;
using Api.DTOs.Auth;
using Api.Models;
using Api.Services.Token;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Auth
{
    public class AuthService : IAuthService
    {
       private readonly AppDbContext _dbContext;
        private readonly ITokenService _tokenService;

        
        public AuthService(AppDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }
    
        public async Task<AuthResult> RegisterAsync(RegisterRequestDto registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Email is already registered."
                };
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            
            var newUser = new Api.Models.User 
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return new AuthResult
            {
                Success = true,
                Message = "Registration successful."
            };

        }


        public async Task<AuthResult> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var token = _tokenService.CreateToken(user);
            return new AuthResult
            {
                Success = true,
                Token = token,
                Message = "Login successful."
            };
        }
    }
}