using Application.Features.Auth.Common;

namespace Application.Common.Interfaces.Persistence;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);

    void Add(Session session);

    void Delete(Session session);

    Task DirectDeleteExpiredAsync(CancellationToken ct = default);
}
