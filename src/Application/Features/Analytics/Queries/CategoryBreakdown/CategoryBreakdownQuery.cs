using Application.Common.Behaviors;
using Application.Features.Analytics.Contracts;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Analytics.Queries.CategoryBreakdown;

public record CategoryBreakdownQuery(
    AnalyticsPeriod Period
) : IRequest<Result<IReadOnlyCollection<CategoryBreakdownResult>>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
