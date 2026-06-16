namespace Contracts.Features.Analytics.Requests;

public record AnalyticsPeriodRequest(
    string Range,
    DateOnly? From,
    DateOnly? To
);