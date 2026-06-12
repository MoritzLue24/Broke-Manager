using Domain.Common;
using Domain.Common.Models;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Session : Entity
{
    private readonly List<Role> _roles = [];

    public Guid UserId { get; }
    public IReadOnlyCollection<Role> Roles => this._roles.AsReadOnly();
    public Hash TokenHash { get; } = null!;    // Used for verification, identification -> id
    public DateTime ExpiresAt { get; }
    public DateTime LastSeen { get; private set; }
    public DateTime CreatedAt { get; }

    private Session() : base(Guid.Empty) { }

    public Session(
        Guid id,
        Guid userId,
        List<Role> roles,
        Hash tokenHash,
        DateTime expiresAt)
        : base(id)
    {
        this.UserId = userId;
        this._roles = roles;
        this.TokenHash = tokenHash;
        this.ExpiresAt = expiresAt;
        this.LastSeen = DateTime.UtcNow;
        this.CreatedAt = DateTime.UtcNow;
    }

    public static Result<Session> Create(
        Guid userId,
        IEnumerable<Role> roles,
        Hash tokenHash,
        DateTime expiresAt)
    {
        if (userId == Guid.Empty)
            return new InvalidGuidError();

        return new Session(
            Guid.NewGuid(),
            userId,
            roles.ToList(),
            tokenHash,
            expiresAt
        );
    }
}
