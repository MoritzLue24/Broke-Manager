using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid? StandingOrderId { get; private set; }
    public Guid CategoryId { get; private set; }
    public CategorySource CategorySource { get; private set; }
    public StandingOrderSource? StandingOrderSource { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateOnly Date {get; private set;}
    public string Title {get; private set;}
    public string Description { get; private set; }
    public string CounterParty {get; private set;}
    public DateTime CreatedAt { get; }

    private Transaction(
        Guid userId,
        Guid categoryId,
        CategorySource categorySource,
        decimal amount,
        TransactionType type,
        DateOnly date,
        string title,
        string description,
        string counterParty)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        StandingOrderId = null;
        CategoryId = categoryId;
        CategorySource = categorySource;
        StandingOrderSource = null;
        Amount = amount;
        Type = type;
        Date = date;
        Title = title;
        Description = description;
        CounterParty = counterParty;
    }

    public static DomainResult<Transaction> Create(
        Guid userId,
        Guid categoryId,
        CategorySource categorySource,
        decimal amount,
        TransactionType type,
        DateOnly date,
        string title,
        string description, 
        string counterParty)
    {
        if(userId == Guid.Empty || categoryId == Guid.Empty)
            return DomainResult<Transaction>.Fail(DomainErrorCode.InvalidGuid);

        if(amount <= 0)
            return DomainResult<Transaction>.Fail(DomainErrorCode.InvalidAmount);

        if (string.IsNullOrWhiteSpace(title))
            return DomainResult<Transaction>.Fail(DomainErrorCode.TransactionTitleEmpty);

        if (!Enum.IsDefined(typeof(CategorySource), categorySource))
            return DomainResult<Transaction>.Fail(DomainErrorCode.InvalidCategorySource);

        return DomainResult<Transaction>.Ok(new Transaction(
            userId,
            categoryId,
            categorySource,
            amount,
            type,
            date,
            title,
            description,
            counterParty
        ));
    }

    public DomainResult<Unit> ChangeCategory(Guid categoryId, CategorySource source)
    {
        if(categoryId == Guid.Empty)
            return DomainResult<Unit>.Fail(DomainErrorCode.InvalidGuid); 

        CategoryId = categoryId;
        CategorySource = source;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeStandingOrder(Guid standingOrderId, StandingOrderSource source)
    {
        if(standingOrderId == Guid.Empty)
            return DomainResult<Unit>.Fail(DomainErrorCode.InvalidGuid); 

        StandingOrderId = standingOrderId;
        StandingOrderSource = source;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> RemoveStandingOrder()
    {
        StandingOrderId = null;
        StandingOrderSource = null;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeAmount(decimal amount, TransactionType type)
    {
        if(amount <= 0)
            return DomainResult<Unit>.Fail(DomainErrorCode.InvalidAmount);

        Amount = amount;
        Type = type;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeDate(DateOnly date)
    {
        Date = date;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return DomainResult<Unit>.Fail(DomainErrorCode.TransactionTitleEmpty);

        Title = title;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeDescription(string description)
    {
        Description = description;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeCounterParty(string counterParty)
    {
        CounterParty = counterParty;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> Delete()
    {
        return DomainResult<Unit>.Ok();
    }
}