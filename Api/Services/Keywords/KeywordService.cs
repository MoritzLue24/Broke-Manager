using Api.Data;
using Api.DTOs.Keywords;
using Api.Exceptions;
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

        public async Task<KeywordResponseDto> CreateAsync(
            int userId,
            int categoryId,
            KeywordCreateDto createDto)
        {
            if (!await _dbContext.Categories.AnyAsync(c => c.Id == categoryId && c.UserId == userId))
                throw new NotFoundException("Category not found");

            if (await _dbContext.Keywords.AnyAsync(k => k.Value == createDto.Value))
                throw new AlreadyExistsException($"Keyword '{createDto.Value}' already exists");

            Keyword newKeyword = new Keyword
            {
                Value = createDto.Value,
                CategoryId = categoryId
            };
            await _dbContext.Keywords.AddAsync(newKeyword);
            await _dbContext.SaveChangesAsync();

            return new KeywordResponseDto
            {
                Id = newKeyword.Id,
                Value = newKeyword.Value,
                CategoryId = newKeyword.CategoryId
            };
        }

        public async Task UpdateAsync(
            int userId,
            int categoryId,
            int keywordId,
            KeywordUpdateDto updateDto)
        {
            var keyword = await _dbContext.Keywords     // FIXME: Vllt fehlt include()?
                .SingleOrDefaultAsync(
                    k => k.Id == keywordId && 
                    k.CategoryId == categoryId && 
                    k.Category.UserId == userId)
                ?? throw new NotFoundException("Keyword not found");
            
            if (await _dbContext.Keywords.AnyAsync(k => k.Value == updateDto.Value))
                throw new AlreadyExistsException($"Keyword '{updateDto.Value}' already exists");

            if (updateDto.Value != null)
                keyword.Value = updateDto.Value;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(int userId, int categoryId)
        {
            var category = await _dbContext.Categories
                .Include(c => c.Keywords)
                .Where(c => c.Id == categoryId && c.UserId == userId)
                .SingleOrDefaultAsync()
                ?? throw new NotFoundException("Category not found");

            _dbContext.Keywords.RemoveRange(category.Keywords);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int userId, int categoryId, int keywordId)
        {
            var keyword = await _dbContext.Keywords     // FIXME: Vllt fehlt include()?
                .SingleOrDefaultAsync(k => k.Id == keywordId && k.CategoryId == categoryId && k.Category.UserId == userId)
                ?? throw new NotFoundException("Keyword not found");
            _dbContext.Keywords.Remove(keyword);
            await _dbContext.SaveChangesAsync();
        }
    }
}