using Application.Common.Interfaces.Persistence;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
        => await this._dbContext.SaveChangesAsync(ct);
}