namespace Domain.Common;

public enum DomainErrorCode
{
    //Common
    InvalidGuid = 0,

    // Keyword
    KeywordEmpty,
    KeywordAlreadyExists,
    KeywordNotFound,

    //User
    InvalidEmailFormat = 10,
    InvaildHashFormat = 11,
    UserNotFound = 12,

    //Category
    CategoryNameEmpty = 20,
    CategoryNotFound = 21,
    NoKeywordForDefaultCategory = 24,
    CannotDeleteDefaultCategory = 26,

    //Transaction
    InvalidTransactionDate = 30,
    InvalidAmount = 31,
    TransactionTitleEmpty = 32,
    InvalidCategorySource = 34,
    InvalidInterval = 35,

    // Standing Order
    StandingOrderNameEmpty = 40,
    StandingOrderDatesInvalid = 41,

    // Recurrence Pattern
    RecurrencePatternInvalidExecutionDay = 50,

    // Standing Order Pause
    StandingOrderPauseDatesInvalid = 60,
}
