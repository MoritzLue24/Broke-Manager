using Application.Common.Behaviors;
using Application.Features.Users.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<Result<UserResult>>, IRequireAuthorization
{
    // Admins can execute this command
    public Role[] Roles => [Role.Admin];
}
