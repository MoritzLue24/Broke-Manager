using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    // : IPipelineBehavior<TRequest, Result<TResponse>>
    // where TRequest : IRequest<Result<TResponse>> DOES NOT WORK
    // ^ in dotnet, DI with nested generics sometimes do not work.
    // if we used this commented-out-approach, our behavior never gets called :/
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this._validators = validators;
    }

    // Gets called before every mediator call. 
    // `next` is the actual mediator call, just like in the exception middleware
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!this._validators.Any())
            return await next(cancellationToken);

        // Task.WhenAll hab ich nicht wirklich gecheckt :/
        var validationResults = await Task.WhenAll(this._validators.Select(async v
            => await v.ValidateAsync(request, cancellationToken)
        ));

        if (validationResults.All(v => v.IsValid))
            return await next(cancellationToken);

        var errors = validationResults
            .SelectMany(v => v.Errors)  // Flatten nested collection
            .Select(error => new ValidationError(
                error.PropertyName,
                error.ErrorMessage
            ));

        // We have to cast this during runtime (ugly),
        // but theres no other option since we have this very unclear 
        // `TResponse` generic as return type
        // FIXME: Find other, more cleaner, approaches
        return (dynamic)errors.Cast<Error>().ToList();
    }
}
