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

    public Guid? SessionId
        => this._httpContext.HttpContext?.Items["sessionId"] as Guid?;

    public Guid? UserId
        => this._httpContext.HttpContext?.Items["userId"] as Guid?;

    public IReadOnlyCollection<Role> UserRoles
        => this._httpContext.HttpContext?.Items["roles"] as IReadOnlyCollection<Role>
            ?? [];
}
