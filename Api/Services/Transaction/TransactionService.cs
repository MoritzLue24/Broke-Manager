using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.DTOs.Transactions;

namespace Api.Services.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _dbContext;
        public TransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransactionResponseDto>> GetAllTransactionsForUserAsync(int userId)
        {
            return await _dbContext.Transactions
                .Where(t => t.UserId == userId)
                .Select(t => new TransactionResponseDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    CounterParty = t.CounterParty,
                    Title = t.Title,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category != null ? t.Category.Name : "Other" // hier checke ich nicht so ganz was dann gemacht werden soll
                })
                .ToListAsync();
        }   

        public async Task<TransactionResponseDto?> GetTransactionByIdAsync(int userId, int transactionId)
        {
            var transaction = await _dbContext.Transactions
                .Where(t => t.UserId == userId && t.Id == transactionId)
                .Select(t => new TransactionResponseDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    CounterParty = t.CounterParty,
                    Title = t.Title,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category != null ? t.Category.Name : "Other" // hier checke ich nicht so ganz was dann gemacht werden soll
                })
                .FirstOrDefaultAsync();

            return transaction;
        }

        public async Task<TransactionResponseDto> CreateTransactionAsync(int userId, TransactionCreateDto dto)
        {
            var transaction = new Models.Transaction
            {
                UserId = userId,
                Date = dto.Date,
                Amount = dto.Amount,
                CounterParty = dto.CounterParty,
                Title = dto.Title,
                CategoryId = dto.CategoryId
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            return new TransactionResponseDto
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Amount = transaction.Amount,
                CounterParty = transaction.CounterParty,
                Title = transaction.Title,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category != null ? transaction.Category.Name : "Other" // hier checke ich nicht so ganz was dann gemacht werden soll
            };
        }

        public async Task<bool> UpdateTransactionAsync(int userId, int id, TransactionUpdateDto dto)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);
            if (transaction == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(dto.Title))
            {
                transaction.Title = dto.Title;
            }

            if (!string.IsNullOrEmpty(dto.CounterParty))
            {
                transaction.CounterParty = dto.CounterParty;
            }

            if (dto.Amount.HasValue)
            {
                transaction.Amount = dto.Amount.Value;
            }

            if (dto.Date.HasValue)
            {
                transaction.Date = dto.Date.Value;
            }

            
            if (dto.CategoryId.HasValue)
            {
                transaction.CategoryId = dto.CategoryId.Value;
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTransactionAsync(int userId, int id)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);
            if (transaction == null)
            {
                return false;
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteAllTransactionsForUserAsync(int userId)
        {
            var transactions = await _dbContext.Transactions.Where(t => t.UserId == userId).ToListAsync();
            int count = transactions.Count;

            _dbContext.Transactions.RemoveRange(transactions);
            await _dbContext.SaveChangesAsync();

            return count;
    
        }
    
    
    }
}