using Contracts.Features.Transactions.Requests;

namespace Contracts.Features.AutoAssign.Requests;

public record AutoAssignRequest (
    TransactionFilterRequest Filter,
    Guid[]? UseCategoryIds,
    bool? OverwriteManual   // Nullable because if not nullable -> not set -> automatically "false"
);