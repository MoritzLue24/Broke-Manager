using Domain.Events.Users;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Events.UserCreated;

public record UserCreatedNotification(Guid UserId, Email Email) : INotification;

public static class UserCreatedEventExtension
{
    public static UserCreatedNotification ToNotification(this UserCreatedEvent e)
        => new(e.UserId, e.Email);
}
