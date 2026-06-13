using Domain.Enums;
using Domain.Events.Users;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Events.RoleChanged;

public record RoleChangedNotification(
    Guid UserId,
    Email Email,
    Role NewRole,
    Role OldRole
) : INotification;

public static class RoleChangedEventExtension
{
    public static RoleChangedNotification ToNotification(this RoleChangedEvent e)
        => new(e.UserId, e.Email, e.NewRole, e.OldRole);
}
