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
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(
            [FromRoute] int id)
        {
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory(
            [FromBody] CategoryCreateDto createDto)
        {
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(
            [FromRoute] int id,
            [FromBody] CategoryUpdateDto updateDto)
        {
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllCategories()
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(
            [FromRoute] int id)
        {
            return NoContent();
        }
    }
}
