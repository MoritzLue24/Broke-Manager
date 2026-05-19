using MediatR;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Application.Common;
using Application.Common.Interfaces.Persistence;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public CreateTransactionHandler(
        IUnitOfWork uow,
        ITransactionRepository transactionRepo,
        ICategoryRepository categoryRepo)
    {
        this._uow = uow;
        this._transactionRepo = transactionRepo;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<TransactionDto>> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        Guid categoryId;
        CategorySource categorySource;

        // Category specified -> source = Manual
        if (request.CategoryId.HasValue)
        {
            // Check if the given category exists
            if (!await this._categoryRepo.ExistsForUserAsync(request.UserId, request.CategoryId.Value))
                return new CategoryNotFoundError();
            categoryId = request.CategoryId.Value;
            categorySource = CategorySource.Manual;
        }
        // No category specified -> (later auto-categorize) -> source = Unmatched with default category
        else
        {
            // Get default category
            Guid? categoryIdRes = await this._categoryRepo.GetDefaultByUserIdAsync(request.UserId);
            if (!categoryIdRes.HasValue)
                return new DefaultCategoryNotFoundError();
            categoryId = categoryIdRes.Value;
            categorySource = CategorySource.Unmatched;
        }

        var domainResult = Transaction.Create(
            request.UserId,
            // TODO: Auto-categorize?
            categoryId,
            categorySource,
            request.Amount,
            request.Type,
            request.Date,
            request.Title,
            request.Description,
            request.CounterParty
        );
        // On failure, map the domain error to an application error
        // For now, the errors are basically the same but we dont want to
        // pass domain errors into the Api layer
        if (!domainResult.Success)
            return domainResult.Cast<TransactionDto>();

        this._transactionRepo.Add(domainResult.Value);
        await this._uow.SaveChangesAsync(cancellationToken);
        return domainResult.Cast(t => t.ToDto());
    }
}