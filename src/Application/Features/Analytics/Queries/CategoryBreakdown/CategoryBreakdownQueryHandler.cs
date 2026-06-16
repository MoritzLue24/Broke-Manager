using Application.Common.Interfaces.Security;
using Application.Features.Analytics.Contracts;
using Application.Features.Analytics.Services;
using Application.Features.Categories.Contracts;
using Application.Features.Categories.Interfaces;
using Application.Features.Transactions.Interfaces;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Analytics.Queries.CategoryBreakdown;

public class CategoryBreakdownQueryHandler : IRequestHandler<CategoryBreakdownQuery, Result<IReadOnlyCollection<CategoryBreakdownResult>>>
{
    private readonly IUserContext _userContext;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public CategoryBreakdownQueryHandler(
        IUserContext userContext,
        ITransactionRepository transactionRepo,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._transactionRepo = transactionRepo;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<IReadOnlyCollection<CategoryBreakdownResult>>> Handle(
        CategoryBreakdownQuery request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId ?? Guid.Empty;
        if (!Enum.TryParse<AnalyticsPeriodRange>(request.Period.Range, true, out var range))
            throw new InvalidOperationException();  // Because we assume the request is valid (its validated)

        var categories = await this._categoryRepo.GetAllByUserIdAsync(userId, cancellationToken);

        (DateOnly? from, DateOnly? to) = AnalyticsService.CalculatePeriod(range, request.Period.From, request.Period.To);
        var transactions = await this._transactionRepo.GetWithFilterAsync(userId, null, null, from, to, cancellationToken);

        List<CategoryBreakdownResult> results = [];
        foreach (var category in categories)
        {
            // TODO: put directly in query
            var categoryTransactions = transactions.Where(t => t.CategoryId == category.Id);
            decimal expenses = categoryTransactions.Sum(t => t.Type == TransactionType.Expense ? t.Amount : 0);

            results.Add(new CategoryBreakdownResult(category.ToResult(), expenses, 0));
        }

        var totalExpenses = results.Sum(r => r.Expenses);
        results = results.Select(r=> new CategoryBreakdownResult(
            r.CategoryResult,
            r.Expenses,
            (double)r.Expenses / (double)totalExpenses)
        ).ToList();

        return results;
    }
}
