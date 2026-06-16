namespace Application.Features.Analytics.Contracts;

public record SummaryResult(
    decimal Balance,
    decimal Income,
    decimal Expenses
);