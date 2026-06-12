using Application.Common.Behaviors;
using Application.Features.Users.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Commands.UpdateCurrentUser;

public record UpdateCurrentUserCommand(
    string? Email
) : IRequest<Result<UserResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
