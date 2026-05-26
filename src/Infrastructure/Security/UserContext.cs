using System.Security.Claims;
using Application.Common.Interfaces.Security;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContext;

    public UserContext(IHttpContextAccessor httpContext)
    {
        this._httpContext = httpContext;
    }

    public Guid? UserId
    {
        get
        {
            var user = this._httpContext.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("NameIdentifier claim not found");

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public IReadOnlyCollection<Role> UserRoles
        => this._httpContext.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(c =>
            {
                if (Enum.TryParse(c.Value, out Role role))
                    return role;
                throw new InvalidOperationException("Invalid role in Role claim");
            }).ToArray()
            // If HttpContext is null -> User is null -> FindAll is null -> ...
            // If this is the case, dont want to return null, but an empty collection / array
            ?? [];
}
