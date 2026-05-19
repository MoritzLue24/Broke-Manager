using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<Transaction>> GetAllByUserId(Guid userId, CancellationToken ct)
        => await this._dbContext.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync(ct);

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct)
        => await this._dbContext.Transactions.FindAsync([id], ct);

    public void Add(Transaction transaction)
        => this._dbContext.Transactions.Add(transaction);
}
