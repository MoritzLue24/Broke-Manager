using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; }
    public Email Email { get; private set; }
    public Hash PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; }

    // private User() { } // Für EF Core??

    private User(Email email, Hash passwordHash, Role role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static DomainResult<User> Create(Email email, Hash passwordHash)
        => DomainResult<User>.Ok(new(email, passwordHash, Role.User));

    public DomainResult<Unit> ChangeEmail(Email email)
    {
        Email = email;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangePasswordHash(Hash passwordHash)
    {
        PasswordHash = passwordHash;
        return DomainResult<Unit>.Ok();
    }

    public DomainResult<Unit> ChangeRole(Role role)
    {
        Role = role;
        return DomainResult<Unit>.Ok();
    }
}
