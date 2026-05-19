namespace Domain.Common;

public class Result<TValue>
{
    private readonly TValue _value;
    private readonly IEnumerable<Error> _errors;

    public bool Success { get; }

    public TValue Value => this.Success
        ? this._value
        : throw new InvalidOperationException("No value on failure");

    public IEnumerable<Error> Errors => !this.Success
        ? this._errors
        : throw new InvalidOperationException("No error on success");

    public Error FirstError => !this.Success
        ? this._errors.Any()
            ? this._errors.First()
            : throw new InvalidOperationException("Result not successful, but errors are empty")
        : throw new InvalidOperationException("No error on success");

    private Result(TValue value)
    {
        this.Success = true;
        this._value = value;
        this._errors = [];
    }

    private Result(IEnumerable<Error> errors)
    {
        this.Success = false;
        this._value = default!;
        this._errors = errors;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new([error]);
    public static implicit operator Result<TValue>(List<Error> errors) => new(errors);
    public static implicit operator Result<TValue>(Error[] errors) => new(errors);

    public Result<TResult> Cast<TResult>(Func<TValue, TResult>? converter = null)
    {
        if (!this.Success)
            return new(this._errors);

        if (converter != null)
            return new(converter(this._value));

        if (this._value is TResult valueAsU)
            return new(valueAsU);

        throw new InvalidOperationException(
            $"Cannot convert {typeof(TValue).Name} to {typeof(TResult).Name} without converter");
    }

    public TOut Match<TOut>(
        Func<TValue, TOut> onSuccess,
        Func<IEnumerable<Error>, TOut> onError)
        => this.Success ? onSuccess(this._value) : onError(this._errors);
}
