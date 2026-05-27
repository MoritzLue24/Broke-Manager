using Application.Common.Events;
using Domain.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class PublishDomainEventsInteceptor : SaveChangesInterceptor
{
    private readonly DomainEventDispatcher _dispatcher;

    public PublishDomainEventsInteceptor(DomainEventDispatcher dispatcher)
    {
        this._dispatcher = dispatcher;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        throw new InvalidOperationException("Use async method");
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await this.PublishDomainEventsAsync(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task PublishDomainEventsAsync(DbContext? dbContext)
    {
        if (dbContext is null)
            return;

        var aggregatesWithEvents = dbContext.ChangeTracker.Entries<AggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();
        var events = aggregatesWithEvents
            .SelectMany(aggregate => aggregate.DomainEvents)
            .ToList();  // Important for copying

        aggregatesWithEvents.ForEach(aggregate => aggregate.ClearDomainEvents());

        await this._dispatcher.DispatchAsync(events);
    }
}
