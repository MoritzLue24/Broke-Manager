using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.DTOs.Categories;
using Api.DTOs.Keywords;
using Api.DTOs;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {
            // TODO: Check if c.userId equals current userId
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

            return categoriesDtos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory([FromRoute] int id)
        {
            // TODO: Check if c.userId equals current userId
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
            return categoryDto;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryCreateDto createDto)
        {
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto updateDto)
        {
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllCategories()
        {
            // TODO: Get Current UserId
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            return NoContent();
        }

        [HttpDelete("{id}/keywords")]
        public async Task<ActionResult> DeleteKeywords([FromRoute] int id)
        {
            return NoContent();
        }
    }
}
