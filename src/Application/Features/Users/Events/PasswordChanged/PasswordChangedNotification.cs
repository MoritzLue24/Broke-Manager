using Domain.Events.Users;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Events.PasswordChanged;

public record PasswordChangedNotification(
    Guid UserId,
    Email Email
) : INotification;

public static class PasswordChangedEventExtension
{
    public static PasswordChangedNotification ToNotification(this PasswordChangedEvent e)
        => new(e.UserId, e.Email);
}
