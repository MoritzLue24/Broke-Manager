using Domain.Entities;

namespace Application.Features.Categories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken ct = default);

    Task<List<Category>> GetAllWithIdsAsync(Guid userId, IReadOnlyCollection<Guid> categoryIds, CancellationToken ct = default);

    Task<List<Category>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);

    /// Gets the default category by userid
    Task<Guid?> GetDefaultIdByUserIdAsync(Guid userId, CancellationToken ct = default);

    /// Check if a category for the user exists
    Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId, CancellationToken ct = default);
    Task<bool> NameExistsForUserAsync(Guid userId, string name, CancellationToken ct = default);

    void Add(Category category);

    void Delete(Category category);
}
