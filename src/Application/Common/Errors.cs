using Domain.Common;

namespace Application.Common;

public sealed record CategoryNotFoundError : Error;
public sealed record DefaultCategoryNotFoundError : Error;

public sealed record TransactionNotFoundError : Error;