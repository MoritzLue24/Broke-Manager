
namespace Application.Common.Results;

public enum ErrorCode
{
    // Domain
    InvalidGuid,
    InvalidTransactionAmount,
    TransactionTitleEmpty,

    // User
    UserNotFound,
    EmailAlreadyExists,
    InvalidPassword,

    // Transaction
    TransactionNotFound,

    // Category
    DefaultCategoryNotFound,
    CategoryNotFound
}
