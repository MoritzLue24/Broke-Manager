namespace Contracts.Features.Categories;

public record CategoryDetailResponse(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    string[] Keywords,  // TODO: Use matching rule response object
    DateTime CreatedAt
);

