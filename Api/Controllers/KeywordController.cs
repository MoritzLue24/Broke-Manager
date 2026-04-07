using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Keywords;
using Api.Services.Keywords;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Api.Exceptions;


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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<KeywordResponseDto>> CreateKeyword(
            [FromRoute] int categoryId,
            [FromBody] KeywordCreateDto createDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            return await _keywordService.CreateAsync(userId, categoryId, createDto);
        }

        [Authorize]
        [HttpPut("{keywordId}")]
        public async Task<ActionResult> UpdateKeyword(
            [FromRoute] int categoryId,
            [FromRoute] int keywordId,
            [FromBody] KeywordUpdateDto updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await _keywordService.UpdateAsync(userId, categoryId, keywordId, updateDto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteAll(
            [FromRoute] int categoryId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await _keywordService.DeleteAllAsync(userId, categoryId);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{keywordId}")]
        public async Task<ActionResult> DeleteKeyword(
            [FromRoute] int categoryId,
            [FromRoute] int keywordId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await _keywordService.DeleteByIdAsync(userId, categoryId, keywordId);
            return NoContent();
        }
    }
}