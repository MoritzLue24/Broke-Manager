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
    public string Title {get; private set;} = null!;    // für leeren constructor
    public string Description { get; private set; } = null!;
    public string CounterParty {get; private set;} = null!;
    public DateTime CreatedAt { get; }

    private Transaction() { }

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

    public static Result<Transaction> Create(
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
            return new InvalidGuidError();

        if(amount <= 0)
            return new InvalidAmountError();

        if (string.IsNullOrWhiteSpace(title))
            return new EmptyTransactionTitleError();

        if (!Enum.IsDefined(typeof(CategorySource), categorySource))
            return new InvalidCategorySourceError();

        return new Transaction(
            userId,
            categoryId,
            categorySource,
            amount,
            type,
            date,
            title,
            description,
            counterParty
        );
    }

    public Result<Unit> ChangeCategory(Guid categoryId, CategorySource source)
    {
        if(categoryId == Guid.Empty)
            return new InvalidGuidError();

        CategoryId = categoryId;
        CategorySource = source;
        return Unit.Value;
    }

    public Result<Unit> ChangeStandingOrder(Guid standingOrderId, StandingOrderSource source)
    {
        if(standingOrderId == Guid.Empty)
            return new InvalidGuidError();

        StandingOrderId = standingOrderId;
        StandingOrderSource = source;
        return Unit.Value;
    }

    public Result<Unit> RemoveStandingOrder()
    {
        StandingOrderId = null;
        StandingOrderSource = null;
        return Unit.Value;
    }

    public Result<Unit> ChangeAmount(decimal amount, TransactionType type)
    {
        if(amount <= 0)
            return new InvalidAmountError();

        Amount = amount;
        Type = type;
        return Unit.Value;
    }

    public Result<Unit> ChangeDate(DateOnly date)
    {
        Date = date;
        return Unit.Value;
    }

    public Result<Unit> ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return new EmptyTransactionTitleError();

        Title = title;
        return Unit.Value;
    }

    public Result<Unit> ChangeDescription(string description)
    {
        Description = description;
        return Unit.Value;
    }

    public Result<Unit> ChangeCounterParty(string counterParty)
    {
        CounterParty = counterParty;
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        return Unit.Value;
    }
}