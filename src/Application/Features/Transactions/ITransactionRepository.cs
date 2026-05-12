using Domain.Entities;

namespace Application.Features.Transactions;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    void Add(Transaction transaction);
}