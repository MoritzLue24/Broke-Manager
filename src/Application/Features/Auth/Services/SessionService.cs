using Application.Common.Interfaces.Security;
using Application.Features.Auth.Contracts;
using Application.Features.Auth.Interfaces;
using Application.Features.Users.Interfaces;

namespace Application.Features.Auth.Services;

public class SessionService
{
    private readonly ISessionSettings _sessionSettings;
    private readonly ISessionRepository _sessionRepo;
    private readonly IUserRepository _userRepo;
    private readonly IHasher _hasher;

    public SessionService(
        ISessionSettings sessionSettings,
        ISessionRepository sessionRepo,
        IUserRepository userRepo,
        IHasher hasher)
    {
        this._sessionSettings = sessionSettings;
        this._sessionRepo = sessionRepo;
        this._userRepo = userRepo;
        this._hasher = hasher;
    }

    public async Task<SessionResult?> Validate(Guid sessionId, string sessionToken)
    {
        var session = await this._sessionRepo.GetByIdAsync(sessionId);
        if (session is null ||
            session.ExpiresAt <= DateTime.UtcNow ||
            !this._hasher.Verify(sessionToken, session.TokenHash.Value) ||
            !await this._userRepo.IdExistsAsync(session.UserId))
            return null;

        return session.ToResult();
    }

    public async Task<bool> Visit(Guid sessionId, DateTime lastSeen)
    {
        if (DateTime.UtcNow - lastSeen < TimeSpan.FromMinutes(this._sessionSettings.LastSeenUpdateWindowMinutes))
            return false;

        await this._sessionRepo.ExecuteVisitAsync(sessionId);
        return true;
    }
}