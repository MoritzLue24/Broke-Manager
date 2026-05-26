namespace Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? other)
        => other is not null
            && this.GetType() == other.GetType()
            && this.GetEqualityComponents()
                .SequenceEqual(((ValueObject)other).GetEqualityComponents());

    public bool Equals(ValueObject? other)
        => other is not null
            && this.GetEqualityComponents()
                .SequenceEqual(other.GetEqualityComponents());

    public static bool operator ==(ValueObject? lhs, ValueObject? rhs)
        => lhs is null & rhs is null
            || lhs?.Equals(rhs) == true;

    public static bool operator !=(ValueObject lhs, ValueObject rhs)
        => !(lhs == rhs);

    public override int GetHashCode()
        => this.GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((hash, x) => HashCode.Combine(hash, x));
}
