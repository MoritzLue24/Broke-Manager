using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.AutoAssign.Contracts;
using Application.Features.AutoAssign.Services;
using Application.Features.Categories.Interfaces;
using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Interfaces;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.AutoAssign.Commands.AutoAssign;

public class AutoAssignCommandHandler : IRequestHandler<AutoAssignCommand, Result<IReadOnlyCollection<AutoAssignResult>>>
{
    private readonly IUserContext _userContext;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IUnitOfWork _uow;

    public AutoAssignCommandHandler(
        IUserContext userContext,
        ITransactionRepository transactionRepo,
        ICategoryRepository categoryRepo,
        IUnitOfWork uow)
    {
        this._userContext = userContext;
        this._transactionRepo = transactionRepo;
        this._categoryRepo = categoryRepo;
        this._uow = uow;
    }

    public async Task<Result<IReadOnlyCollection<AutoAssignResult>>> Handle(
        AutoAssignCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId ?? Guid.Empty;

        var transactions = await this._transactionRepo.GetWithFilterAsync(
            userId,
            request.Filter.TransactionIds,
            request.Filter.CategoryIds,
            request.Filter.From,
            request.Filter.To,
            cancellationToken
        );
        // TODO: maybe directly in query
        if (!request.OverwriteManual.HasValue || !request.OverwriteManual.Value)
            transactions = transactions.Where(t
                => t.CategorySource == CategorySource.Auto ||
                    t.CategorySource == CategorySource.Unmatched // ||
                    // t.CategorySource == CategorySource.FromStandingOrder
            ).ToList();

        var categories = request.UseCategoryIds is null
            ? await this._categoryRepo.GetAllByUserIdAsync(userId, cancellationToken)
            : await this._categoryRepo.GetAllWithIdsAsync(userId, request.UseCategoryIds, cancellationToken);

        var defaultCategoryId = await this._categoryRepo.GetDefaultIdByUserIdAsync(userId, cancellationToken)
            ?? throw new NotImplementedException();

        List<AutoAssignResult> results = [];
        foreach (var transaction in transactions)
        {
            var match = AutoAssignService.FindMatch(transaction, categories, defaultCategoryId);

            if (match.CategoryId != transaction.CategoryId)
                transaction.ChangeCategory(match.CategoryId, match.CategorySource);

            results.Add(new(transaction.ToResult(), match.ConflictingCategories));
        }

        await this._uow.SaveChangesAsync(cancellationToken);
        return results;
    }
}
