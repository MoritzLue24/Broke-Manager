using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public sealed class Hash : ValueObject
{
    public string Value { get; }

    private Hash(string value)
    {
        this.Value = value;
    }

    public static Result<Hash> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return new InvalidHashFormatError();

        return new Hash(hash);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
        => [this.Value];
}
