using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Keyword
{
    public string Value { get;}

    private Keyword(string value)
    {
        Value = value;
    }


    public static DomainResult<Keyword> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DomainResult<Keyword>.Fail(DomainErrorCode.InvalidKeyWordFormat);
        }

        return DomainResult<Keyword>.Ok(new Keyword(value));
    }
}