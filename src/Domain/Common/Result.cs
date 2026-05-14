namespace Domain.Common;

public class Result<V>
{
    private readonly V _value;
    private readonly Error _error;

    public bool Success { get; }

    public V Value => Success
        ? _value
        : throw new InvalidOperationException("No value on failure");

    public Error Error => !Success
        ? _error
        : throw new InvalidOperationException("No error on success");

    private Result(V value)
    {
        Success = true;
        _value = value;
        _error = default!;
    }

    private Result(Error error)
    {
        Success = false;
        _value = default!;
        _error = error;
    }

    public static implicit operator Result<V>(V value) => new(value);
    public static implicit operator Result<V>(Error error) => new(error);

    public Result<U> Cast<U>(Func<V, U>? converter = null)
    {
        if (!Success)
            return new(_error);

        if (converter != null)
            return new(converter(_value));

        if (_value is U valueAsU)
            return new(valueAsU);

        throw new InvalidOperationException(
            $"Cannot convert {typeof(V).Name} to {typeof(U).Name} without converter"
        );
    }

    public TOut Match<TOut>(Func<V, TOut> onSuccess, Func<Error, TOut> onError)
        => Success ? onSuccess(_value) : onError(_error);
}