using Application.Features.Auth.Common;

namespace Application.Common.Interfaces.Security;

public interface ISessionCookieService
{
    string CreateCookieValue(Guid sessionId, string plainToken);

    Task<SessionResult?> ValidateCookieAsync(string cookieValue);
}
