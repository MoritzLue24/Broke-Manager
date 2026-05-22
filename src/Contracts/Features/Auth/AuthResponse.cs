namespace Contracts.Features.Auth;

public record AuthResponse(
    Guid UserId,
    string Token
);
