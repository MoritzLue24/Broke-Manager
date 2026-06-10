using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Common;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    Guid TransactionId,
    Guid? CategoryId,
    decimal? Amount,
    string? Type,
    DateOnly? Date,
    string? Title,
    string? Description,
    string? CounterParty
) : IRequest<Result<TransactionResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
