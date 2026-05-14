using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(
    Guid UserId,
    Guid TransactionId
) : IRequest<Result<TransactionDto>>;