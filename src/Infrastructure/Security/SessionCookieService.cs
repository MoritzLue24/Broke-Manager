using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Auth.Common;

namespace Infrastructure.Security;

public class SessionCookieService : ISessionCookieService
{
    private readonly ISessionRepository _sessionRepo;
    private readonly IUserRepository _userRepo;
    private readonly IHasher _hasher;

    public SessionCookieService(
        ISessionRepository sessionRepo,
        IUserRepository userRepo,
        IHasher hasher)
    {
        this._sessionRepo = sessionRepo;
        this._userRepo = userRepo;
        this._hasher = hasher;
    }

    public string CreateCookieValue(Guid sessionId, string plainToken)
        => $"{sessionId}:{plainToken}";

    public async Task<SessionResult?> ValidateCookieAsync(string cookieValue)
    {
        var parts = cookieValue.Split(':');
        if (parts.Length != 2)
            return null;

        if (!Guid.TryParse(parts[0], out var sessionId))
            return null;

        var session = await this._sessionRepo.GetByIdAsync(sessionId);
        if (session is null ||
            session.ExpiresAt <= DateTime.UtcNow ||
            !this._hasher.Verify(parts[1], session.TokenHash.Value) ||
            !await this._userRepo.IdExistsAsync(session.UserId))
            return null;

        return new(session.Id, session.UserId, session.Roles);
    }
}
