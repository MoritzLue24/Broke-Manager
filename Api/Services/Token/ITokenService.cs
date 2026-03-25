using Api.Models;

namespace Api.Services.Token
{
    public interface ITokenService
    {
        string CreateToken(Models.User user);
    }
}
