using Application.Common.Interfaces;

namespace Infrastructure.Persistence;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}