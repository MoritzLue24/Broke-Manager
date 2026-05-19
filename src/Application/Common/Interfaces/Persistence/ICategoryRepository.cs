namespace Application.Common.Interfaces.Persistence;

public interface ICategoryRepository
{
    /// Gets the default category by userid
    Task<Guid?> GetDefaultByUserIdAsync(Guid userId);

    /// Check if a category for the user exists
    Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId);
}
