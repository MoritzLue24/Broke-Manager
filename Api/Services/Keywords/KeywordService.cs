using System.Threading.Tasks;
using Api.Data;
using Api.DTOs.Keywords;
using Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Api.Services.Keywords
{
    public class KeywordService : IKeywordService
    {
        private readonly AppDbContext _dbContext;

        public KeywordService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<KeywordResponseDto> CreateAsync(int userId, KeywordCreateDto createDto)
        {
            var category = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == createDto.CategoryId)
                ?? throw new KeyNotFoundException("Category not found");
            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category is not owned by specified user");
            }

            Keyword newKeyword = new Keyword
            {
                Value = createDto.Value,
                CategoryId = createDto.CategoryId
            };
            var keywordResponse = (await _dbContext.Keywords.AddAsync(newKeyword)).Entity;
            await _dbContext.SaveChangesAsync();

            return new KeywordResponseDto
            {
                Id = keywordResponse.Id,
                Value = keywordResponse.Value,
                CategoryId = keywordResponse.CategoryId
            };
        }

        public async void UpdateAsync(int userId, int keywordId, KeywordUpdateDto updateDto)
        {
            var keyword = await _dbContext.Keywords
                .SingleOrDefaultAsync(k => k.Id == keywordId)
                ?? throw new KeyNotFoundException("Keyword not found");
            if (keyword.Category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category of keyword is not owned by specified user");
            }

            if (updateDto.Value != null)
            {
                keyword.Value = updateDto.Value;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async void DeleteByIdAsync(int userId, int keywordId)
        {
            var keyword = await _dbContext.Keywords
                .SingleOrDefaultAsync(k => k.Id == keywordId)
                ?? throw new KeyNotFoundException("Keyword not found");
            if (keyword.Category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category of keyword is not owned by specified user");
            }
            _dbContext.Keywords.Remove(keyword);
        }
    }
}