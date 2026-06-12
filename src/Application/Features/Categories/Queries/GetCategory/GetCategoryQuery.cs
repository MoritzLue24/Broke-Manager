using Application.Common.Behaviors;
using Application.Features.Categories.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategory;

public record GetCategoryQuery(
    Guid CategoryId
) : IRequest<Result<CategoryResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
