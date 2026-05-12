using Application.Common.Results;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid UserId,
    Guid? CategoryId,
    decimal Amount,
    TransactionType Type,
    DateOnly Date,
    string Title,
    string Description,
    string CounterParty
) : IRequest<Result<TransactionDto>>;