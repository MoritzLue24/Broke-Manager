using Api.DTOs.Auth;

namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequestDto registerDto);
        Task<AuthResult> LoginAsync(LoginRequestDto loginDto);
    }
}