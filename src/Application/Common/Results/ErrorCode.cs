
namespace Application.Common.Results;

public enum ErrorCode
{
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
