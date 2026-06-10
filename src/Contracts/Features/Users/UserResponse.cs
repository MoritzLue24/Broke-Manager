namespace Contracts.Features.Users;

public record UserResponse(
    Guid Id,
    string Email,
    string Role,
    DateTime CreatedAt
);