using Domain.Common.Models;

namespace Domain.Events.Transactions;

public record TransactionDeletedEvent() : IDomainEvent;
