using Contracts.Features.AutoAssign.Responses;

namespace Contracts.Features.Categories.Responses;

public record CategoryResponse(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    MatchingRuleResponse[] MatchingRules,
    DateTime CreatedAt
);

