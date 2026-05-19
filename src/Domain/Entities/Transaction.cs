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
    public DateOnly Date { get; private set; }
    public string Title { get; private set; } = null!;    // für leeren constructor
    public string Description { get; private set; } = null!;
    public string CounterParty { get; private set; } = null!;
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
        this.Id = Guid.NewGuid();
        this.UserId = userId;
        this.StandingOrderId = null;
        this.CategoryId = categoryId;
        this.CategorySource = categorySource;
        this.StandingOrderSource = null;
        this.Amount = amount;
        this.Type = type;
        this.Date = date;
        this.Title = title;
        this.Description = description;
        this.CounterParty = counterParty;
        this.CreatedAt = DateTime.UtcNow;
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
        if (userId == Guid.Empty || categoryId == Guid.Empty)
            return new InvalidGuidError();

        if (amount <= 0)
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
            counterParty);
    }

    public Result<Unit> ChangeCategory(Guid categoryId, CategorySource source)
    {
        if (categoryId == Guid.Empty)
            return new InvalidGuidError();

        this.CategoryId = categoryId;
        this.CategorySource = source;
        return Unit.Value;
    }

    public Result<Unit> ChangeStandingOrder(Guid standingOrderId, StandingOrderSource source)
    {
        if (standingOrderId == Guid.Empty)
            return new InvalidGuidError();

        this.StandingOrderId = standingOrderId;
        this.StandingOrderSource = source;
        return Unit.Value;
    }

    public Result<Unit> RemoveStandingOrder()
    {
        this.StandingOrderId = null;
        this.StandingOrderSource = null;
        return Unit.Value;
    }

    public Result<Unit> ChangeAmount(decimal amount, TransactionType type)
    {
        if (amount <= 0)
            return new InvalidAmountError();

        this.Amount = amount;
        this.Type = type;
        return Unit.Value;
    }

    public Result<Unit> ChangeDate(DateOnly date)
    {
        this.Date = date;
        return Unit.Value;
    }

    public Result<Unit> ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return new EmptyTransactionTitleError();

        this.Title = title;
        return Unit.Value;
    }

    public Result<Unit> ChangeDescription(string description)
    {
        this.Description = description;
        return Unit.Value;
    }

    public Result<Unit> ChangeCounterParty(string counterParty)
    {
        this.CounterParty = counterParty;
        return Unit.Value;
    }

    public static Result<Unit> Delete()
        => Unit.Value;
}
