using Api.DTOs.Auth;

namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDto registerDto);
        Task<string> LoginAsync(LoginRequestDto loginDto);
        UserClaimsDto GetCurrentUser();
    }
}