using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Auth.Contracts;

public record SessionResult(
    Guid Id,
    DateTime LastSeen,
    Guid UserId,
    IReadOnlyCollection<Role> Roles
);

public static class SessionExtension
{
    public static SessionResult ToResult(this Session session)
        => new(
            session.Id,
            session.LastSeen,
            session.UserId,
            session.Roles
        );
}
