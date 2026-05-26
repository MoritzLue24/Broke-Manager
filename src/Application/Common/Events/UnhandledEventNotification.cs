using Domain.Common.Models;
using MediatR;

namespace Application.Common.Events;

public record UnhandledEventNotification(IDomainEvent DomainEvent) : INotification;
