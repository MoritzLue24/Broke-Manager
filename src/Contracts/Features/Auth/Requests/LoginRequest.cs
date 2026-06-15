namespace Contracts.Features.Auth.Requests;

public record LoginRequest(
    string Email,
    string Password
);
