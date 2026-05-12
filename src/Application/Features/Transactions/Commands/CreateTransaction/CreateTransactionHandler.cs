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
        if (command.CategoryId.HasValue)
        {
            if (!await _categoryReaderRepo.ExistsForUser(command.UserId, command.CategoryId.Value))
                return Result<TransactionDto>.Fail(ErrorCode.CategoryNotFound);
            categoryId = command.CategoryId.Value;
        }
        else
        {
            Guid? categoryIdRes = await _categoryReaderRepo.GetDefaultByUserIdAsync(command.UserId);
            if (!categoryIdRes.HasValue)
                return Result<TransactionDto>.Fail(ErrorCode.DefaultCategoryNotFound);
            categoryId = categoryIdRes.Value;
        }

        var domainResult = Transaction.Create(
            command.UserId,
            // TODO: Auto-categorize?
            categoryId,
            CategorySource.Manual,
            command.Amount,
            command.Type,
            command.Date,
            command.Title,
            command.Description,
            command.CounterParty
        );
        if (!domainResult.Success)
            return domainResult.MapError<TransactionDto, Transaction>();

        _transactionRepo.Add(domainResult.Value);
        await _uow.SaveChangesAsync(ct);
        return domainResult.Map(t => t.ToDto());
    }
}