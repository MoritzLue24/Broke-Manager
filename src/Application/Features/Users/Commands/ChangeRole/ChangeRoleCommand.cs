using Application.Common.Interfaces.Security;
using Application.Features.Users.Common;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Commands.ChangeRole;

public record ChangeRoleCommand(
    Guid UserId,
    string Role
) : IRequest<Result<UserResult>>, IRequireAuthorization
{
    public Role[] Roles => [Domain.Enums.Role.Admin];
}
