using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Keywords;
using Api.Services.Keywords;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/categories/{categoryId}/keywords")]
    public class KeywordController : ControllerBase
    {
        private readonly IKeywordService _keywordService;
        
        public KeywordController(IKeywordService keywordService)
        {
            _keywordService = keywordService;
        }

        [HttpPost]
        public async Task<ActionResult<KeywordResponseDto>> CreateKeyword(
            [FromRoute] int categoryId,
            [FromBody] KeywordCreateDto createDto)
        {
            int userId = 1;
            return await _keywordService.CreateAsync(userId, categoryId, createDto);
        }

        [HttpPut("{keywordId}")]
        public async Task<ActionResult> UpdateKeyword(
            [FromRoute] int categoryId,
            [FromRoute] int keywordId,
            [FromBody] KeywordUpdateDto updateDto)
        {
            int userId = 1;
            await _keywordService.UpdateAsync(userId, categoryId, keywordId, updateDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAll(
            [FromRoute] int categoryId)
        {
            int userId = 1;
            await _keywordService.DeleteAllAsync(userId, categoryId);
            return Ok();
        }

        [HttpDelete("{keywordId}")]
        public async Task<ActionResult> DeleteKeyword(
            [FromRoute] int categoryId,
            [FromRoute] int keywordId)
        {
            int userId = 1;
            await _keywordService.DeleteByIdAsync(userId, categoryId, keywordId);
            return Ok();
        }
    }
}