namespace Domain.Common;

public class Result<V>
{
    private readonly V _value;
    private readonly IEnumerable<Error> _errors;

    public bool Success { get; }

    public V Value => Success
        ? _value
        : throw new InvalidOperationException("No value on failure");

    public IEnumerable<Error> Errors => !Success
        ? _errors
        : throw new InvalidOperationException("No error on success");

    public Error FirstError => !Success
        ? _errors.Any() 
            ? _errors.First()
            : throw new InvalidOperationException("Result not successful, but errors are empty")
        : throw new InvalidOperationException("No error on success");

    private Result(V value)
    {
        Success = true;
        _value = value;
        _errors = [];
    }

    private Result(IEnumerable<Error> errors)
    {
        Success = false;
        _value = default!;
        _errors = errors;
    }

    public static implicit operator Result<V>(V value) => new(value);
    public static implicit operator Result<V>(Error error) => new([error]);
    public static implicit operator Result<V>(List<Error> errors) => new(errors);
    public static implicit operator Result<V>(Error[] errors) => new(errors);

    public Result<U> Cast<U>(Func<V, U>? converter = null)
    {
        if (!Success)
            return new(_errors);

        if (converter != null)
            return new(converter(_value));

        if (_value is U valueAsU)
            return new(valueAsU);

        throw new InvalidOperationException(
            $"Cannot convert {typeof(V).Name} to {typeof(U).Name} without converter"
        );
    }

    public TOut Match<TOut>(
        Func<V, TOut> onSuccess,
        Func<IEnumerable<Error>, TOut> onError)
        => Success ? onSuccess(_value) : onError(_errors);
}