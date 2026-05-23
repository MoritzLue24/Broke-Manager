using Domain.Enums;

namespace Application.Common.Interfaces.Security;

public interface IRequireAuthorization
{
    Role[] Roles { get; }
}