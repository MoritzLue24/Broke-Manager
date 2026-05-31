using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken ct = default);

    /// Gets the default category by userid
    Task<Guid?> GetDefaultIdByUserIdAsync(Guid userId, CancellationToken ct = default);

    /// Check if a category for the user exists
    Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId, CancellationToken ct = default);

    void Add(Category category);
}
