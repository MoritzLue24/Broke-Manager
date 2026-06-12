using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Users.Contracts;

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
