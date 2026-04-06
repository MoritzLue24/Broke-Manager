using Api.Data;
using Api.DTOs.Auth;
using Api.Exceptions;
using Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;

        public AuthService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(string email, string password)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == email))
                throw new AlreadyExistsException($"User with email '{email}' already exists");

            User newUser = new User
            {
                Email = email,
                Password = password // TODO: Hashish
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = newUser.Id
            };
        }

        public async Task<AuthResponseDto?> ValidateUserAsync(string email, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || user.Password != password)  // TODO: Hashish
            {
                return null;
            }

            return new AuthResponseDto
            {
                UserId = user.Id
            };
        }

        public string GenJwtToken(int userId)
        {
            throw new NotImplementedException();
        }
    }
}