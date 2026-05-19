using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category
{
    private readonly List<Keyword> _keywords = [];

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = null!;    // für leeren constructor
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Keyword> Keywords => this._keywords.AsReadOnly();

    private Category() { }

    private Category(Guid userId, string name, bool isDefault)
    {
        this.Id = Guid.NewGuid();
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

        return new Category(userId, name, isDefault);
    }

    public Result<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        this.Name = name;
        return Unit.Value;
    }

    public Result<Unit> AddKeyword(Keyword keyword)
    {
        if (this.IsDefault)
            return new CategoryIsDefaultError();

        if (this._keywords.Any(k => k == keyword))
            return new DuplicateKeywordError();

        this._keywords.Add(keyword);
        return Unit.Value;
    }

    public Result<Unit> RemoveKeyword(Keyword keyword)
    {
        if (!this._keywords.Remove(keyword))
            return new KeywordNotFoundError();

        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        if (this.IsDefault)
            return new CategoryIsDefaultError();

        return Unit.Value;
    }
}
