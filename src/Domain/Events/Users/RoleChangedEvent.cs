using Domain.Common.Models;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Events.Users;

public record RoleChangedEvent(
    Guid UserId,
    Email Email,
    Role NewRole,
    Role OldRole
) : IDomainEvent;
