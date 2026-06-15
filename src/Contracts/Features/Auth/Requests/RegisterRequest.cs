namespace Contracts.Features.Auth.Requests;

public record RegisterRequest(
    string Email,
    string Password,
    string ConfirmPassword
);
