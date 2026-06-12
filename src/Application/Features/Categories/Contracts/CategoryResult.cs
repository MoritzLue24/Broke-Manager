using Application.Features.MatchingRules.Contracts;
using Domain.Entities;

namespace Application.Features.Categories.Contracts;

public record CategoryResult(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    List<MatchingRuleResult> MatchingRules,
    DateTime CreatedAt
);

public static class CategoryExtension
{
    public static CategoryResult ToResult(this Category category)
        => new(
            category.Id,
            category.UserId,
            category.Name,
            category.IsDefault,
            category.MatchingRules.Select(k => k.ToResult()).ToList(),
            category.CreatedAt
        );
}
