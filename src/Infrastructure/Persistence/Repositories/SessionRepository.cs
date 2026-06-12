using Application.Features.Auth.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _dbContext;

    public SessionRepository(AppDbContext dbContext)
        => this._dbContext = dbContext;

    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await this._dbContext.Sessions
            .Where(s => s.Id == id && s.ExpiresAt > DateTime.UtcNow)
            .SingleOrDefaultAsync(ct);

    public async Task<int> CountActiveByUserAsync(Guid userId, CancellationToken ct = default)
        => await this._dbContext.Sessions
            .Where(s => s.UserId == userId)
            .CountAsync(ct);

    public void Add(Session session)
        => this._dbContext.Sessions.Add(session);

    public async Task<bool> ExecuteVisitAsync(Guid sessionId, CancellationToken ct = default)
        => await this._dbContext.Sessions
            .Where(s => s.Id == sessionId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(s => s.LastSeen, DateTime.UtcNow),
                ct
            ) == 1;

    public void Delete(Session session)
        => this._dbContext.Sessions.Remove(session);

    public async Task DirectDeleteMostInactiveByUser(Guid userId, CancellationToken ct = default)
    {
        var oldestId = await this._dbContext.Sessions
            .Where(s => s.UserId == userId && s.ExpiresAt > DateTime.UtcNow)
            .OrderBy(s => s.LastSeen)
            .Select(s => s.Id)
            .FirstOrDefaultAsync(ct);

        if (oldestId != Guid.Empty)
        {
            await this._dbContext.Sessions
                .Where(s => s.Id == oldestId)
                .ExecuteDeleteAsync(ct);
        }
    }

    public async Task DirectDeleteExpiredAsync(CancellationToken ct = default)
    => await this._dbContext.Sessions
        .Where(s => s.ExpiresAt <= DateTime.UtcNow)
        .ExecuteDeleteAsync(ct);
}
