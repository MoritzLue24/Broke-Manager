namespace Contracts.Features.Users.Responses;

public record UserResponse(
    Guid Id,
    string Email,
    string Role,
    DateTime CreatedAt
);
