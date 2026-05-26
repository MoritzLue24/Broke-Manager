using Domain.Common.Models;

namespace Domain.Tests.Common.Models;

public class AggregateRootTests
{
    private class Aggregate(Guid id) : AggregateRoot(id)
    {
        public void AddEvent(IDomainEvent domainEvent)
            => this.AddDomainEvent(domainEvent);
    }
    private class Event() : IDomainEvent;

    [Fact]
    public void AddDomainEvent_ShouldAdd()
    {
        Aggregate aggregate = new(Guid.NewGuid());
        Event e1 = new();
        Event e2 = new();

        aggregate.AddEvent(e1);
        aggregate.AddEvent(e2);

        Assert.Equal([e1, e2], aggregate.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_ShouldClear()
    {
        Aggregate aggregate = new(Guid.NewGuid());
        Event e1 = new();
        Event e2 = new();

        aggregate.AddEvent(e1);
        aggregate.AddEvent(e1);

        aggregate.ClearDomainEvents();

        Assert.Equal([], aggregate.DomainEvents);
    }
}
