using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Api.IntegrationTests.TestInfrastructure.Controllers;
using Contracts.Features.Auth;
using Contracts.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.IntegrationTests.Controllers;

public class AuthControllerTests : BaseTest
{
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly TokenValidationParameters _jwtValidationParams;

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

    private static string? GetAccessToken(HttpResponseMessage response)
    {
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        return cookies?
            .FirstOrDefault(c => c.StartsWith($"access_token="))
            ?.Split(';')[0]
            .Substring("access_token=".Length); // skip "access_token="
    }

    [Fact]
    public async void Register_ShouldReturn201AndUserAndSetCookieAndCreateDefaultCategory_WhenRequestValid()
    {
        var request = new RegisterRequest("valid@email.com", "mypasswd123!", "mypasswd123!");
        var response = await this.Client.PostAsJsonAsync("/auth/register", request);
        var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(userResponse);
        Assert.Equal("valid@email.com", userResponse?.Email);
        Assert.Equal("User", userResponse?.Role);

        // Jwt cookie
        var claims = this.ValidateJwtToken(GetAccessToken(response) ?? throw new InvalidOperationException());

        Assert.True(Guid.TryParse(claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId));
        Assert.Equal(userResponse?.Id, userId);
        Assert.NotNull(claims?.FindFirst(ClaimTypes.NameIdentifier));
        Assert.Equal("User", claims?.FindFirst(ClaimTypes.Role)?.Value);

        // Persistency check
        Assert.True(await this.Db.Users.AnyAsync(u => u.Id == userResponse!.Id));
        Assert.True(await this.Db.Categories.AnyAsync(c => c.UserId == userResponse!.Id && c.IsDefault));
    }

    [Fact]
    public async void Login_ShouldReturn200AndUserAndSetCookie_WhenCredentialsValid()
    {
        // Setup
        this.CreateMockUser("mock@mail.com", "mypasswd123!");

        // Execute
        var request = new LoginRequest("mock@mail.com", "mypasswd123!");
        var response = await this.Client.PostAsJsonAsync("/auth/login", request);
        var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(userResponse);
        Assert.Equal("mock@mail.com", userResponse?.Email);
        Assert.Equal("User", userResponse?.Role);

        // Jwt cookie
        var claims = this.ValidateJwtToken(GetAccessToken(response) ?? throw new InvalidOperationException());

        Assert.True(Guid.TryParse(claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId));
        Assert.Equal(userResponse?.Id, userId);
        Assert.NotNull(claims?.FindFirst(ClaimTypes.NameIdentifier));
        Assert.Equal("User", claims?.FindFirst(ClaimTypes.Role)?.Value);
    }
}
