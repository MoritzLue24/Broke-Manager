
using Domain.Common;

namespace Domain.Entities;

public class StandingOrderPause
{
    public Guid Id { get; }
    public DateOnly From { get; private set; }
    public DateOnly To { get; private set; }

    private StandingOrderPause(DateOnly from, DateOnly to)
    {
        Id = Guid.NewGuid();
        From = from;
        To = to;
    }

    public static DomainResult<StandingOrderPause> Create(DateOnly from, DateOnly? to)
    {
        if (from > (to ?? DateOnly.MaxValue))
            return DomainResult<StandingOrderPause>.Fail(DomainErrorCode.StandingOrderPauseDatesInvalid);

        return DomainResult<StandingOrderPause>.Ok(new(
            from,
            to ?? DateOnly.MaxValue
        ));
    }

    public DomainResult<Unit> UpdateFrom(DateOnly from)
    {
        if (from > To)
            return DomainResult<Unit>.Fail(DomainErrorCode.StandingOrderPauseDatesInvalid);

        From = from;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> UpdateTo(DateOnly to)
    {
        if (From > to)
            return DomainResult<Unit>.Fail(DomainErrorCode.StandingOrderPauseDatesInvalid);

        To = to;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> MakeInfinite()
    {
        To = DateOnly.MaxValue;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> Delete()
    {
        return DomainResult<Unit>.Ok();
    }
}