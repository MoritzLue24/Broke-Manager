using Domain.Entities;

namespace Application.Features.Transactions;

/// Transaction specific repository, should not be used across features
public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    void Add(Transaction transaction);
}