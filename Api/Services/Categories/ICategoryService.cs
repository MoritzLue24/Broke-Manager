using Api.DTOs.Categories;


namespace Api.Services.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllByUserAsync(int userId);
        Task<CategoryResponseDto> GetByIdAsync(int userId, int categoryId);
        Task<CategoryResponseDto> CreateAsync(int userId, CategoryCreateDto createDto);
        Task UpdateAsync(int userId, int categoryId, CategoryUpdateDto updateDto);
        Task DeleteAllByUserAsync(int userId);
        Task DeleteByIdAsync(int userId, int categoryId);
    }
}