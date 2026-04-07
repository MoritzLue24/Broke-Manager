using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Categories;
using Api.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Api.Exceptions;


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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            return await _categoryService.GetAllByUserAsync(userId);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(
            [FromRoute] int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            return await _categoryService.GetByIdAsync(userId, id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory(
            [FromBody] CategoryCreateDto createDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            return await _categoryService.CreateAsync(userId, createDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(
            [FromRoute] int id,
            [FromBody] CategoryUpdateDto updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await _categoryService.UpdateAsync(userId, id, updateDto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteAllCategories()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await _categoryService.DeleteAllByUserAsync(userId);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(
            [FromRoute] int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await _categoryService.DeleteByIdAsync(userId, id);
            return NoContent();
        }
    }
}
