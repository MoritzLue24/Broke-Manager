using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<Transaction>> GetAllByUserId(Guid userId, CancellationToken ct);    // TODO: not all

    void Add(Transaction transaction);
}