namespace Domain.Common.Models;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id)
        => this.Id = id;

    public override bool Equals(object? other)
        => other is Entity entity
            && this.Id == entity.Id;

    public bool Equals(Entity? other)
        => this.Id == other?.Id;

    public static bool operator ==(Entity? lhs, Entity? rhs)
        => lhs is null && rhs is null
            || lhs?.Equals(rhs) == true;

    public static bool operator !=(Entity? lhs, Entity? rhs)
        => !(lhs == rhs);

    public override int GetHashCode()
        => this.Id.GetHashCode();
}
