using Api.DTOs.Keywords;
using Api.Exceptions;


namespace Api.Services.Keywords
{
    public interface IKeywordService
    {
        /// <summary>
        /// Creates a new keyword for a given Category, which has to be owned by specified user.
        /// </summary>
        /// <param name="userId">The owner / user id</param>
        /// <param name="categoryId">The category id to add the new keyword to</param>
        /// <param name="createDto">The new keyword to create</param>
        /// <returns>Async Task with the newly created keyword dto (containing its id)</returns>
        /// <exception cref="NotFoundException">If the category does not exist / is not owned by given user</exception>
        /// <exception cref="AlreadyExistsException">If the keyword already exists</exception>
        Task<KeywordResponseDto> CreateAsync(int userId, int categoryId, KeywordCreateDto createDto);
        /// <summary>
        /// Updates an existing keyword of a given category owned by specified user.
        /// </summary>
        /// <param name="userId">The owner of category / user id</param>
        /// <param name="categoryId">The category id where the keyword is from</param>
        /// <param name="keywordId">The keyword to change</param>
        /// <param name="updateDto">The properties to update the keyword with</param>
        /// <returns>Async Task</returns>
        /// <exception cref="NotFoundException">If the keyword does not exist / is not owned by given user OR the keyword is not from given category</exception>
        /// <exception cref="AlreadyExistsException">If the keyword already exists</exception>
        Task UpdateAsync(int userId, int categoryId, int keywordId, KeywordUpdateDto updateDto);
        /// <summary>
        /// Delete all keywords of a given category, owned by given user. (dangerous)
        /// </summary>
        /// <param name="userId">The owner of the category</param>
        /// <param name="categoryId">The category to delete all keywords from</param>
        /// <returns>Async task</returns>
        /// <exception cref="NotFoundException">If the category does not exist / is not owned by given user</exception>
        Task DeleteAllAsync(int userId, int categoryId);
        /// <summary>
        /// Delete specific category owned by given user.
        /// </summary>
        /// <param name="userId">The owner of category</param>
        /// <param name="categoryId">The category to delete the keyword from</param>
        /// <param name="keywordId">The keyword to delete</param>
        /// <returns>Async task</returns>
        /// <exception cref="NotFoundException">If the category does not exist / is not owned by given user OR if the keyword is not from given category</exception>
        Task DeleteByIdAsync(int userId, int categoryId, int keywordId);
    }
}