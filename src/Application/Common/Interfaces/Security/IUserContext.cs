using Domain.Enums;

namespace Application.Common.Interfaces.Security;

public interface IUserContext
{
    Guid? SessionId { get; }
    Guid? UserId { get; }
    IReadOnlyCollection<Role> UserRoles { get; }
}
