namespace Contracts.Features.Authentification;

public record RegisterRequest(
    string Email,
    string Password,
    string ConfirmPassword
);
