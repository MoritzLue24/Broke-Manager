namespace Domain.Common;

public class DomainResult<T>
{
    private readonly T _value;
    private readonly DomainErrorCode _error;

    public bool Success { get; }

    public T Value => Success
        ? _value
        : throw new InvalidOperationException("No value on failure");

    public DomainErrorCode Error => !Success
        ? _error
        : throw new InvalidOperationException("No error on success");

    private DomainResult(T value)
    {
        Success = true;
        _value = value;
    }

    private DomainResult(DomainErrorCode error)
    {
        Success = false;
        _value = default!;
        _error = error;
    }

    public static DomainResult<T> Ok(T value)
        => new(value);

    public static DomainResult<Unit> Ok()
        => new(Unit.Value);

    public static DomainResult<T> Fail(DomainErrorCode error)
        => new(error);
}