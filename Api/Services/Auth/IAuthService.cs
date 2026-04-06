using Api.DTOs.Auth;


namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUserAsync(string email, string password);
        Task<AuthResponseDto?> ValidateUserAsync(string email, string password);
        string GenJwtToken(int userId);
    }
}