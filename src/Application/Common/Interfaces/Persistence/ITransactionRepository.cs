using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<List<Transaction>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);    // TODO: not all

    void Add(Transaction transaction);
}
