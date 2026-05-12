using Domain.Entities;

namespace Application.Features.Transactions;

public static class TransactionExtension
{
    public static TransactionDto ToDto(this Transaction transaction)
        => new(
            transaction.Id,
            transaction.UserId,
            transaction.CategoryId,
            transaction.CategorySource,
            transaction.Amount,
            transaction.Type,
            transaction.Date,
            transaction.Title,
            transaction.Description,
            transaction.CounterParty,
            transaction.CreatedAt
        );
}