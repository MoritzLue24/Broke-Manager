using Domain.Entities;

namespace Application.Features.Transactions;

/// Transaction specific repository, should not be used across features
public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    List<Transaction> GetAllByUserId(Guid userId);    // TODO: not all
    void Add(Transaction transaction);
}