using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public class StandingOrderPause : ValueObject
{
    public DateOnly From { get; private set; }
    public DateOnly To { get; private set; }

    private StandingOrderPause(DateOnly from, DateOnly to)
    {
        this.From = from;
        this.To = to;
    }

    public static Result<StandingOrderPause> Create(DateOnly from, DateOnly? to)
    {
        if (from > (to ?? DateOnly.MaxValue))
            return new DateFromGreaterThanToError();

        return new StandingOrderPause(
            from,
            to ?? DateOnly.MaxValue);
    }

    public Result<Unit> UpdateFrom(DateOnly from)
    {
        if (from > this.To)
            return new DateFromGreaterThanToError();

        this.From = from;
        return Unit.Value;
    }

    public Result<Unit> UpdateTo(DateOnly to)
    {
        if (this.From > to)
            return new DateFromGreaterThanToError();

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

    protected override IEnumerable<object?> GetEqualityComponents()
        => [this.From, this.To];
}
