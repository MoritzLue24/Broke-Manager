using Application.Features.Transactions.Interfaces;
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

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await this._dbContext.Transactions.FindAsync([id], ct);

    public async Task<List<Transaction>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await this._dbContext.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync(ct);

    public async Task<List<Transaction>> GetAllByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
        => await this._dbContext.Transactions
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync(ct);

    public void Add(Transaction transaction)
        => this._dbContext.Transactions.Add(transaction);

    public void Delete(Transaction transaction)
        => this._dbContext.Transactions.Remove(transaction);
}
