namespace Contracts.Features.Categories;

public record CreateCategoryRequest(
    string Name,
    string[] Keywords   // Use create natching rule request object
);
