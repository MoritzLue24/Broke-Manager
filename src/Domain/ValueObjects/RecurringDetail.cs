using Domain.Common;
using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record RecurringDetail
{
    public DateOnly Start {get; }
    public DateOnly? End {get; }
    public Interval Interval {get;}

    private RecurringDetail(DateOnly start, DateOnly end, Interval interval)
    {
        Start = start;
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

        if(finalEnd < start)
        {
            return DomainResult<RecurringDetail>.Fail(DomainErrorCode.InvalidRecurringDate);
        }
    
        return DomainResult<RecurringDetail>.Ok(new RecurringDetail(start, finalEnd, interval));
    }

}