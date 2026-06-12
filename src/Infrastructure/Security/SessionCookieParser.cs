using Application.Features.Auth.Interfaces;

namespace Infrastructure.Security;

public class SessionCookieParser : ISessionCookieParser
{
    public string CreateCookieValue(Guid sessionId, string plainToken)
        => $"{sessionId}:{plainToken}";

    public (Guid sessionId, string sessionToken)? ParseCookie(string cookieValue)
    {
        var parts = cookieValue.Split(':');
        if (parts.Length != 2)
            return null;

        if (!Guid.TryParse(parts[0], out var sessionId))
            return null;

        return (sessionId, parts[1]);
    }
}
