using Microsoft.AspNetCore.Mvc;
using Api.DTOs.Transactions;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {

        public TransactionController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionResponseDto>>> GetAllTransactions()
        {
            await Task.CompletedTask;
            return NoContent();
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseDto>> GetTransaction([FromRoute] int id)
        {
            await Task.CompletedTask;
            return NoContent();
        }
    
        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> CreateTransaction([FromBody] TransactionCreateDto createDto)
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionUpdateDto updateDto)
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllTransactions()
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            await Task.CompletedTask;
            return NoContent();
        }

        [HttpPost("import")]
        public async Task<ActionResult> ImportCSV(IFormFile file)
        {
            await Task.CompletedTask;
            return NoContent();
        }
    }
}