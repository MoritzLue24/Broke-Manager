using Infrastructure.Security;

namespace Infrastructure.IntegrationTests.Security;

public class JwtSettingsTests
{
    [Fact]
    public void Validate_ShouldReturnTrue_WhenNothingEmptyAndExipryGreaterZero()
    {
        var settings = new JwtSettings
        {
            Secret = "asd",
            ExpiryMinutes = 2,
            Issuer = "asd",
            Audience = "asd",
            CookieName = "asd"
        };
        var result = settings.Validate();

        Assert.True(result);
    }

    [Fact]
    public void Validate_ShouldThrowInvalidOperationException_WhenExpiryZero()
    {
        var settings = new JwtSettings
        {
            Secret = "asd",
            ExpiryMinutes = 0,
            Issuer = "asd",
            Audience = "asd",
            CookieName = "asd"
        };
        Assert.Throws<InvalidOperationException>(() => settings.Validate());
    }

    [Fact]
    public void Validate_ShouldThrowInvalidOperationException_WhenSecretEmpty()
    {
        var settings = new JwtSettings()
        {
            ExpiryMinutes = 2,
        };
        Assert.Throws<InvalidOperationException>(() => settings.Validate());
    }

    [Fact]
    public void Validate_ShouldThrowInvalidOperationException_WhenIssuerEmpty()
    {
        var settings = new JwtSettings()
        {
            Secret = "asd",
            ExpiryMinutes = 2
        };
        Assert.Throws<InvalidOperationException>(() => settings.Validate());
    }

    [Fact]
    public void Validate_ShouldThrowInvalidOperationException_WhenAudienceEmpty()
    {
        var settings = new JwtSettings()
        {
            Secret = "asd",
            ExpiryMinutes = 2,
            Issuer = "asd"
        };
        Assert.Throws<InvalidOperationException>(() => settings.Validate());
    }

    [Fact]
    public void Validate_ShouldThrowInvalidOperationException_WhenCookieNameEmpty()
    {
        var settings = new JwtSettings()
        {
            Secret = "asd",
            ExpiryMinutes = 2,
            Issuer = "asd",
            Audience = "asd"
        };
        Assert.Throws<InvalidOperationException>(() => settings.Validate());
    }
}