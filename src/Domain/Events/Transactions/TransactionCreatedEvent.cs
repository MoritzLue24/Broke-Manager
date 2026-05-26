using Domain.Common.Models;

namespace Domain.Events.Transactions;

public record TransactionCreatedEvent(
    Guid TransactionId
) : IDomainEvent;
