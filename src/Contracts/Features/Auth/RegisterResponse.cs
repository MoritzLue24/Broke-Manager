namespace Contracts.Features.Auth;

public record RegisterResponse(
    Guid UserId,
    string Email,
    string Role,
    DateTime CreatedAt
);
