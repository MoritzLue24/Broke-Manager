using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Hash
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
}
