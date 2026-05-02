namespace Domain.Common;

public enum DomainErrorCode
{
    //Common
    InvalidId = 0,
    
    //User
    InvalidEmailFormat = 10,
    InvaildHashFormat = 11,
    UserNotFoun = 12,

    //Category
    CategoryNameEmpty = 20,
    CategoryNotFound = 21,
    InvalidKeyWordFormat = 22,
    NotUniqueKeywordWithinOneCategory = 23,
    NoKeywordForDefaultCategory = 24,
    KeywordNotFounInCategory = 25,
    DefaultCategoryCannotDelete = 26,

    
    //Transaction
    InvalidTransactionDate = 30,
    WrongAmount = 31,
    TransactionTitleEmpty = 32,
    TransactionCounterPartyEmpty = 33,
    InvalidCategorySource = 34,


}
