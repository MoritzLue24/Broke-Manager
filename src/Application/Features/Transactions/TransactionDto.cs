using Domain.Enums;

namespace Application.Features.Transactions;

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