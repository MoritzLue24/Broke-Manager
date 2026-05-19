namespace Application.Common.Interfaces.Security;

public interface IHasher
{
    string Hash(string plain);
}