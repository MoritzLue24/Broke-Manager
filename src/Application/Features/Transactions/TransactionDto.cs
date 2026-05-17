using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Transactions;

/// Basic response dto. Other dtos like 
/// CreateDto, UpdateDto are now Commands / Queries.
/// Maybe later more Dtos, like TransactionDetailDto
public record TransactionDto(
    Guid Id,
    Guid UserId,
    Guid CategoryId,
    CategorySource CategorySource,
    decimal Amount,
    TransactionType Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty,
    DateTime CreatedAt
);

public static class TransactionExtension {
    public static TransactionDto ToDto(this Transaction t)
    {
        return new(
            t.Id,
            t.UserId,
            t.CategoryId,
            t.CategorySource,
            t.Amount,
            t.Type,
            t.Date,
            t.Title,
            t.Description,
            t.CounterParty,
            t.CreatedAt
        );
    }
}