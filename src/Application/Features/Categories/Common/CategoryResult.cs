using Domain.Entities;

namespace Application.Features.Categories.Common;

public record CategoryResult(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    List<string> Keywords,  // TODO: Matching rule object list
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
            category.MatchingRules.Select(k => k.Keyword).ToList(),
            category.CreatedAt
        );
}
