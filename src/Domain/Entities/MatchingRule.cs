using Domain.Common;
using Domain.Common.Models;

namespace Domain.Entities;

public class MatchingRule : Entity
{
    public string Keyword { get; }

    public MatchingRule(Guid id, string keyword)
        : base(id)
    {
        this.Keyword = keyword;
    }

    public static Result<MatchingRule> Create(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new EmptyKeywordError();

        return new MatchingRule(Guid.NewGuid(), keyword);
    }
}
