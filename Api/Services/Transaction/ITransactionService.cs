using Api.DTOs.Transactions;


namespace Api.Services.Transaction
{
    public interface ITransactionService
    {
        Task<List<TransactionResponseDto>> GetAllForUserAsync(int userId);
        Task<TransactionResponseDto?> GetByIdAsync(int userId, int transactionId);
        Task<TransactionResponseDto> CreateAsync(int userId, TransactionCreateDto dto);
        Task<bool> UpdateAsync(int userId, int id, TransactionUpdateDto dto);
        Task<bool> DeleteAsync(int userId, int id);
        Task<int> DeleteAllForUserAsync(int userId);
    }
}