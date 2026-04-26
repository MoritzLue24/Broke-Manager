using Api.Data;
using Api.DTOs.Auth;
using Api.Exceptions;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public AuthService(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
            _httpContextAccessor = new HttpContextAccessor();
        }

        public async Task<string> RegisterAsync(RegisterRequestDto registerDto)
        {
            
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new AlreadyExistsException($"User with email {registerDto.Email} already exists.");
            }

            
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            
            var newUser = new Api.Models.User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = Role.User
            };

            
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            
            return GenerateJwtToken(newUser);
        }

        public async Task<string> LoginAsync(LoginRequestDto loginDto)
        {
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

           
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            
            return GenerateJwtToken(user);
        }

        public UserClaimsDto GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                throw new UnauthorizedAccessException("You are not logged in.");
            }

            
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            
            var email = user.FindFirst(ClaimTypes.Email)?.Value ?? user.FindFirst("email")?.Value;

            
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid token data.");
            }

            
            return new UserClaimsDto
            {
                UserId = userId,
                Email = email ?? "",
                Role = role ?? "User"
            };
        }
        
        private string GenerateJwtToken(Api.Models.User user)
        {
            
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new MissingConfigurationException("JWT_SECRET is missing.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Токен живет 7 дней
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}