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

    private Category () { } //Für EfCore?? Ich habe keine Ahnung wie das funktioniert
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

    public DomainResult<Unit> ChangeIsDefault(bool isDefault)
    {
        IsDefault = isDefault;
        return DomainResult<Unit>.Ok();
    }





}