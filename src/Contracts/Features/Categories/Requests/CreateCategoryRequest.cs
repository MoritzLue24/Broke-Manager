namespace Contracts.Features.Categories.Requests;

public record CreateCategoryRequest(
    string Name,
    string[] Keywords   // Use create natching rule request object
);
