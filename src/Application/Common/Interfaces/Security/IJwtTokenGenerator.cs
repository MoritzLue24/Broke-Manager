namespace Application.Common.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string GenToken(Guid userId);
}
