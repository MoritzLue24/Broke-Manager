namespace Contracts.Features.Auth;

public record RegisterRequest(
    string Email,
    string Password,
    string ConfirmPassword
);
