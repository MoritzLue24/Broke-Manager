using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.DTOs.Transactions;
using System.Security.Claims;
using Api.Services.Transaction;



namespace Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize] 
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // Hilfsmethode, um die UserId aus JWT-Claims zu extrahieren
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdClaim, out int userId);
            return userId;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionResponseDto>>> GetAll()
        {
            int userId = GetCurrentUserId();
            var transactions = await _transactionService.GetAllTransactionsForUserAsync(userId);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseDto>> GetById([FromRoute] int id)
        {
            int userId = GetCurrentUserId();
            var transaction = await _transactionService.GetTransactionByIdAsync(userId, id);

            if (transaction == null)
            {
                return NotFound(new { message = "No transaction found or you don't have access to it." });
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> Create([FromBody] TransactionCreateDto dto)
        {
            int userId = GetCurrentUserId();
            var result = await _transactionService.CreateTransactionAsync(userId, dto);
            
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] TransactionUpdateDto dto)
        {
            int userId = GetCurrentUserId();
            await _transactionService.UpdateTransactionAsync(userId, id, dto);

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            int userId = GetCurrentUserId();
            await _transactionService.DeleteTransactionAsync(userId, id);


            return NoContent();
        }

        [HttpDelete("all")]
        public async Task<ActionResult> DeleteAll()
        {
            int userId = GetCurrentUserId();
            int deletedCount = await _transactionService.DeleteAllTransactionsForUserAsync(userId);
            
            return Ok(new { message = $"Successfully deleted transactions: {deletedCount}" });
        }
    


        [HttpPost("import")]
        public async Task<ActionResult> ImportCSV(IFormFile file)
        {
            await Task.CompletedTask;
            return NoContent();
        }
    }
}