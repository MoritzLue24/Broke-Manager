using Application.Common.Interfaces.Security;
using Application.Features.Analytics.Contracts;
using Application.Features.Analytics.Services;
using Application.Features.Transactions.Interfaces;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Analytics.Queries.Summary;

public class SummaryQueryHandler : IRequestHandler<SummaryQuery, Result<SummaryResult>>
{
    private readonly IUserContext _userContext;
    private readonly ITransactionRepository _transactionRepo;

    public SummaryQueryHandler(
        IUserContext userContext,
        ITransactionRepository transactionRepo)
    {
        this._userContext = userContext;
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<SummaryResult>> Handle(
        SummaryQuery request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId ?? Guid.Empty;
        if (!Enum.TryParse<AnalyticsPeriodRange>(request.Period.Range, true, out var range))
            throw new InvalidOperationException();  // Because we assume the request is valid (its validated)

        (DateOnly? from, DateOnly? to) = AnalyticsService.CalculatePeriod(range, request.Period.From, request.Period.To);
        var transactions = await this._transactionRepo.GetWithFilterAsync(userId, null, null, from, to, cancellationToken);

        var balance = transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        var income = transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : 0);
        var expenses = transactions.Sum(t => t.Type == TransactionType.Income ? 0 : t.Amount);

        return new SummaryResult(balance, income, expenses);
    }
}
