using Domain.Common;

namespace Application.Common;

public sealed record ValidationError(
    string Property,
    string Message
) : Error;

public sealed record UnauthorizedError : Error;
public sealed record ForbiddenError : Error;
public sealed record InvalidCredentialsError : Error;
public sealed record EmailAlreadyRegisteredError : Error;
public sealed record UserNoLongerExistsError : Error;

public sealed record UserNotFoundError : Error;

public sealed record CategoryNotFoundError : Error;
public sealed record DefaultCategoryNotFoundError : Error;
public sealed record CategoryNameAlreadyExistsError : Error;

public sealed record TransactionNotFoundError : Error;
