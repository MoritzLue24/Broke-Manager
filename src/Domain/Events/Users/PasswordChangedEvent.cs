using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Events.Users;

public record PasswordChangedEvent(
    Guid UserId,
    Email Email
) : IDomainEvent;
