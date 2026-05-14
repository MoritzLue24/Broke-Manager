using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; }
    public Email Email { get; private set; } = null!;   // für leeren constructor
    public Hash PasswordHash { get; private set; } = null!; // auch
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; }

    private User() { } // Für EF Core??

    private User(Email email, Hash passwordHash, Role role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<User> Create(Email email, Hash passwordHash)
    {
        return new User(email, passwordHash, Role.User);
    }

    public Result<Unit> ChangeEmail(Email email)
    {
        Email = email;
        return Unit.Value;
    }

    public Result<Unit> ChangePasswordHash(Hash passwordHash)
    {
        PasswordHash = passwordHash;
        return Unit.Value;
    }

    public Result<Unit> ChangeRole(Role role)
    {
        Role = role;
        return Unit.Value;
    }
}
