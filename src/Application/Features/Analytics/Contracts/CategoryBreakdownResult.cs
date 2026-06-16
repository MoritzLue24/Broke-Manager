using Application.Features.Categories.Contracts;

namespace Application.Features.Analytics.Contracts;

public record CategoryBreakdownResult(
    CategoryResult CategoryResult,
    decimal Expenses,
    double Percentage
);