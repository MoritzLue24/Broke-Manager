using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Common;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public record GetTransactionsByUserQuery(   // TODO: with pages etc.
) : IRequest<Result<List<TransactionResult>>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
