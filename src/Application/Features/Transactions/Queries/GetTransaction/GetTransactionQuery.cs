using Application.Common.Results;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(
    Guid UserId,
    Guid TransactionId
) : IRequest<Result<TransactionDto>>;