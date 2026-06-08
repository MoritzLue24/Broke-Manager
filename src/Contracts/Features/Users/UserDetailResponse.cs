namespace Contracts.Features.Users;

public record UserDetailResponse(
    Guid Id,
    string Email,
    string Role,
    DateTime CreatedAt
);