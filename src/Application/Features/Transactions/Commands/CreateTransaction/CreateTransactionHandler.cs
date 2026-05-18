using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

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
        _uow = uow;
        _transactionRepo = transactionRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand command, CancellationToken ct)
    {
        Guid categoryId;
        CategorySource categorySource;

        // Category specified -> source = Manual
        if (command.CategoryId.HasValue)
        {
            // Check if the given category exists
            if (!await _categoryRepo.ExistsForUserAsync(command.UserId, command.CategoryId.Value))
                return new CategoryNotFoundError();
            categoryId = command.CategoryId.Value;
            categorySource = CategorySource.Manual;
        }
        // No category specified -> (later auto-categorize) -> source = Unmatched with default category
        else
        {
            // Get default category
            Guid? categoryIdRes = await _categoryRepo.GetDefaultByUserIdAsync(command.UserId);
            if (!categoryIdRes.HasValue)
                return new DefaultCategoryNotFoundError();
            categoryId = categoryIdRes.Value;
            categorySource = CategorySource.Unmatched;
        }

        var domainResult = Transaction.Create(
            command.UserId,
            // TODO: Auto-categorize?
            categoryId,
            categorySource,
            command.Amount,
            command.Type,
            command.Date,
            command.Title,
            command.Description,
            command.CounterParty
        );
        // On failure, map the domain error to an application error
        // For now, the errors are basically the same but we dont want to
        // pass domain errors into the Api layer
        if (!domainResult.Success)
            return domainResult.Cast<TransactionDto>();

        _transactionRepo.Add(domainResult.Value);
        await _uow.SaveChangesAsync(ct);
        return domainResult.Cast(t => t.ToDto());
    }
}