using Domain.Common;
using Domain.Common.Models;
using Domain.Enums;
using Domain.Events.Users;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : AggregateRoot
{
    public Email Email { get; private set; } = null!;   // für leeren constructor
    public Hash PasswordHash { get; private set; } = null!; // auch
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; }

    private User() : base(Guid.Empty) { } // Für EF Core?? FIXME: ????

    private User(Guid id, Email email, Hash passwordHash, Role role)
        : base(id)
    {
        this.Email = email;
        this.PasswordHash = passwordHash;
        this.Role = role;
        this.CreatedAt = DateTime.UtcNow;
    }

    public static Result<User> Create(Email email, Hash passwordHash)
    {
        var user = new User(Guid.NewGuid(), email, passwordHash, Role.User);

        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email));
        return user;
    }

    public Result<Unit> ChangeEmail(Email email)
    {
        var old = this.Email;
        this.Email = email;

        this.AddDomainEvent(new EmailChangedEvent(this.Id, this.Email, old));
        return Unit.Value;
    }

    public Result<Unit> ChangePasswordHash(Hash passwordHash)
    {
        this.PasswordHash = passwordHash;

        this.AddDomainEvent(new PasswordChangedEvent(this.Id, this.Email));
        return Unit.Value;
    }

    public Result<Unit> ChangeRole(Role role)
    {
        var old = this.Role;
        this.Role = role;

        this.AddDomainEvent(new RoleChangedEvent(this.Id, this.Email, this.Role, old));
        return Unit.Value;
    }

    public Result<Unit> Delete()
    {
        this.AddDomainEvent(new UserDeletedEvent(this.Email));
        return Unit.Value;
    }
}
