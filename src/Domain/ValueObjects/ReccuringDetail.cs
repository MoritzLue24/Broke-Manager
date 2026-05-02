using Domain.Common;

namespace Domain.ValueObjects;

public sealed record ReccuringDetail
{
    public DateOnly Start {get; }
    public DateOnly End {get; }

}