using Domain.Enums;

namespace Application.Features.Auth.Common;

public record RegisterResult(
    Guid UserId,
    string Email,
    Role Role,
    DateTime CreatedAt,
    string Token
);
