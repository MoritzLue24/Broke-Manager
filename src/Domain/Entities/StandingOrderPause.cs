using Domain.Common;

namespace Domain.Entities;

public class StandingOrderPause
{
    public Guid Id { get; }
    public DateOnly From { get; private set; }
    public DateOnly To { get; private set; }

    private StandingOrderPause(DateOnly from, DateOnly to)
    {
        this.Id = Guid.NewGuid();
        this.From = from;
        this.To = to;
    }

    public static Result<StandingOrderPause> Create(DateOnly from, DateOnly? to)
    {
        if (from > (to ?? DateOnly.MaxValue))
            throw new NotImplementedException();

        return new StandingOrderPause(
            from,
            to ?? DateOnly.MaxValue);
    }

    public Result<Unit> UpdateFrom(DateOnly from)
    {
        if (from > this.To)
            throw new NotImplementedException();

        this.From = from;
        return Unit.Value;
    }

    public Result<Unit> UpdateTo(DateOnly to)
    {
        if (this.From > to)
            throw new NotImplementedException();

        this.To = to;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        this.To = DateOnly.MaxValue;
        return Unit.Value;
    }

    public static Result<Unit> Delete()
        => Unit.Value;
}
