using Domain.Entities;

namespace Application.Features.Auth.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<int> CountActiveByUserAsync(Guid userId, CancellationToken ct = default);

    void Add(Session session);

    void Delete(Session session);

    Task DirectDeleteOldestActiveByUser(Guid userId, CancellationToken ct = default);

    Task DirectDeleteExpiredAsync(CancellationToken ct = default);
}
