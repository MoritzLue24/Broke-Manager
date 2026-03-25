using Api.DTOs.Categories;


namespace Api.Services.Categories
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get all categories owned by specified user.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <returns>Async Task containing a List of CategoryResponseDtos</returns>
        Task<List<CategoryResponseDto>> GetAllByUserAsync(int userId);
        /// <summary>
        /// Get a specific category owned by specified user.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="categoryId">The category id</param>
        /// <returns>Async Task containing the CategoryResponseDto</returns>
        /// <exception cref="KeyNotFoundException">If the category does not exist / is not owned by specified user.</exception>
        Task<CategoryResponseDto> GetByIdAsync(int userId, int categoryId);
        /// <summary>
        /// Create a new category with the given Dto.
        /// </summary>
        /// <param name="userId">The future owner of this new category</param>
        /// <param name="createDto">The properties of the new category</param>
        /// <returns>Async Task containing the newly created CategoryResponseDto, including its Id</returns>
        Task<CategoryResponseDto> CreateAsync(int userId, CategoryCreateDto createDto);
        /// <summary>
        /// Updates the properties of the given category, which has to be owned by the specified user.
        /// </summary>
        /// <param name="userId">The owner of this category</param>
        /// <param name="categoryId">The category id</param>
        /// <param name="updateDto">The properties to update (properties can be null to dont change)</param>
        /// <returns>Async Task</returns>
        /// <exception cref="KeyNotFoundException">If the category does not exist / is not owned by specified user.</exception>
        Task UpdateAsync(int userId, int categoryId, CategoryUpdateDto updateDto);
        /// <summary>
        /// Delete ALL categories owned by the specified user. (dangerous)
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <returns>Async Task</returns>
        Task DeleteAllByUserAsync(int userId);
        /// <summary>
        /// Delete only a single, specified category owned by given user.
        /// </summary>
        /// <param name="userId">The owner / user id</param>
        /// <param name="categoryId">The category id</param>
        /// <returns>Async task</returns>
        Task DeleteByIdAsync(int userId, int categoryId);
    }
}