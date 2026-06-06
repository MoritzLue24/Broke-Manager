using Application.Common.Interfaces.Security;
using Domain.Common;
using Domain.Enums;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(
    Guid Id
) : IRequest<Result<Unit>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
