namespace Domain.Common.Models;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => this._domainEvents.AsReadOnly();

    protected AggregateRoot(Guid id) : base(id) { }

    protected void AddDomainEvent(IDomainEvent domainEvent)
        => this._domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => this._domainEvents.Clear();
}
