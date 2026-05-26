using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task<Category?> GetById(Guid categoryId);

    /// Gets the default category by userid
    Task<Guid?> GetDefaultIdByUserIdAsync(Guid userId);

    /// Check if a category for the user exists
    Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId);
}
