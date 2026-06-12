using Application.Common.Interfaces.Persistence;
using Application.Features.Auth.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _dbContext;

    public SessionRepository(AppDbContext dbContext)
        => this._dbContext = dbContext;

    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await this._dbContext.Sessions
            .Where(s => s.Id == id)
            .SingleOrDefaultAsync(ct);

    public void Add(Session session)
        => this._dbContext.Sessions.Add(session);

    public void Delete(Session session)
        => this._dbContext.Sessions.Remove(session);

    public async Task DirectDeleteExpiredAsync(CancellationToken ct = default)
    => await this._dbContext.Sessions
        .Where(s => s.ExpiresAt <= DateTime.UtcNow)
        .ExecuteDeleteAsync(ct);
}
