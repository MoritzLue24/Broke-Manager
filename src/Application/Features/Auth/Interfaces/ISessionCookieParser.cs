namespace Application.Features.Auth.Interfaces;

public interface ISessionCookieParser
{
    string CreateCookieValue(Guid sessionId, string plainToken);

    (Guid sessionId, string sessionToken)? ParseCookie(string cookieValue);
}
