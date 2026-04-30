using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Hash
{
    public string Value { get; }

    private Hash(string value)
        => Value = value;

    public static DomainResult<Hash> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return DomainResult<Hash>.Fail(DomainErrorCode.InvaildHashFormat);

        return DomainResult<Hash>.Ok(new Hash(hash));
    }
}