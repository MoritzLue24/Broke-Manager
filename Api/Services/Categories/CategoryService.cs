using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.DTOs.Categories;
using Api.DTOs.Keywords;
using Api.DTOs;
using Api.Models;


namespace Api.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryResponseDto>> GetAllByUserAsync(int userId)
        {
            return await _dbContext.Categories
                .Where(c => c.UserId == userId)
                // TODO: Automapper?
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Keywords = c.Keywords.Select(k => new KeywordResponseDto
                    {
                        Id = k.Id,
                        Value = k.Value,
                        CategoryId = k.CategoryId
                    }).ToList(),
                    Interval = (IntervalDto)c.Interval,
                    IsDefault = c.IsDefault
                }).ToListAsync();
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int userId, int categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == categoryId)
                .SingleOrDefaultAsync() 
                ?? throw new KeyNotFoundException("Category not found.");

            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category is not owned by specified user.");
            }

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Keywords = category.Keywords.Select(k => new KeywordResponseDto
                {
                    Id = k.Id,
                    Value = k.Value,
                    CategoryId = k.CategoryId
                }).ToList(),
                Interval = (IntervalDto)category.Interval,
                IsDefault = category.IsDefault
            };
        }

        public async Task<CategoryResponseDto> CreateAsync(int userId, int categoryId, CategoryCreateDto createDto)
        {
            // Category
            Category newCategory = new Category
            {
                Name = createDto.Name,
                Interval = (Interval)createDto.Interval,
                IsDefault = createDto.IsDefault,        // TODO: Always false?
                UserId = userId
            };
            var categoryResponse = (await _dbContext.Categories.AddAsync(newCategory)).Entity;

            CategoryResponseDto responseDto = new CategoryResponseDto
            {
                Id = categoryResponse.Id,
                Name = categoryResponse.Name,
                Keywords = [],
                Interval = (IntervalDto)categoryResponse.Interval,
                IsDefault = categoryResponse.IsDefault
            };

            // Keywords
            foreach (var keyword in createDto.Keywords)
            {
                Keyword newKeyword = new Keyword
                {
                    Value = keyword.Value,
                    CategoryId = keyword.CategoryId     // TODO: Use categoryId and change KeywordCreateDto??
                };
                var keywordResponse = (await _dbContext.Keywords.AddAsync(newKeyword)).Entity;
                responseDto.Keywords.Add(new KeywordResponseDto
                {
                    Id = keywordResponse.Id,
                    Value = keywordResponse.Value,
                    CategoryId = keywordResponse.CategoryId
                });
            }

            await _dbContext.SaveChangesAsync();
            return responseDto;
        }

        public async void UpdateAsync(int userId, int categoryId, CategoryUpdateDto updateDto)
        {
            var category = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId) 
                ?? throw new KeyNotFoundException("Category not found.");

            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category is not owned by specified user");
            }

            if (updateDto.Name != null)
            {
                category.Name = updateDto.Name;
            }
            if (updateDto.Interval != null)
            {
                category.Interval = (Interval)updateDto.Interval;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async void DeleteAllByUserAsync(int userId)
        {
            _dbContext.RemoveRange(_dbContext.Categories.Where(c => c.UserId == userId));
            await _dbContext.SaveChangesAsync();
        }

        public async void DeleteByIdAsync(int userId, int categoryId)
        {
            var category = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == userId)
                ?? throw new KeyNotFoundException("Category not found");

            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category not owned by specified user");
            } 
            _dbContext.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async void DeleteKeywordsByCategoryAsync(int userId, int categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == categoryId)
                .SingleOrDefaultAsync()
                ?? throw new KeyNotFoundException("Category not found");
            
            if (category.UserId != userId)
            {
                throw new UnauthorizedAccessException("Category not owned by specified user");
            }
            _dbContext.Keywords.RemoveRange(category.Keywords);
        }
    }
}