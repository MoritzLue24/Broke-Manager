using Api.DTOs.Keywords;


namespace Api.Services.Keywords
{
    public interface IKeywordService
    {
        Task<KeywordResponseDto> CreateAsync(int userId, KeywordCreateDto createDto);
        void UpdateAsync(int userId, int keywordId, KeywordUpdateDto updateDto);
        void DeleteById(int userId, int keywordId);
    }
}