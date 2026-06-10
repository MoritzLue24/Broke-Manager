using Application.Common.Interfaces.Security;
using Domain.Common;
using Domain.Enums;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Result<Unit>>, IRequireAuthorization
{
    public Role[] Roles => [Role.Admin];
}
