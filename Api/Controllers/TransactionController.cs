using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Transactions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Api.Exceptions;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {

        public TransactionController()
        {
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<TransactionResponseDto>>> GetAllTransactions()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseDto>> GetTransaction([FromRoute] int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> CreateTransaction([FromBody] TransactionCreateDto createDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionUpdateDto updateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteAllTransactions()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpPost("import")]
        public async Task<ActionResult> ImportCSV(IFormFile file)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }
    }
}