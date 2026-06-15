using Contracts.Features.Transactions.Responses;

namespace Contracts.Features.AutoAssign.Responses;

public record AutoAssignResponse(
    TransactionResponse Transaction,
    CategoryConflictResponse[]? ConflictingCategories
);
