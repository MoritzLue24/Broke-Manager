using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Application.Features.Transactions;
using Application.Features.Transactions.Commands.CreateTransaction;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using NSubstitute;

namespace Application.Tests.Features.Transactions.Commands;

public class CreateTransactionTests
{
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public CreateTransactionTests()
    {
        // Mock interfaces: in each test, set what interface methods should return with what parameters
        _uow = Substitute.For<IUnitOfWork>();
        _transactionRepo = Substitute.For<ITransactionRepository>();
        _categoryRepo = Substitute.For<ICategoryRepository>();
    }

    [Fact]
    public async Task ShouldCreateTransaction_WhenManualCategoryExists()
    {
        // Setup
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        _categoryRepo.ExistsForUserAsync(userId, categoryId).Returns(true);

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            userId,
            categoryId,
            20,
            TransactionType.Expense,
            date,
            "Essen gehen",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal(categoryId, result.Value.CategoryId);
        Assert.Equal(CategorySource.Manual, result.Value.CategorySource);
        Assert.Equal(20, result.Value.Amount);
        Assert.Equal(TransactionType.Expense, result.Value.Type);
        Assert.Equal(date, result.Value.Date);
        Assert.Equal("Essen gehen", result.Value.Title);
        Assert.Equal("", result.Value.Description);
        Assert.Equal("Pizza place", result.Value.CounterParty);

        await _categoryRepo.Received(1).ExistsForUserAsync(userId, categoryId);
        await _categoryRepo.Received(0).GetDefaultByUserIdAsync(Arg.Any<Guid>());
        _transactionRepo.Received(1).Add(Arg.Any<Transaction>());
        await _uow.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldReturnCategoryNotFound_WhenManualCategoryDoesntExist()
    {
        // Setup
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        _categoryRepo.ExistsForUserAsync(userId, categoryId).Returns(false);

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            userId,
            categoryId,
            20,
            TransactionType.Expense,
            date,
            "Essen gehen",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(new CategoryNotFoundError(), result.Error);

        await _categoryRepo.Received(1).ExistsForUserAsync(userId, categoryId);
        await _categoryRepo.Received(0).GetDefaultByUserIdAsync(Arg.Any<Guid>());
        _transactionRepo.Received(0).Add(Arg.Any<Transaction>());
        await _uow.Received(0).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldUseDefaultCategory_WhenDefaultCategoryExists()
    {
        var userId = Guid.NewGuid();
        var defaultCategoryId = Guid.NewGuid();

        _categoryRepo.GetDefaultByUserIdAsync(userId).Returns(defaultCategoryId);

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            userId,
            null,
            20,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Essen gehen",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(defaultCategoryId, result.Value.CategoryId);
        Assert.Equal(CategorySource.Unmatched, result.Value.CategorySource);

        await _categoryRepo.Received(0).ExistsForUserAsync(Arg.Any<Guid>(), Arg.Any<Guid>());
        await _categoryRepo.Received(1).GetDefaultByUserIdAsync(userId);
        _transactionRepo.Received(1).Add(Arg.Any<Transaction>());
        await _uow.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldReturnDefaultCategoryNotFound_WhenDefaultCategoryDoesntExists()
    {
        var userId = Guid.NewGuid();
        _categoryRepo.GetDefaultByUserIdAsync(userId).Returns((Guid?)null);

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            userId,
            null,
            20,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Essen gehen",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(new DefaultCategoryNotFoundError(), result.Error);

        await _categoryRepo.Received(0).ExistsForUserAsync(Arg.Any<Guid>(), Arg.Any<Guid>());
        await _categoryRepo.Received(1).GetDefaultByUserIdAsync(userId);
        _transactionRepo.Received(0).Add(Arg.Any<Transaction>());
        await _uow.Received(0).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldReturnInvalidGuid_WhenUserIdEmpty()
    {
        // Setup
        _categoryRepo.GetDefaultByUserIdAsync(Arg.Any<Guid>()).Returns(Guid.NewGuid());

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            new Guid(),
            null,
            20,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Essen gehen",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(new InvalidGuidError(), result.Error);
    }

    [Fact]
    public async Task ShouldReturnInvalidTransactionAmount_WhenAmountNegative()
    {
        // Setup
        _categoryRepo.GetDefaultByUserIdAsync(Arg.Any<Guid>()).Returns(Guid.NewGuid());

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            Guid.NewGuid(),
            null,
            -20,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Essen gehen",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(new InvalidAmountError(), result.Error);
    }

    [Fact]
    public async Task ShouldReturnTransactionTitleEmpty_WhenTitleEmpty()
    {
        // Setup
        _categoryRepo.GetDefaultByUserIdAsync(Arg.Any<Guid>()).Returns(Guid.NewGuid());

        var handler = new CreateTransactionHandler(_uow, _transactionRepo, _categoryRepo);
        var command = new CreateTransactionCommand(
            Guid.NewGuid(),
            null,
            20,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "",
            "",
            "Pizza place"
        );

        // Execute
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(new EmptyTransactionTitleError(), result.Error);
    }
}