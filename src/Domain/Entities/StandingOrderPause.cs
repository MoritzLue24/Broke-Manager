
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

    public static Result<StandingOrderPause> Create(DateOnly from, DateOnly? to)
    {
        if (from > (to ?? DateOnly.MaxValue))
            throw new NotImplementedException();

        return new StandingOrderPause(
            from,
            to ?? DateOnly.MaxValue
        );
    }

    public Result<Unit> UpdateFrom(DateOnly from)
    {
        if (from > To)
            throw new NotImplementedException();

        From = from;
        return Unit.Value;
    }

    public Result<Unit> UpdateTo(DateOnly to)
    {
        if (From > to)
            throw new NotImplementedException();

        To = to;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        To = DateOnly.MaxValue;
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        return Unit.Value;
    }
}