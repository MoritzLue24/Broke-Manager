using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Events.Users;

public record EmailChangedEvent(
    Guid UserId,
    Email NewEmail,
    Email OldEmail
) : IDomainEvent;
