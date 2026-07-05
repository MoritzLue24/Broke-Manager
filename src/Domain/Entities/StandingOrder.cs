using Domain.Common;
using Domain.Common.Models;
using Domain.Enums;

namespace Domain.Entities;

public class StandingOrder : Entity
{
    private readonly List<Guid> _pauseHistory = [];

    public Guid UserId { get; }
    public Guid? CategoryId { get; private set; }
    public string Name { get; private set; }

    // Transaction identification
    public decimal TransactionAmount { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string TransactionTitle { get; private set; }
    public string TransactionCounterParty { get; private set; }
    public string TransactionDescription { get; private set; }

    // Reoccurrence
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public Interval Interval { get; private set; }
    public int ExecutionDay { get; private set; }   // Relative to Interval
    public IReadOnlyCollection<Guid> PauseHistory => this._pauseHistory.AsReadOnly();

    public DateTime CreatedAt { get; }

    private StandingOrder(
        Guid id,
        Guid userId,
        Guid? categoryId,
        string name,
        decimal transactionAmount,
        TransactionType transactionType,
        string transactionTitle,
        string transactionCounterParty,
        string transactionDescription,
        DateOnly startDate,
        DateOnly endDate,
        Interval interval,
        int executionDay)
        : base(id)
    {
        this.UserId = userId;
        this.CategoryId = categoryId;
        this.Name = name;
        this.TransactionAmount = transactionAmount;
        this.TransactionType = transactionType;
        this.TransactionTitle = transactionTitle;
        this.TransactionCounterParty = transactionCounterParty;
        this.TransactionDescription = transactionDescription;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.Interval = interval;
        this.ExecutionDay = executionDay;
        this.CreatedAt = DateTime.UtcNow;
    }

    public static Result<StandingOrder> Create(
        Guid userId,
        Guid? categoryId,
        string name,
        decimal transactionAmount,
        TransactionType transactionType,
        string transactionTitle,
        string transactionCounterParty,
        string transactionDescription,
        DateOnly startDate,
        DateOnly? endDate,
        Interval interval,
        int executionDay)
    {
        if (userId == Guid.Empty | (categoryId is not null && categoryId == Guid.Empty))
            return new InvalidGuidError();

        if (string.IsNullOrWhiteSpace(name))
            return new EmptyStandingOrderNameError();

        if (transactionAmount <= 0)
            return new InvalidAmountError();

        if (string.IsNullOrWhiteSpace(transactionTitle))
            return new EmptyTransactionTitleError();

        if (transactionCounterParty is null)
            return new TransactionCounterPartyNullError();

        if (transactionDescription is null)
            return new TransactionDescriptionNullError();

        if (startDate > (endDate ?? DateOnly.MaxValue))
            return new DateFromGreaterThanToError();

        if (executionDay < 1)
            return new InvalidExecutionDayError();

        return new StandingOrder(
            Guid.NewGuid(),
            userId,
            categoryId,
            name,
            transactionAmount,
            transactionType,
            transactionTitle,
            transactionCounterParty,
            transactionDescription,
            startDate,
            endDate ?? DateOnly.MaxValue,
            interval,
            executionDay);
    }

    public Result<Unit> ChangeCategory(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            return new InvalidGuidError();

        this.CategoryId = categoryId;
        return Unit.Value;
    }

    public Result<Unit> RemoveCategory()
    {
        this.CategoryId = null;
        return Unit.Value;
    }

    public Result<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NotImplementedException();

        this.Name = name;
        return Unit.Value;
    }

    public Result<Unit> ChangeTransactionAmount(decimal amount, TransactionType type)
    {
        if (amount <= 0)
            return new InvalidAmountError();

        this.TransactionAmount = amount;
        this.TransactionType = type;
        return Unit.Value;
    }

    public Result<Unit> ChangeTransactionTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return new EmptyTransactionTitleError();

        this.TransactionTitle = title;
        return Unit.Value;
    }

    public Result<Unit> ChangeTransactionCounterParty(string counterParty)
    {
        if (counterParty is null)
            return new TransactionCounterPartyNullError();
    
        this.TransactionCounterParty = counterParty;
        return Unit.Value;
    }

    public Result<Unit> ChangeTransactionDescription(string description)
    {
        if (description is null)
            return new TransactionDescriptionNullError();
    
        this.TransactionDescription = description;
        return Unit.Value;
    }

    public Result<Unit> ChangeStartDate(DateOnly startDate)
    {
        if (startDate > this.EndDate)
            return new DateFromGreaterThanToError();

        this.StartDate = startDate;
        return Unit.Value;
    }

    public Result<Unit> ChangeEndDate(DateOnly endDate)
    {
        if (this.StartDate > endDate)
            return new DateFromGreaterThanToError();

        this.EndDate = endDate;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        this.EndDate = DateOnly.MaxValue;
        return Unit.Value;
    }

    public Result<Unit> ChangeInterval(Interval interval)
    {
        this.Interval = interval;
        return Unit.Value;
    }

    public Result<Unit> ChangeExecutionDay(int executionDay)
    {
        if (executionDay < 0)
            return new InvalidExecutionDayError();

        this.ExecutionDay = executionDay;
        return Unit.Value;
    }

    public Result<DateOnly> GetActualDate(DateOnly referenceDate)
    {
        DateOnly periodStart = this.Interval switch
        {
            Interval.Weekly => referenceDate.AddDays(1 - (
                referenceDate.DayOfWeek == DayOfWeek.Sunday
                ? 7
                : (int)referenceDate.DayOfWeek
            )),
            Interval.Monthly => new DateOnly(
                referenceDate.Year,
                referenceDate.Month,
                1
            ),
            Interval.Quarterly => new DateOnly(
                referenceDate.Year,
                ((referenceDate.Month - 1) / 3 * 3) + 1,
                1
            ),
            Interval.Yearly => new DateOnly(referenceDate.Year, 1, 1),
            _ => throw new NotImplementedException()
        };

        DateOnly periodEnd = this.Interval switch
        {
            Interval.Weekly => periodStart.AddDays(6),
            Interval.Monthly => periodStart.AddMonths(1).AddDays(-1),
            Interval.Quarterly => periodStart.AddMonths(3).AddDays(-1),
            Interval.Yearly => periodStart.AddYears(1).AddDays(-1),
            _ => throw new NotImplementedException()
        };

        DateOnly executionDate = periodStart.AddDays(this.ExecutionDay - 1);
        return executionDate > periodEnd
            ? periodEnd
            : executionDate;
    }

    public static Result<Unit> Delete()
        => Unit.Value;
}
