using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryReaderRepository : ICategoryReaderRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryReaderRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Guid?> GetDefaultByUserIdAsync(Guid userId)
    {
        return await _dbContext.Categories
            .Where(c => c.UserId == userId && c.IsDefault)
            .Select(c => (Guid?)c.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId)
    {
        return await _dbContext.Categories
            .Where(c => c.UserId == userId && c.Id == categoryId)
            .AnyAsync();
    }
}