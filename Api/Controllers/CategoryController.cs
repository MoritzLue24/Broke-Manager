using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Categories;
using Api.Services.Categories;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {
            int userId = 1;
            return await _categoryService.GetAllByUserAsync(userId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(
            [FromRoute] int id)
        {
            int userId = 1;
            return await _categoryService.GetByIdAsync(userId, id);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory(
            [FromBody] CategoryCreateDto createDto)
        {
            int userId = 1;
            return await _categoryService.CreateAsync(userId, createDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(
            [FromRoute] int id,
            [FromBody] CategoryUpdateDto updateDto)
        {
            int userId = 1;
            await _categoryService.UpdateAsync(userId, id, updateDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllCategories()
        {
            int userId = 1;
            await _categoryService.DeleteAllByUserAsync(userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(
            [FromRoute] int id)
        {
            int userId = 1;
            await _categoryService.DeleteByIdAsync(userId, id);
            return NoContent();
        }
    }
}
