namespace Domain.Common;

public abstract record Error;

public sealed record InvalidGuidError : Error;

public sealed record InvalidEmailFormatError : Error;
public sealed record InvalidHashFormatError : Error;
public sealed record EmptyKeywordError : Error;

public sealed record InvalidAmountError : Error;
public sealed record EmptyTransactionTitleError : Error;
public sealed record InvalidCategorySourceError : Error;

public sealed record EmptyCategoryNameError : Error;
public sealed record CategoryIsDefaultError : Error;
public sealed record DuplicateKeywordError : Error;
public sealed record KeywordNotFoundError : Error;
