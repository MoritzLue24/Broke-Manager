using Domain.Common;
using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record RecurringDetail
{
    public DateOnly? End {get; }
    public Interval Interval {get;}

    private RecurringDetail( DateOnly end, Interval interval)
    {
        End = end;
        Interval = interval;
    }

    public static DomainResult<RecurringDetail> Create(DateOnly start, DateOnly? end, Interval interval)
    {
        DateOnly finalEnd;

        if(end != null)
        {
            finalEnd = end.Value;
        } else
        {
            finalEnd = DateOnly.MaxValue;
        }

        if (!Enum.IsDefined(typeof(CategorySource), interval))
                {
                        return DomainResult<RecurringDetail>.Fail(DomainErrorCode.InvalidInterval);
                }

        return DomainResult<RecurringDetail>.Ok(new RecurringDetail(finalEnd, interval));
    }

}