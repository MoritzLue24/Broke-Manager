using Microsoft.Extensions.Configuration;
using Api.DTOs.Auth;
using Api.Services.Auth;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Api.Models;


namespace Tests.Services
{
    public class AuthServiceTests
    {
        private static IConfiguration GetConfiguration()
        {
            var settings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "1c760f2dfcb20526e4db7691d9f661a438dac862bc0c750abdbefed64d3a3986"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:ExpiresInMinutes", "60"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        // REGISTER_USER_ASYNC :)
        [Fact]
        public async Task RegisterUserAsync_ShouldReturnUserClaims_WhenCredentialsAreValid()
        {
            var db = DbContextHelper.CreateContext();
            var config = GetConfiguration();

            // Service & Validation
            var service = new AuthService(db, config);
            var registerDto = new RegisterRequestDto { Email = "peter@gmx.de", Password = "MeinPassort123!", ConfirmPassword = "MeinPassort123!" };
            Validator.ValidateObject(
                registerDto,
                new ValidationContext(registerDto),
                validateAllProperties: true
            );

            // Execute
            var result = await service.RegisterUserAsync(registerDto);

            // Assert
            Assert.Equal(1, result.UserId);
        }

        // VALIDATE_USER_ASYNC :)
        [Fact]
        public async Task ValidateUserAsync_ShouldReturnUserClaims_WhenCredentialsAreValid()
        {
            // Setup
            var db = DbContextHelper.CreateContext();
            var config = GetConfiguration();
            User user = new User { Id = 1, Email = "peter@gmx.de", PasswordHash = BCrypt.Net.BCrypt.HashPassword("MeinPassort123!") }; 
            db.Users.Add(user);
            await db.SaveChangesAsync();

            // Service & Validation
            var service = new AuthService(db, config);
            var loginDto = new LoginRequestDto { Email = "peter@gmx.de", Password = "MeinPassort123!" };
            Validator.ValidateObject(
                loginDto,
                new ValidationContext(loginDto),
                validateAllProperties: true
            );

            // Execute
            var result = await service.ValidateUserAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        // GEN_JWT_TOKEN :)
        [Fact]
        public void GenJwtToken_ShouldReturnToken_WhenUserClaimsAreValid()
        {
            // Setup
            var config = GetConfiguration();
            var service = new AuthService(null!, config);
            var userClaims = new UserClaimsDto { UserId = 1 };

            // Execute
            var result = service.GenJwtToken(userClaims);

            // Assert
            Assert.NotNull(result);
        }
    }
}