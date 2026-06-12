using System.Security.Cryptography;
using Application.Common.Interfaces.Security;

namespace Infrastructure.Security;

public class SessionTokenGenerator : ISessionTokenGenerator
{
    public string GenToken()
    {
        var bytes = new byte[32];    // 256 Bit
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
