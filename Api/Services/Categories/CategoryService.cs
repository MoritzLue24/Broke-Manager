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
            return (await _dbContext.Categories
                .Where(c => c.UserId == userId)
                .Include(c => c.Keywords)
                .ToListAsync())     // Wichtig, damit c.Interval richtig gemapped wird
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
                }).ToList();
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int userId, int categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == categoryId && c.UserId == userId)
                .Include(c => c.Keywords)
                .SingleOrDefaultAsync() 
                ?? throw new KeyNotFoundException("Category not found.");

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

        public async Task<CategoryResponseDto> CreateAsync(int userId, CategoryCreateDto createDto)
        {
            // Category
            Category newCategory = new Category
            {
                Name = createDto.Name,
                Interval = (Interval)createDto.Interval,
                IsDefault = false,
                UserId = userId
            };
            await _dbContext.Categories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();

            CategoryResponseDto responseDto = new CategoryResponseDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Keywords = [],
                Interval = (IntervalDto)newCategory.Interval,
                IsDefault = newCategory.IsDefault
            };

            // Keywords
            foreach (var keyword in createDto.Keywords)
            {
                Keyword newKeyword = new Keyword
                {
                    Value = keyword.Value,
                    CategoryId = responseDto.Id
                };
                await _dbContext.Keywords.AddAsync(newKeyword);
                await _dbContext.SaveChangesAsync();

                responseDto.Keywords.Add(new KeywordResponseDto
                {
                    Id = newKeyword.Id,
                    Value = newKeyword.Value,
                    CategoryId = newKeyword.CategoryId
                });
            }

            await _dbContext.SaveChangesAsync();
            return responseDto;
        }

        public async Task UpdateAsync(int userId, int categoryId, CategoryUpdateDto updateDto)
        {
            var category = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId) 
                ?? throw new KeyNotFoundException("Category not found.");

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

        public async Task DeleteAllByUserAsync(int userId)
        {
            _dbContext.RemoveRange(_dbContext.Categories.Where(c => c.UserId == userId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int userId, int categoryId)
        {
            var category = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId)
                ?? throw new KeyNotFoundException("Category not found");

            _dbContext.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}