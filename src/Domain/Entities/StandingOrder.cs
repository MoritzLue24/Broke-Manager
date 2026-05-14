using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class StandingOrder
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid? CategoryId { get; private set; }
    public string Name { get; private set; }
    private readonly List<Keyword> _keywords = [];
    public IReadOnlyCollection<Keyword> Keywords
    {
        get => _keywords.AsReadOnly();
    }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public RecurrencePattern RecurrencePattern { get; private set; }
    private readonly List<Guid> _pauseHistory = [];
    public IReadOnlyCollection<Guid> PauseHistory
    {
        get => _pauseHistory.AsReadOnly();
    }
    public DateTime CreatedAt { get; }

    private StandingOrder(
        Guid userId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        RecurrencePattern recurrencePattern)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = null;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        RecurrencePattern = recurrencePattern;
        CreatedAt = DateTime.UtcNow;
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
            recurrencePattern
        );
    }

    public Result<Unit> ChangeCategory(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            return new InvalidGuidError();

        CategoryId = categoryId;
        return Unit.Value;
    }

    public Result<Unit> RemoveCategory()
    {
        CategoryId = null;
        return Unit.Value;
    }

    public Result<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NotImplementedException();

        Name = name;
        return Unit.Value;
    }

    public Result<Unit> AddKeyword(Keyword keyword)
    {
        if(_keywords.Any(k => k == keyword))
            throw new NotImplementedException();

        _keywords.Add(keyword);
        return Unit.Value;
    }

    public Result<Unit> RemoveKeyword(Keyword keyword)
    {
        if (_keywords.Remove(keyword) == false)
            throw new NotImplementedException();

        return Unit.Value;
    }

    public Result<Unit> ChangeStartDate(DateOnly startDate)
    {
        if (startDate > EndDate)
            throw new NotImplementedException();

        StartDate = startDate;
        return Unit.Value;
    }

    public Result<Unit> ChangeEndDate(DateOnly endDate)
    {
        if (StartDate > endDate)
            throw new NotImplementedException();

        EndDate = endDate;
        return Unit.Value;
    }

    public Result<Unit> MakeInfinite()
    {
        EndDate = DateOnly.MaxValue;
        return Unit.Value;
    }

    public Result<Unit> ChangeRecurrencePattern(RecurrencePattern recurrencePattern)
    {
        RecurrencePattern = recurrencePattern;
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        return Unit.Value;
    }
}