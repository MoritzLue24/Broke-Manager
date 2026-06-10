using Contracts.Features.MatchingRules;

namespace Contracts.Features.Categories;

public record CategoryResponse(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    MatchingRuleResponse[] MatchingRules,
    DateTime CreatedAt
);

