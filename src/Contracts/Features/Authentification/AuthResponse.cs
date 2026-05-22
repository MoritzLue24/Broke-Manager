namespace Contracts.Features.Authentification;

public record AuthResponse(
    Guid UserId,
    string Token
);
