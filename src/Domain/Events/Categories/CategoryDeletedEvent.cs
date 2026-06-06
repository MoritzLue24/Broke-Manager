using Domain.Common.Models;

namespace Domain.Events.Categories;

public record CategoryDeletedEvent(
    Guid CategoryId
) : IDomainEvent;
