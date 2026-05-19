using Domain.Common;
using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record RecurrencePattern
{
    public Interval Interval { get; }
    public int ExecutionDay { get; }

    private RecurrencePattern(Interval interval, int executionDay)
    {
        this.Interval = interval;
        this.ExecutionDay = executionDay;
    }

    public static Result<RecurrencePattern> Create(Interval interval, int executionDay)
    {
        if (executionDay < 1)
            throw new NotImplementedException();

        return new RecurrencePattern(
            interval,
            executionDay);
    }

    public Result<DateOnly> GetActualDay(DateOnly referenceDate)
    {
        DateOnly periodStart = this.Interval switch
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
                ((referenceDate.Month - 1) / 3 * 3) + 1,
                1
            ),
            Interval.Yearly => new DateOnly(referenceDate.Year, 1, 1),
            _ => throw new NotImplementedException()
        };

        DateOnly periodEnd = this.Interval switch
        {
            Interval.Weekly => periodStart.AddDays(6),
            Interval.Monthly => periodStart.AddMonths(1).AddDays(-1),
            Interval.Quarterly => periodStart.AddMonths(3).AddDays(-1),
            Interval.Yearly => periodStart.AddYears(1).AddDays(-1),
            _ => throw new NotImplementedException()
        };

        DateOnly executionDate = periodStart.AddDays(this.ExecutionDay - 1);
        return executionDate > periodEnd
            ? periodEnd
            : executionDate;
    }
}