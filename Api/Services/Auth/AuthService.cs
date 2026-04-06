using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.DTOs.Auth;
using Api.Exceptions;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<UserClaimsDto> RegisterUserAsync(RegisterRequestDto registerDto)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new AlreadyExistsException($"User with email '{registerDto.Email}' already exists");

            User newUser = new User
            {
                Email = registerDto.Email,
                Password = registerDto.Password // TODO: Hashish
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new UserClaimsDto
            {
                UserId = newUser.Id
            };
        }

        public async Task<UserClaimsDto?> ValidateUserAsync(LoginRequestDto loginDto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || user.Password != loginDto.Password)  // TODO: Hashish
            {
                return null;
            }

            return new UserClaimsDto
            {
                UserId = user.Id
            };
        }

        public string GenJwtToken(UserClaimsDto userClaims)
        {
            string string_key = _config["Jwt:Key"] 
                ?? throw new MissingConfigurationException("Jwt-Key not found in appsettings.json");
            byte[] key = Encoding.ASCII.GetBytes(string_key);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userClaims.UserId.ToString())
            };

            string string_expiresIn = _config["Jwt:ExpiresInMinutes"]
                ?? throw new MissingConfigurationException("Jwt-ExpiresInMinutes not found in appsettings.json");

            if (!int.TryParse(string_expiresIn, out int expiresIn))
                throw new FormatException("Jwt-ExpiresInMinutes does not have the correct int-format");

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiresIn),
                Issuer = _config["Jwt:Issuer"]
                    ?? throw new MissingConfigurationException("Jwt-Issuer not found in appsettings.json"),
                Audience = _config["Jwt:Audience"]
                    ?? throw new MissingConfigurationException("Jwt-Audience not found in appsettings.json"),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}