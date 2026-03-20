using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.DTOs.Categories;
using Api.DTOs.Keywords;
using Api.DTOs;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categoriesDtos = await _dbContext.Categories
                .Include(c => c.Keywords)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Keywords = c.Keywords.Select(k => new KeywordResponseDto
                    {
                        Id = k.Id,
                        Value = k.Value,
                        CategoryId = c.Id
                    }).ToList(),
                    Interval = (IntervalDto)c.Interval
                })
                .ToListAsync();

            return Ok(categoriesDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            var categoryDto = await _dbContext.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Keywords = c.Keywords.Select(k => new KeywordResponseDto
                    {
                        Id = k.Id,
                        Value = k.Value,
                        CategoryId = c.Id
                    }).ToList(),
                    Interval = (IntervalDto)c.Interval
                })
                .FirstOrDefaultAsync();

            if (categoryDto == null)
            {
                return NotFound();
            }
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            var newCategory = new Category
            {
                Name = categoryDto.Name,
                Interval = (Interval)categoryDto.Interval,
                UserId = 1,     // TODO
            };
            _dbContext.Categories.Add(newCategory);

            // Keywords
            var newKeywords = categoryDto.Keywords.Select(k => new Keyword
            {
                Value = k.Value,
                CategoryId = k.CategoryId,
            });
            foreach (var kw in newKeywords)
            {
                _dbContext.Keywords.Add(kw);
            }

            await _dbContext.SaveChangesAsync();

            var createdCategoryDto = new CategoryResponseDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Keywords = newKeywords.Select(k => new KeywordResponseDto
                {
                    Id = k.Id,
                    Value = k.Value,
                    CategoryId = k.CategoryId
                }).ToList(),
                Interval = (IntervalDto)newCategory.Interval
            };

            return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Id }, createdCategoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto userDto)
        {
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            return NoContent();
        }
    }
}
