using Application.Features.Auth.Contracts;

namespace Application.Features.Auth.Interfaces;

public interface ISessionCookieService
{
    string CreateCookieValue(Guid sessionId, string plainToken);

    Task<SessionResult?> ValidateCookieAsync(string cookieValue);
}
