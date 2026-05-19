using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class StandingOrder
{
    private readonly List<Guid> _pauseHistory = [];

    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid? CategoryId { get; private set; }
    public string Name { get; private set; }
    private readonly List<Keyword> _keywords = [];
    public IReadOnlyCollection<Keyword> Keywords => this._keywords.AsReadOnly();
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public RecurrencePattern RecurrencePattern { get; private set; }
    public IReadOnlyCollection<Guid> PauseHistory => this._pauseHistory.AsReadOnly();
    public DateTime CreatedAt { get; }

    private StandingOrder(
        Guid userId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        RecurrencePattern recurrencePattern)
    {
        this.Id = Guid.NewGuid();
        this.UserId = userId;
        this.CategoryId = null;
        this.Name = name;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.RecurrencePattern = recurrencePattern;
        this.CreatedAt = DateTime.UtcNow;
    }

    public static Result<StandingOrder> Create(
        Guid userId,
        string name,
        DateOnly startDate,
        DateOnly? endDate,
        RecurrencePattern recurrencePattern)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NotImplementedException();

        if (startDate > (endDate ?? DateOnly.MaxValue))
            throw new NotImplementedException();

        return new StandingOrder(
            userId,
            name,
            startDate,
            endDate ?? DateOnly.MaxValue,
            recurrencePattern);
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

    public Result<Unit> AddKeyword(Keyword keyword)
    {
        if (this._keywords.Any(k => k == keyword))
            throw new NotImplementedException();

        this._keywords.Add(keyword);
        return Unit.Value;
    }

    public Result<Unit> RemoveKeyword(Keyword keyword)
    {
        if (!this._keywords.Remove(keyword))
            throw new NotImplementedException();

        return Unit.Value;
    }

    public Result<Unit> ChangeStartDate(DateOnly startDate)
    {
        if (startDate > this.EndDate)
            throw new NotImplementedException();

        this.StartDate = startDate;
        return Unit.Value;
    }

    public Result<Unit> ChangeEndDate(DateOnly endDate)
    {
        if (this.StartDate > endDate)
            throw new NotImplementedException();

        this.EndDate = endDate;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        this.EndDate = DateOnly.MaxValue;
        return Unit.Value;
    }

    public Result<Unit> ChangeRecurrencePattern(RecurrencePattern recurrencePattern)
    {
        this.RecurrencePattern = recurrencePattern;
        return Unit.Value;
    }

    public static Result<Unit> Delete()
        => Unit.Value;
}
