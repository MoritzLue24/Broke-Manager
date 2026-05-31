namespace Contracts.Features.Auth;

public record LoginRequest(
    string Email,
    string Password
);
