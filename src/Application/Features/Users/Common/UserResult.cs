using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Users.Common;

/// Basic transaction result. Other dtos like 
/// CreateDto, UpdateDto are now Commands / Queries.
/// Maybe later more Dtos, like TransactionDetailResult
public record UserResult(
    Guid Id,
    string Email,
    Role Role,
    DateTime CreatedAt
);

public static class UserExtension
{
    public static UserResult ToResult(this User u)
        => new(
            u.Id,
            u.Email.Value,
            u.Role,
            u.CreatedAt
        );
}
