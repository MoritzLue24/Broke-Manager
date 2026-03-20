using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.DTOs.Transactions;

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
    }
}