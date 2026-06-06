using Domain.Common;
using Domain.Common.Models;
using Domain.Events.Categories;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category : AggregateRoot
{
    private readonly List<MatchingRule> _matchingRules = [];

    public Guid UserId { get; private set; }
    public string Name { get; private set; } = null!;    // für leeren constructor
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<MatchingRule> MatchingRules => this._matchingRules.AsReadOnly();

    private Category() : base(Guid.Empty) { }

    private Category(
        Guid id,
        Guid userId,
        string name,
        bool isDefault)
        : base(id)
    {
        this.UserId = userId;
        this.Name = name;
        this.IsDefault = isDefault;
        this.CreatedAt = DateTime.UtcNow;
    }

    public static Result<Category> Create(Guid userId, string name, bool isDefault)
    {
        if (userId == Guid.Empty)
            return new InvalidGuidError();

        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        var category = new Category(Guid.NewGuid(), userId, name, isDefault);

        category.AddDomainEvent(new CategoryCreatedEvent(category.Id));
        return category;
    }

    public Result<Unit> ChangeName(string name)
    {
        if (this.IsDefault)
            return new CategoryIsDefaultError();

        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        this.Name = name;
        return Unit.Value;
    }

    public Result<Unit> AddRule(MatchingRule rule)
    {
        if (this.IsDefault)
            return new CategoryIsDefaultError();

        if (this._matchingRules.Any(r => r == rule))
            return new DuplicateKeywordError();

        this._matchingRules.Add(rule);
        return Unit.Value;
    }

    public Result<Unit> RemoveRule(MatchingRule rule)
    {
        if (this.IsDefault)
            return new CategoryIsDefaultError();

        if (!this._matchingRules.Remove(rule))
            return new KeywordNotFoundError();

        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        if (this.IsDefault)
            return new CategoryIsDefaultError();

        this.AddDomainEvent(new CategoryDeletedEvent(this.Id));
        return Unit.Value;
    }
}
