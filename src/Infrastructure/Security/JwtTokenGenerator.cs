using Application.Common.Interfaces.Security;

namespace Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public string GenToken(Guid userId) // TODO
        => throw new NotImplementedException();
}