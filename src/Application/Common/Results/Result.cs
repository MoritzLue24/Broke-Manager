using Domain.Common;

namespace Application.Common.Results;

public class Result<T>
{
    private readonly T _value;
    private readonly ErrorCode _error;

    public bool Success { get; }

    public T Value => Success
        ? _value
        : throw new InvalidOperationException("No value on failure");

    public ErrorCode Error => Success
        ? _error
        : throw new InvalidOperationException("No error on success");

    private Result(T value)
    {
        Success = true;
        _value = value;
    }

    private Result(ErrorCode error)
    {
        Success = false;
        _value = default!;
        _error = error;
    }

    public static Result<T> Ok(T value)
        => new(value);

    public static Result<Unit> Ok()
        => new(Unit.Value);

    public static Result<T> Fail(ErrorCode error)
        => new(error);
}