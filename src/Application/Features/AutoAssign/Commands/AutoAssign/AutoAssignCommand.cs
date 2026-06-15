using Application.Common.Behaviors;
using Application.Features.AutoAssign.Contracts;
using Application.Features.Transactions.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.AutoAssign.Commands.AutoAssign;

public record AutoAssignCommand(
    TransactionFilter Filter,
    Guid[]? UseCategoryIds,
    bool? OverwriteManual   // Nullable because if not nullable -> not set -> automatically "false"
) : IRequest<Result<IReadOnlyCollection<AutoAssignResult>>>, IRequireAuthorization
{
    public Role[] Roles => [Role.User, Role.Admin];
}
