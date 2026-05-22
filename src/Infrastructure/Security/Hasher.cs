using Application.Common.Interfaces.Security;

namespace Infrastructure.Security;

public class Hasher : IHasher
{
    public string Hash(string plain)
        => BCrypt.Net.BCrypt.HashPassword(plain);

    public bool Verify(string plain, string hash)
        => BCrypt.Net.BCrypt.Verify(plain, hash);
}
