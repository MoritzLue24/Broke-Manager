using Domain.Entities;

namespace Application.Features.Transactions.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<List<Transaction>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);    // TODO: not all

    Task<List<Transaction>> GetAllByCategoryIdAsync(Guid categoryId, CancellationToken ct = default);

    void Add(Transaction transaction);

    void Delete(Transaction transaction);
}
