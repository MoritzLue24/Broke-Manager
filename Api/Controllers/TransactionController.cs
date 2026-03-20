using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.DTOs.Transactions;
using Api.DTOs.Categories;

namespace BrokeManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TransactionController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            
            var transactions = await _dbContext.Transactions
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

            return Ok(transactions);
        }
    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction([FromRoute] int id)
        {
            
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
            return Ok(transactionDto);
        }
    
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto transactionDto)
        {
            var defaultCategory = await _dbContext.Categories
                .Where(c => c.IsDefault)
                .FirstOrDefaultAsync();
            

            if (defaultCategory == null && transactionDto.CategoryId == null)
            {
                return BadRequest("Default category not found. Please create a default category before adding transactions.");
            }



            var newTransaction = new Transaction
            {
                Date = transactionDto.Date,
                Amount = transactionDto.Amount,
                CounterParty = transactionDto.CounterParty,
                Title = transactionDto.Title,
                CategoryId = transactionDto.CategoryId == null ? defaultCategory.Id : transactionDto.CategoryId,
                UserId = 1 // TODO: SPÄTER
            };

            _dbContext.Transactions.Add(newTransaction);
            await _dbContext.SaveChangesAsync();

            var createdTransactionDto = new TransactionResponseDto
            {
                Id = newTransaction.Id,
                Date = newTransaction.Date,
                Amount = newTransaction.Amount,
                CounterParty = newTransaction.CounterParty,
                Title = newTransaction.Title,
                CategoryId = newTransaction.CategoryId,
                CategoryName = newTransaction.Category.Name
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = newTransaction.Id }, createdTransactionDto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionUpdateDto transactionDto)
        {
             var defaultCategory = await _dbContext.Categories
                .Where(c => c.IsDefault)
                .FirstOrDefaultAsync();
            

            if (defaultCategory == null && transactionDto.CategoryId == null)
            {
                return BadRequest("Default category not found. Please create a default category before adding transactions.");
            }
            
            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)            
            {
                return NotFound();
            }

            

            transaction.Date = transactionDto.Date;
            transaction.Amount = transactionDto.Amount;
            transaction.CounterParty = transactionDto.CounterParty;
            transaction.Title = transactionDto.Title;
            
            if (transactionDto.CategoryId.HasValue)
            {
                transaction.CategoryId = transactionDto.CategoryId.Value;
            }
            else
            {
                transaction.CategoryId = defaultCategory.Id;
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}