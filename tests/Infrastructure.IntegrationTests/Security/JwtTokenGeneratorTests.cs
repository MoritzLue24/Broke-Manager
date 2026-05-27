using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
using Infrastructure.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.IntegrationTests.Security;

public class JwtTokenGeneratorTests
{
    private readonly JwtSettings _mockSettings = new()
    {
        Secret = "test-secret-key-min-32-characters!!",
        ExpiryMinutes = 30,
        Issuer = "Broke-Manager",
        Audience = "Broke-Manager"
    };

    [Fact]
    public void GenToken_ReturnsCorrectValues()
    {
        // Setup
        var jwtGenerator = new JwtTokenGenerator(Options.Create(this._mockSettings));
        var userId = Guid.NewGuid();
        var role = Role.User;

        // Execute
        var token = jwtGenerator.GenToken(userId, [role]);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Assert
        Assert.Equal(this._mockSettings.Issuer, jwt.Issuer);
        Assert.Equal(this._mockSettings.Audience, jwt.Audiences.First());
        Assert.Equal(userId.ToString(), jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(role.ToString(), jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }

    [Fact]
    public void GenToken_ExpiryCorrect()
    {
        // Setup
        var jwtGenerator = new JwtTokenGenerator(Options.Create(this._mockSettings));
        var userId = Guid.NewGuid();
        var role = Role.User;

        // Execute
        var token = jwtGenerator.GenToken(userId, [role]);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Assert
        Assert.True(jwt.ValidTo > DateTime.UtcNow);
        Assert.True(jwt.ValidTo < DateTime.UtcNow.AddMinutes(this._mockSettings.ExpiryMinutes + 1));
    }

    [Fact]
    public void GenToken_TokenVerifiable()
    {
        // Setup
        var jwtGenerator = new JwtTokenGenerator(Options.Create(this._mockSettings));
        var userId = Guid.NewGuid();
        var role = Role.User;

        // Execute
        var token = jwtGenerator.GenToken(userId, [role]);

        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = this._mockSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = this._mockSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(this._mockSettings.Secret)),
            ValidateLifetime = true,
        };

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token, validationParams, out _);

        // Assert
        Assert.NotNull(principal);
    }
}
