using Application.Common.Interfaces.Security;
using Application.Features.Categories.Common;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string[] Keywords   // TODO: MatchingRule object list
) : IRequest<Result<CategoryResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
