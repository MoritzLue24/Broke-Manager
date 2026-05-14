using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Keyword
{
    public string Value { get; }

    private Keyword(string value)
    {
        Value = value;
    }

    public static Result<Keyword> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new EmptyKeywordError();

        return new Keyword(value);
    }
}