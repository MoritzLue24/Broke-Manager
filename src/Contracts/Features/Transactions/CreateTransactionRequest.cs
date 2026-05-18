namespace Contracts.Features.Transactions;

public record CreateTransactionRequest(
    Guid? CategoryId,
    decimal Amount,
    string Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty
);