using Domain.Entities;

namespace Application.Features.MatchingRules.Common;

public record MatchingRuleResult(
    Guid Id,
    string Keyword
);

public static class MatchingRuleExtension
{
    public static MatchingRuleResult ToResult(this MatchingRule rule)
        => new(
            rule.Id,
            rule.Keyword
        );
}
