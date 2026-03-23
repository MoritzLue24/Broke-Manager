using Microsoft.AspNetCore.Mvc;
using Api.Data;
using Api.DTOs.Keywords;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/keywords")]
    public class KeywordController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public KeywordController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<KeywordResponseDto>> CreateKeyword([FromBody] KeywordCreateDto createDto)
        {
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateKeyword(int id, [FromBody] KeywordUpdateDto updateDto)
        {
            // TODO: Check Keyword-Owner
            var keyword = await _dbContext.Keywords.FindAsync(id);
            if (keyword == null)
            {
                return NotFound();
            }
            keyword.Value = updateDto.Value;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteKeyword(int id)
        {
            // TODO: Check Owner
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