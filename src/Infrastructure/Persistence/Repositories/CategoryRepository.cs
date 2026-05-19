using Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Guid?> GetDefaultByUserIdAsync(Guid userId)
        => await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.IsDefault)
            .Select(c => (Guid?)c.Id)
            .FirstOrDefaultAsync();

    public async Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId)
        => await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.Id == categoryId)
            .AnyAsync();
}
