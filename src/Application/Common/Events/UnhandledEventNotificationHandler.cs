using MediatR;

namespace Application.Common.Events;

public class UnhandledEventNotificationHandler : INotificationHandler<UnhandledEventNotification>
{
    public Task Handle(UnhandledEventNotification notification, CancellationToken cancellationToken)
    {
        // TODO: Do something
        return Task.CompletedTask;
    }
}
