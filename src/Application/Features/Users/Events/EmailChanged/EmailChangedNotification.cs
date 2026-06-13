using Domain.Events.Users;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Events.EmailChanged;

public record EmailChangedNotification(
    Guid UserId,
    Email NewEmail,
    Email OldEmail
) : INotification;

public static class EmailChangedEventExtension
{
    public static EmailChangedNotification ToNotification(this EmailChangedEvent e)
        => new(e.UserId, e.NewEmail, e.OldEmail);
}
