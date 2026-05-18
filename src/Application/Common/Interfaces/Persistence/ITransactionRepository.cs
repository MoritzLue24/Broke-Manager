using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    List<Transaction> GetAllByUserId(Guid userId);    // TODO: not all
    void Add(Transaction transaction);
}