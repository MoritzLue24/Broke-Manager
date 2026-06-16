using Contracts.Features.Categories.Responses;

namespace Contracts.Features.Analytics.Responses;

public record CategoryBreakdownResponse(
    CategoryResponse Category,
    decimal Expenses,
    double Percentage
);