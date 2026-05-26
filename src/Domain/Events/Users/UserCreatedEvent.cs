using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Events.Users;

public record UserCreatedEvent(
    Guid UserId,
    Email Email
) : IDomainEvent;
