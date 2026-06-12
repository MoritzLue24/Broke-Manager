using System.Security.Cryptography;
using Application.Common.Interfaces.Security;

namespace Infrastructure.Security;

public class TokenGenerator : ITokenGenerator
{
    public string GenToken()
    {
        var bytes = new byte[32];    // 256 Bit
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
