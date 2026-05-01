using Domain.Common;

namespace Application.Common.Results;

public static class DomainResultExtension
{
    public static Result<T> Map<T, U>(
        this DomainResult<U> domainResult,
        Func<U, T> mapping)
    {
        if (domainResult.Success)
            return Result<T>.Ok(mapping(domainResult.Value));
        return MapError<T, U>(domainResult);
    }

    public static Result<T> MapError<T, U>(this DomainResult<U> domainResult)
    {
        if (domainResult.Success) 
            throw new InvalidOperationException("Cannot map error if there is no error");

        return Result<T>.Fail(domainResult switch
        {
            // TODO
            _ => throw new NotImplementedException()
        });
    }
}
