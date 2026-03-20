using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.DTOs.Keywords;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeywordController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public KeywordController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult> CreateKeyword([FromBody] KeywordCreateDto keywordDto)
        {
            var newKeyword = new Keyword
            {
                Value = keywordDto.Value,
                CategoryId = keywordDto.CategoryId
            };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateKeyword(int id, [FromBody] string keyword)
        {
            // TODO: Implement update keyword logic
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteKeyword(int id)
        {
            var keyword = await _dbContext.Keywords.FindAsync(id);
            if (keyword == null)
            {
                return NotFound();
            }
            _dbContext.Keywords.Remove(keyword);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}