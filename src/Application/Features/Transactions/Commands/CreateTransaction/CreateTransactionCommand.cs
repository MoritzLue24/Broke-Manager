using Application.Common.Behaviors;
using Application.Features.AutoAssign.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid? CategoryId,
    decimal Amount,
    string Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty
) : IRequest<Result<AutoAssignResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
