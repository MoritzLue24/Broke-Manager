using Domain.Common;
using Domain.ValueObjects;



namespace Domain.Entiteis;

public class Category
{
    public Guid Id { get; private set;}
    public Guid UserId { get; private set;}
    public string Name { get; private set;}
    public bool IsDefault { get; private set;}
    public DateTime CreatedAt {get; private set;}

    private readonly List<Keyword> _keywords = new();

    public IReadOnlyCollection<Keyword> Keywords
    {
        get 
        {
            return _keywords.AsReadOnly();
        }
    }

    //private Category () { } Für EfCore?? Ich habe keine Ahnung wie das funktioniert
    private Category (Guid userId,string name, bool isDefault)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }

    public static DomainResult<Category> Create(Guid userId, string name, bool isDefault)
    {
        if (string.IsNullOrWhiteSpace(name)){
            return DomainResult<Category>.Fail(DomainErrorCode.CategoryNameEmpty);
        }
        
        return DomainResult<Category>.Ok(new Category(userId, name, isDefault));
    }

    public DomainResult<Unit> ChangeName(string name)
    {
        Name = name;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> AddKeyword(Keyword keyword)
    {
        if(IsDefault)
        {
            return DomainResult<Unit>.Fail(DomainErrorCode.NoKeywordForDefaultCategory);
        }
        
        if(_keywords.Any(k => k == keyword))
        {
            return DomainResult<Unit>.Fail(DomainErrorCode.NotUniqueKeywordWithinOneCategory);
        }

        _keywords.Add(keyword);
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> DeleteKeyword(Keyword keyword)
    {
        if(IsDefault)
        {
            return DomainResult<Unit>.Fail(DomainErrorCode.NoKeywordForDefaultCategory);
        }

        if (_keywords.Remove(keyword) == false)
        {
            return DomainResult<Unit>.Fail(DomainErrorCode.KeywordNotFounInCategory);
        }

        return DomainResult<Unit>.Ok();
    }

    
}