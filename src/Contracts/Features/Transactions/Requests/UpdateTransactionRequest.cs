namespace Contracts.Features.Transactions.Requests;

public record UpdateTransactionRequest(
    Guid? CategoryId,
    decimal? Amount,
    string? Type,
    DateOnly? Date,
    string? Title,
    string? Description,
    string? CounterParty
);
