using Application.Common.Behaviors;
using Application.Features.Users.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery() : IRequest<Result<List<UserResult>>>, IRequireAuthorization
{
    // Only Admins can execute this command
    public Role[] Roles => [Role.Admin];
}
