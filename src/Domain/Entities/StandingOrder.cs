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

    public static DomainResult<StandingOrder> Create(
        Guid userId,
        string name,
        DateOnly startDate,
        DateOnly? endDate,
        RecurrencePattern recurrencePattern)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainResult<StandingOrder>.Fail(DomainErrorCode.StandingOrderNameEmpty);

        if (startDate > (endDate ?? DateOnly.MaxValue))
            return DomainResult<StandingOrder>.Fail(DomainErrorCode.StandingOrderDatesInvalid);

        return DomainResult<StandingOrder>.Ok(new StandingOrder(
            userId,
            name,
            startDate,
            endDate ?? DateOnly.MaxValue,
            recurrencePattern
        ));
    }

    public DomainResult<Unit> ChangeCategory(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            return DomainResult<Unit>.Fail(DomainErrorCode.InvalidGuid);

        CategoryId = categoryId;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> RemoveCategory()
    {
        CategoryId = null;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DomainResult<Unit>.Fail(DomainErrorCode.StandingOrderNameEmpty);

        Name = name;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> AddKeyword(Keyword keyword)
    {
        if(_keywords.Any(k => k == keyword))
            return DomainResult<Unit>.Fail(DomainErrorCode.KeywordAlreadyExists);

        _keywords.Add(keyword);
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> RemoveKeyword(Keyword keyword)
    {
        if (_keywords.Remove(keyword) == false)
            return DomainResult<Unit>.Fail(DomainErrorCode.KeywordNotFound);

        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeStartDate(DateOnly startDate)
    {
        if (startDate > EndDate)
            return DomainResult<Unit>.Fail(DomainErrorCode.StandingOrderDatesInvalid);

        StartDate = startDate;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeEndDate(DateOnly endDate)
    {
        if (StartDate > endDate)
            return DomainResult<Unit>.Fail(DomainErrorCode.StandingOrderDatesInvalid);

        EndDate = endDate;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> MakeInfinite()
    {
        EndDate = DateOnly.MaxValue;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeRecurrencePattern(RecurrencePattern recurrencePattern)
    {
        RecurrencePattern = recurrencePattern;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> Delete()
    {
        return DomainResult<Unit>.Ok();
    }
}