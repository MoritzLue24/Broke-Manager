namespace Contracts.Features.Analytics.Responses;

public record SummaryResponse(
    decimal Balance,
    decimal Income,
    decimal Expenses
);