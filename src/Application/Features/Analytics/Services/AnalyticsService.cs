using Application.Features.Analytics.Contracts;

namespace Application.Features.Analytics.Services;

public class AnalyticsService
{
    public static (DateOnly? Start, DateOnly? End) CalculatePeriod(
        AnalyticsPeriodRange range,
        DateOnly? from,
        DateOnly? to)
    {
        var now = DateOnly.FromDateTime(DateTime.UtcNow);
        return range switch 
        {
            AnalyticsPeriodRange.Custom => (from, to),
            AnalyticsPeriodRange.Last30Days => (now.AddDays(-30), now),
            AnalyticsPeriodRange.Last90Days => (now.AddDays(-90), now),
            AnalyticsPeriodRange.ThisMonth => (new DateOnly(now.Year, now.Month, 1), now),
            AnalyticsPeriodRange.ThisYear => (new DateOnly(now.Year, 1, 1), now),
            AnalyticsPeriodRange.AllTime => (null, null),
            _ => throw new NotImplementedException()
        };
    }
}