using Api.DTOs.Categories;


namespace Api.Services.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllByUserAsync(int userId);
        Task<CategoryResponseDto> GetByIdAsync(int userId, int categoryId);
        Task<CategoryResponseDto> CreateAsync(int userId, int categoryId, CategoryCreateDto createDto);
        void UpdateAsync(int userId, int categoryId, CategoryUpdateDto updateDto);
        void DeleteAllByUserAsync(int userId);
        void DeleteByIdAsync(int userId, int categoryId);
        void DeleteKeywordsByCategoryAsync(int userId, int categoryId);
    }
}