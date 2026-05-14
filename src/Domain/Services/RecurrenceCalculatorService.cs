using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Services;

public class RecurrenceCalculatorService
{
    public DomainResult<DateOnly> GetActualDate(RecurrencePattern pattern, DateOnly referenceDate)
    {
        DateOnly periodStart = pattern.Interval switch
        {
            Interval.Weekly => referenceDate.AddDays(1 - (
                referenceDate.DayOfWeek == DayOfWeek.Sunday
                ? 7
                : (int)referenceDate.DayOfWeek
            )),
            Interval.Monthly => new DateOnly(
                referenceDate.Year,
                referenceDate.Month,
                1
            ),
            Interval.Quarterly => new DateOnly(
                referenceDate.Year,
                (int)((referenceDate.Month - 1) / 3) * 3 + 1,
                1
            ),
            Interval.Yearly => new DateOnly(referenceDate.Year, 1, 1),
            _ => throw new NotImplementedException()
        };

        DateOnly periodEnd = pattern.Interval switch
        {
            Interval.Weekly => periodStart.AddDays(6),
            Interval.Monthly => periodStart.AddMonths(1).AddDays(-1),
            Interval.Quarterly => periodStart.AddMonths(3).AddDays(-1),
            Interval.Yearly => periodStart.AddYears(1).AddDays(-1),
            _ => throw new NotImplementedException()
        };

        DateOnly executionDate = periodStart.AddDays(pattern.ExecutionDay - 1);
        return DomainResult<DateOnly>.Ok(
            executionDate > periodEnd
            ? periodEnd
            : executionDate
        );
    }
}