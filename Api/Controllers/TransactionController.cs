using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.DTOs.Transactions;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TransactionController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionResponseDto>>> GetAllTransactions()
        {
            int userId = 0;     // TODO
            var transactions = await _dbContext.Transactions
                .Where(t => t.UserId == userId)
                .Select(t => new TransactionResponseDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    CounterParty = t.CounterParty,
                    Title = t.Title,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name 
                })
                .ToListAsync();

            return transactions;
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseDto>> GetTransaction([FromRoute] int id)
        {
            // TODO: Check if transaction.userId equals current userId

            var transaction = await _dbContext.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            var transactionDto = new TransactionResponseDto
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Amount = transaction.Amount,
                CounterParty = transaction.CounterParty,
                Title = transaction.Title,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.Name
            };
            return transactionDto;
        }
    
        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> CreateTransaction([FromBody] TransactionCreateDto createDto)
        {
            var defaultCategory = await _dbContext.Categories
                .Where(c => c.IsDefault)
                .FirstOrDefaultAsync();

            if (defaultCategory == null && createDto.CategoryId == null)
            {
                return BadRequest("Default category not found. Please create a default category before adding transactions.");
            }

            // TODO

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionUpdateDto updateDto)
        {
             var defaultCategory = await _dbContext.Categories
                .Where(c => c.IsDefault)
                .FirstOrDefaultAsync();

            if (defaultCategory == null && updateDto.CategoryId == null)
            {
                return BadRequest("Default category not found. Please create a default category before adding transactions.");
            }

            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)            
            {
                return NotFound();
            }

            // TODO

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
            // TODO: Check if transaction.userId equals current userId
            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
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