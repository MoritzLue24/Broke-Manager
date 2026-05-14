namespace Contracts.Features.Transactions;

public record TransactionDetailResponse(
    Guid Id,
    Guid UserId,
    Guid CategoryId,
    string CategorySource,
    decimal Amount,
    string Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty,
    DateTime CreatedAt
);