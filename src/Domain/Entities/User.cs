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
        this.Id = Guid.NewGuid();
        this.Email = email;
        this.PasswordHash = passwordHash;
        this.Role = role;
        this.CreatedAt = DateTime.UtcNow;
    }

    public static Result<User> Create(Email email, Hash passwordHash)
        => new User(email, passwordHash, Role.User);

    public Result<Unit> ChangeEmail(Email email)
    {
        this.Email = email;
        return Unit.Value;
    }

    public Result<Unit> ChangePasswordHash(Hash passwordHash)
    {
        this.PasswordHash = passwordHash;
        return Unit.Value;
    }

    public Result<Unit> ChangeRole(Role role)
    {
        this.Role = role;
        return Unit.Value;
    }
}
