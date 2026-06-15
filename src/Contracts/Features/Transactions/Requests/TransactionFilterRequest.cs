namespace Contracts.Features.Transactions.Requests;

// Used in auto-assign & get-categories
public record TransactionFilterRequest (
    Guid[]? TransactionIds,
    Guid[]? CategoryIds,
    DateOnly? From,
    DateOnly? To
);