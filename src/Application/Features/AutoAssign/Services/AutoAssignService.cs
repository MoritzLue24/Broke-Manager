using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Features.AutoAssign.Services;

public record MatchResult(
    Guid CategoryId,
    CategorySource CategorySource,
    IReadOnlyCollection<(Guid CategoryId, double Score)>? ConflictingCategories
);

public class AutoAssignService
{
    public static MatchResult FindMatch(
        Transaction transaction,
        // IReadOnlyCollection<StandingOrder> standingOrders, TODO
        IReadOnlyCollection<Category> categories,
        Guid defaultCategoryId)
    {
        // var standingOrderMatches = this.MatchStandingOrders(transaction, standingOrders)
        // ... TODO

        var categoryMatches = MatchCategories(transaction, categories);
        if (categoryMatches.Count == 0)
            return new(
                defaultCategoryId,
                CategorySource.Unmatched,
                null
            );

        return new(
            categoryMatches.First().Category.Id,
            CategorySource.Auto,
            categoryMatches.Count > 1
                ? categoryMatches.Select(pair => (pair.Category.Id, pair.Score)).ToList()
                : null
        );
    }

    private static IReadOnlyCollection<(Category Category, double Score)> MatchCategories(
        Transaction transaction,
        IReadOnlyCollection<Category> categories)
        => categories
            .Where(c => !c.IsDefault)
            .Select(c => (
                Category: c,
                Score: CalculateScore(transaction, c.MatchingRules)
            ))
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ToList();

    private static double CalculateScore(
        Transaction transaction,
        IReadOnlyCollection<MatchingRule> rules)
    {
        double score = 0;

        foreach (var rule in rules)
        {
            if (!transaction.Title.Contains(rule.Keyword, StringComparison.CurrentCultureIgnoreCase))
                continue;
            var keywordLen = rule.Keyword.Count(c => !char.IsWhiteSpace(c));
            var proportion = (double)keywordLen / transaction.Title.Count(c => !char.IsWhiteSpace(c));
            score += proportion;
        }
        return score;
    }
}