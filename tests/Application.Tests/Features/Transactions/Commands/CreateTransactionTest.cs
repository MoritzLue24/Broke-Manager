using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Commands.CreateTransaction;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using NSubstitute;

namespace Application.Tests.Features.Transactions.Commands;

public class CreateTransactionTests
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public CreateTransactionTests()
    {
        // Mock interfaces: in each test, set what interface methods should return with what parameters
        this._userContext = Substitute.For<IUserContext>();
        this._uow = Substitute.For<IUnitOfWork>();
        this._transactionRepo = Substitute.For<ITransactionRepository>();
        this._categoryRepo = Substitute.For<ICategoryRepository>();
    }

    [Fact]
    public async Task ShouldCreateTransaction_WhenManualCategoryExists()
    {
        // Setup
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        this._userContext.UserId.Returns(userId);
        this._userContext.UserRoles.Returns([Role.User]);
        this._categoryRepo.ExistsForUserAsync(userId, categoryId).Returns(true);

        var handler = new CreateTransactionCommandHandler(
            this._userContext,
            this._uow,
            this._transactionRepo,
            this._categoryRepo
        );
        var command = new CreateTransactionCommand(
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

        await this._categoryRepo.Received(1).ExistsForUserAsync(userId, categoryId);
        await this._categoryRepo.Received(0).GetDefaultByUserIdAsync(Arg.Any<Guid>());
        this._transactionRepo.Received(1).Add(Arg.Any<Transaction>());
        await this._uow.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldReturnCategoryNotFound_WhenManualCategoryDoesntExist()
    {
        // Setup
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        this._userContext.UserId.Returns(userId);
        this._userContext.UserRoles.Returns([Role.User]);
        this._categoryRepo.ExistsForUserAsync(userId, categoryId).Returns(false);

        var handler = new CreateTransactionCommandHandler(
            this._userContext,
            this._uow,
            this._transactionRepo,
            this._categoryRepo
        );
        var command = new CreateTransactionCommand(
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
        Assert.Equal(new CategoryNotFoundError(), result.FirstError);

        await this._categoryRepo.Received(1).ExistsForUserAsync(userId, categoryId);
        await this._categoryRepo.Received(0).GetDefaultByUserIdAsync(Arg.Any<Guid>());
        this._transactionRepo.Received(0).Add(Arg.Any<Transaction>());
        await this._uow.Received(0).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldUseDefaultCategory_WhenDefaultCategoryExists()
    {
        var userId = Guid.NewGuid();
        var defaultCategoryId = Guid.NewGuid();

        this._userContext.UserId.Returns(userId);
        this._userContext.UserRoles.Returns([Role.User]);
        this._categoryRepo.GetDefaultByUserIdAsync(userId).Returns(defaultCategoryId);

        var handler = new CreateTransactionCommandHandler(
            this._userContext,
            this._uow,
            this._transactionRepo,
            this._categoryRepo
        );
        var command = new CreateTransactionCommand(
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

        await this._categoryRepo.Received(0).ExistsForUserAsync(Arg.Any<Guid>(), Arg.Any<Guid>());
        await this._categoryRepo.Received(1).GetDefaultByUserIdAsync(userId);
        this._transactionRepo.Received(1).Add(Arg.Any<Transaction>());
        await this._uow.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldReturnDefaultCategoryNotFound_WhenDefaultCategoryDoesntExists()
    {
        var userId = Guid.NewGuid();
        this._userContext.UserId.Returns(userId);
        this._userContext.UserRoles.Returns([Role.User]);
        this._categoryRepo.GetDefaultByUserIdAsync(userId).Returns((Guid?)null);

        var handler = new CreateTransactionCommandHandler(
            this._userContext,
            this._uow,
            this._transactionRepo,
            this._categoryRepo
        );
        var command = new CreateTransactionCommand(
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
        Assert.Equal(new DefaultCategoryNotFoundError(), result.FirstError);

        await this._categoryRepo.Received(0).ExistsForUserAsync(Arg.Any<Guid>(), Arg.Any<Guid>());
        await this._categoryRepo.Received(1).GetDefaultByUserIdAsync(userId);
        this._transactionRepo.Received(0).Add(Arg.Any<Transaction>());
        await this._uow.Received(0).SaveChangesAsync(default);
    }

    [Fact]
    public async Task ShouldReturnInvalidTransactionAmount_WhenAmountNegative()
    {
        // Setup
        Guid userId = Guid.NewGuid();
        this._userContext.UserId.Returns(userId);
        this._userContext.UserRoles.Returns([Role.User]);
        this._categoryRepo.GetDefaultByUserIdAsync(Arg.Any<Guid>()).Returns(Guid.NewGuid());

        var handler = new CreateTransactionCommandHandler(
            this._userContext,
            this._uow,
            this._transactionRepo,
            this._categoryRepo
        );
        var command = new CreateTransactionCommand(
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
        Assert.Equal(new InvalidAmountError(), result.FirstError);
    }

    [Fact]
    public async Task ShouldReturnTransactionTitleEmpty_WhenTitleEmpty()
    {
        // Setup
        var userId = Guid.NewGuid();
        this._userContext.UserId.Returns(userId);
        this._userContext.UserRoles.Returns([Role.User]);
        this._categoryRepo.GetDefaultByUserIdAsync(Arg.Any<Guid>()).Returns(Guid.NewGuid());

        var handler = new CreateTransactionCommandHandler(
            this._userContext,
            this._uow,
            this._transactionRepo,
            this._categoryRepo
        );
        var command = new CreateTransactionCommand(
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
        Assert.Equal(new EmptyTransactionTitleError(), result.FirstError);
    }
}
