using Domain.Entities;

namespace Application.Features.Auth.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<int> CountActiveByUserAsync(Guid userId, CancellationToken ct = default);

    void Add(Session session);

    Task<bool> ExecuteVisitAsync(Guid sessionId, CancellationToken ct = default);

    void Delete(Session session);

    Task ExecuteDeleteMostInactiveByUser(Guid userId, CancellationToken ct = default);

    Task ExecuteDeleteExpiredAsync(CancellationToken ct = default);

    Task DeleteAllByUserAsync(Guid userId, CancellationToken ct = default);
}
