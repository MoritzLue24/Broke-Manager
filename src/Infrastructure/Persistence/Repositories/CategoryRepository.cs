using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Category?> GetById(Guid categoryId)
        => await this._dbContext.Categories
            .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

    public async Task<Guid?> GetDefaultIdByUserIdAsync(Guid userId)
    {
        Category? category = await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.IsDefault)
            .FirstOrDefaultAsync();

        if (category is null)
            return null;
        return category.Id;
    }

    public async Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId)
        => await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.Id == categoryId)
            .AnyAsync();
}
