using Microsoft.AspNetCore.Mvc;
using Api.Data;
using Api.DTOs.Keywords;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/categories/{categoryId}/keywords")]
    public class KeywordController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public KeywordController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<KeywordResponseDto>> CreateKeyword(
            [FromRoute] int categoryId,
            [FromBody] KeywordCreateDto createDto)
        {
            return NoContent();
        }

        [HttpPut("{keywordId}")]
        public async Task<ActionResult> UpdateKeyword(
            [FromRoute] int categoryId,
            [FromRoute] int keywordId,
            [FromBody] KeywordUpdateDto updateDto)
        {
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAll(
            [FromRoute] int categoryId)
        {
            return NoContent();
        }

        [HttpDelete("{keywordId}")]
        public async Task<ActionResult> DeleteKeyword(
            [FromRoute] int categoryId,
            [FromRoute] int keywordId)
        {
            return NoContent();
        }
    }
}