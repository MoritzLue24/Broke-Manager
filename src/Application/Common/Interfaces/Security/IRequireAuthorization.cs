using Domain.Enums;

namespace Application.Common.Interfaces.Security;

// Add this to a query / command to restrict execution permission
public interface IRequireAuthorization
{
    // Users with at least one role in `Roles` can access this query / command
    Role[] Roles { get; }
}