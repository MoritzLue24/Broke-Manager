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

    public async Task<List<Category>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await this._dbContext.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync(ct);

    public async Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken ct = default)
        => await this._dbContext.Categories
            .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync(ct);

    public async Task<Guid?> GetDefaultIdByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        Category? category = await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.IsDefault)
            .FirstOrDefaultAsync(ct);

        if (category is null)
            return null;
        return category.Id;
    }

    public async Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId, CancellationToken ct = default)
        => await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.Id == categoryId)
            .AnyAsync(ct);

    public async Task<bool> NameExistsForUserAsync(Guid userId, string name, CancellationToken ct = default)
        => await this._dbContext.Categories
            .Where(c => c.UserId == userId && c.Name == name)
            .AnyAsync(ct);

    public void Add(Category category)
        => this._dbContext.Categories.Add(category);

    public void Delete(Category category)
        => this._dbContext.Categories.Remove(category);
}
