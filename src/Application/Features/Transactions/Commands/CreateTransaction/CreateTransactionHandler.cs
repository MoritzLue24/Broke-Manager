using Application.Common.Interfaces;
using Application.Common.Results;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryReaderRepository _categoryReaderRepo;

    public CreateTransactionHandler(
        IUnitOfWork uow,
        ITransactionRepository transactionRepo,
        ICategoryReaderRepository categoryReaderRepo)
    {
        _uow = uow;
        _transactionRepo = transactionRepo;
        _categoryReaderRepo = categoryReaderRepo;
    }

    public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand command, CancellationToken ct)
    {
        Guid categoryId;
        CategorySource categorySource;

        // Category specified -> source = Manual
        if (command.CategoryId.HasValue)
        {
            // Check if the given category exists
            if (!await _categoryReaderRepo.ExistsForUserAsync(command.UserId, command.CategoryId.Value))
                return Result<TransactionDto>.Fail(ErrorCode.CategoryNotFound);
            categoryId = command.CategoryId.Value;
            categorySource = CategorySource.Manual;
        }
        // No category specified -> (later auto-categorize) -> source = Unmatched with default category
        else
        {
            // Get default category
            Guid? categoryIdRes = await _categoryReaderRepo.GetDefaultByUserIdAsync(command.UserId);
            if (!categoryIdRes.HasValue)
                return Result<TransactionDto>.Fail(ErrorCode.DefaultCategoryNotFound);
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
            return domainResult.MapError<TransactionDto, Transaction>();

        _transactionRepo.Add(domainResult.Value);
        await _uow.SaveChangesAsync(ct);
        return domainResult.Map(t => t.ToDto());
    }
}