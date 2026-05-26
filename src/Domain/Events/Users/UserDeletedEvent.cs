using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Events.Users;

public record UserDeletedEvent(
    Email Email
) : IDomainEvent;
