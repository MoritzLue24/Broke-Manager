using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Api.IntegrationTests.TestInfrastructure.Controllers;
using Application.Common.Interfaces.Security;
using Contracts.Features.Auth;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.IntegrationTests.Controllers;

public class AuthControllerTests : BaseTest
{
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly TokenValidationParameters _jwtValidationParams;
    private readonly IHasher _hasher;

    public AuthControllerTests(WebAppFactory factory) : base(factory)
    {
        var jwtSettings = factory.Services.GetRequiredService<IOptions<JwtSettings>>();

        this._jwtHandler = new();
        this._jwtValidationParams = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Value.Issuer,
            ValidAudience = jwtSettings.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Value.Secret)
            )
        };
        this._hasher = factory.Services.GetRequiredService<IHasher>();
    }

    private ClaimsPrincipal? ValidateJwtToken(string token)
    {
        try
        {
            var principal = this._jwtHandler.ValidateToken(token, this._jwtValidationParams, out _);
            return principal;
        }
        catch (SecurityTokenException)
        {
            return null;
        }
    }

    [Fact]
    public async void Register_ShouldReturn201AndJwtAddUserAndCreateDefaultCategory_WhenRequestValid()
    {
        var request = new RegisterRequest("valid@email.com", "mypasswd123!", "mypasswd123!");
        var response = await this.Client.PostAsJsonAsync("/auth/register", request);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(authResponse?.UserId);
        Assert.NotEmpty(authResponse.Token);

        // Jwt token
        var claims = this.ValidateJwtToken(authResponse.Token);
        Assert.True(Guid.TryParse(claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId));
        Assert.NotNull(claims?.FindFirst(ClaimTypes.NameIdentifier));
        Assert.Equal(Role.User.ToString(), claims?.FindFirst(ClaimTypes.Role)?.Value);

        // Persistency check
        Assert.True(await this.Db.Users.AnyAsync(u => u.Id == userId));
        Assert.True(await this.Db.Categories.AnyAsync(c => c.UserId == userId && c.IsDefault));
    }

    [Fact]
    public async void Login_ShouldReturn200AndJwt_WhenCredentialsValid()
    {
        // Setup
        this.Db.Users.Add(User.Create(
            Email.Create("my@email.com").Value,
            Hash.Create(this._hasher.Hash("mypasswd123!")).Value
        ).Value);
        await this.Db.SaveChangesAsync();

        // Execute
        var request = new LoginRequest("my@email.com", "mypasswd123!");
        var response = await this.Client.PostAsJsonAsync("/auth/login", request);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(authResponse?.UserId);
        Assert.NotEmpty(authResponse.Token);

        // Jwt token
        var claims = this.ValidateJwtToken(authResponse.Token);
        Assert.True(Guid.TryParse(claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId));
        Assert.NotNull(claims?.FindFirst(ClaimTypes.NameIdentifier));
        Assert.Equal(Role.User.ToString(), claims?.FindFirst(ClaimTypes.Role)?.Value);
    }
}
