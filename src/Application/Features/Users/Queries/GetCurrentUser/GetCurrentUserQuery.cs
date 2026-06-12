using Application.Common.Behaviors;
using Application.Features.Users.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IRequest<Result<UserResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
