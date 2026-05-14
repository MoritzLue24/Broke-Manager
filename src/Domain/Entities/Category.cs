using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category
{
    public Guid Id { get; private set;}
    public Guid UserId { get; private set;}
    public string Name { get; private set;} = null!;    // für leeren constructor
    public bool IsDefault { get; private set;}
    public DateTime CreatedAt {get; private set;}

    private readonly List<Keyword> _keywords = [];

    public IReadOnlyCollection<Keyword> Keywords
    {
        get 
        {
            return _keywords.AsReadOnly();
        }
    }

    private Category () { }
    
    private Category (Guid userId, string name, bool isDefault)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Category> Create(Guid userId, string name, bool isDefault)
    {
        if(userId == Guid.Empty)
            return new InvalidGuidError();

        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        return new Category(userId, name, isDefault);
    }

    public Result<Unit> ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new EmptyCategoryNameError();

        Name = name;
        return Unit.Value;
    }

    public Result<Unit> AddKeyword(Keyword keyword)
    {
        if(IsDefault)
            return new CategoryIsDefaultError();

        if(_keywords.Any(k => k == keyword))
            return new DuplicateKeywordError();

        _keywords.Add(keyword);
        return Unit.Value;
    }

    public Result<Unit> RemoveKeyword(Keyword keyword)
    {
        if (_keywords.Remove(keyword) == false)
            return new KeywordNotFoundError();

        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        if (IsDefault)
            return new CategoryIsDefaultError();

        return Unit.Value;
    }
}