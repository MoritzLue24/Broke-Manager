using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public sealed class MatchingRule : ValueObject
{
    public string Keyword { get; }

    private MatchingRule(string keyword)
    {
        this.Keyword = keyword;
    }

    public static Result<MatchingRule> Create(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new EmptyKeywordError();

        return new MatchingRule(keyword);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
        => [this.Keyword];
}
