using Domain.Common;
using Domain.ValueObjects;



namespace Domain.Entiteis;

public class Category
{
    public Guid Id { get; private set}
    public Guid UserId { get; private set;}
    public string Name { get; private set;}
    public bool IsDefault { get; private set;}
    public DateTime СreatedAt {get; private set;}

    
}