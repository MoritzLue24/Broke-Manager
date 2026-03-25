using Api.DTOs.Transactions;


namespace Api.Services.Transaction
{
    public interface ITransactionService
    {
        Task<List<TransactionResponseDto>> GetAllTransactionsForUserAsync(int userId);
        Task<TransactionResponseDto?> GetTransactionByIdAsync(int userId, int transactionId);
        Task<TransactionResponseDto> CreateTransactionAsync(int userId, TransactionCreateDto dto);
        Task<bool> UpdateTransactionAsync(int userId, int id, TransactionUpdateDto dto);
        Task<bool> DeleteTransactionAsync(int userId, int id);
        Task<int> DeleteAllTransactionsForUserAsync(int userId);
    }
}