using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Common.Behaviors;

public class AuthorizeBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    // : IPipelineBehavior<TRequest, Result<TResponse>>
    // where TRequest : IRequest<Result<TResponse>> DOES NOT WORK
    // ^ in dotnet, DI with nested generics sometimes do not work.
    // if we used this commented-out-approach, our behavior never gets called :/
    where TRequest : notnull
{
    private readonly IUserContext _userContext;

    public AuthorizeBehavior(IUserContext userContext)
    {
        this._userContext = userContext;
    }

    // Gets called before every mediator call. 
    // `next` is the actual mediator call, just like in the exception middleware
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IRequireAuthorization)
            return await next(cancellationToken);

        // We have to cast this during runtime (ugly),
        // but theres no other option since we have this very unclear 
        // `TResponse` generic as return type
        // FIXME: Find other, more cleaner, approaches
        if (this._userContext.UserId == null)
            return (dynamic)new UnauthorizedError();

        // If there is not one request's valid roles that are in the user roles, return error
        if (!((IRequireAuthorization)request).Roles.Any(role => this._userContext.UserRoles.Contains(role)))
            return (dynamic)new ForbiddenError();

        return await next(cancellationToken);
    }
}
