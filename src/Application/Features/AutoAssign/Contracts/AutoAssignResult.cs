using Application.Features.Transactions.Contracts;

namespace Application.Features.AutoAssign.Contracts;

public record AutoAssignResult(
    TransactionResult TransactionResult,
    IReadOnlyCollection<(Guid CategoryId, double Score)>? ConflictingCategories
);