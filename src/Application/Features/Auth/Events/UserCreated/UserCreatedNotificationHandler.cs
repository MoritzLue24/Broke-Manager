using MediatR;

namespace Application.Features.Auth.Events.UserCreated;

public class UserCreatedHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
