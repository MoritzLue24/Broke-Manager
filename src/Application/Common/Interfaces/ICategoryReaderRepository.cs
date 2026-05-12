namespace Application.Common.Interfaces;

/// This exists to prevent direct dependency between features:
/// The feature "transactions" need the default repository, so we create this interface
/// We could give the "transactions" feature the full category repository,
/// but this would be too much responsibility over the "category feature"
public interface ICategoryReaderRepository
{
    /// Gets the default category by userid
    Task<Guid?> GetDefaultByUserIdAsync(Guid userId);

    /// Check if a category for the user exists
    Task<bool> ExistsForUserAsync(Guid userId, Guid categoryId);
}