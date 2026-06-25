using Domain.ValueObjects;

namespace Application.Features.AutoAssign.Contracts;

public record MatchingRuleResult(
    string Keyword
);

public static class MatchingRuleExtension
{
    public static MatchingRuleResult ToResult(this MatchingRule rule)
        => new(rule.Keyword);
}
