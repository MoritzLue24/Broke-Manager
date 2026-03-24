using Api.DTOs.Keywords;


namespace Api.Services.Keywords
{
    public interface IKeywordService
    {
        Task<KeywordResponseDto> CreateAsync(int userId, int categoryId, KeywordCreateDto createDto);
        Task UpdateAsync(int userId, int categoryId, int keywordId, KeywordUpdateDto updateDto);
        Task DeleteAllAsync(int userId, int categoryId);
        Task DeleteByIdAsync(int userId, int categoryId, int keywordId);
    }
}