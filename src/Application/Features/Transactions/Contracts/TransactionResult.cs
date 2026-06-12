using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Transactions.Contracts;

/// Basic transaction result. Other dtos like 
/// CreateDto, UpdateDto are now Commands / Queries.
/// Maybe later more Dtos, like TransactionDetailResult
public record TransactionResult(
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

public static class TransactionExtension
{
    public static TransactionResult ToResult(this Transaction t)
        => new(
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
