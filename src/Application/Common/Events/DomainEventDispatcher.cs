using Application.Features.Auth.Events.UserCreated;
using Application.Features.Categories.Events.CategoryDeleted;
using Application.Features.Users.Events.EmailChanged;
using Application.Features.Users.Events.PasswordChanged;
using Application.Features.Users.Events.RoleChanged;
using Domain.Common.Models;
using Domain.Events.Categories;
using Domain.Events.Users;
using MediatR;

namespace Application.Common.Events;

// Gets injected to SaveChangesAsync interceptor which calles dispatch
public class DomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        this._mediator = mediator;
    }

    // Publishes
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            // TODO: ohne switch und mit mapper interface oder so
            INotification notification = domainEvent switch
            {
                UserCreatedEvent e => e.ToNotification(),

                EmailChangedEvent e => e.ToNotification(), 
                PasswordChangedEvent e => e.ToNotification(),
                RoleChangedEvent e => e.ToNotification(),

                CategoryDeletedEvent e => e.ToNotification(),

                _ => new UnhandledEventNotification(domainEvent)
            };
            await this._mediator.Publish(notification);
        }
    }
}
