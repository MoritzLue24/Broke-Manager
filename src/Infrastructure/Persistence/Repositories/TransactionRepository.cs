using Application.Features.Transactions;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Transaction?> GetByIdAsync(Guid id)
        => await _dbContext.Transactions.FindAsync(id);

    public void Add(Transaction transaction)
        => _dbContext.Transactions.Add(transaction);
}