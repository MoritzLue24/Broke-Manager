using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.Transactions;

namespace Domain.Tests.Entities;

public class TransactionTests
{
    [Fact]
    public void Create_ShouldReturnTransactionAndAddTransactionCreatedEvent_WhenTransactionValid()
    {
        var result = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        );

        Assert.Equal(CategorySource.Manual, result.Value.CategorySource);
        Assert.Equal(TransactionType.Expense, result.Value.Type);
        Assert.Equal("Test title transaction", result.Value.Title);
        Assert.Equal("Test description Transaction", result.Value.Description);
        Assert.Equal("Test counterparty Transaction", result.Value.CounterParty);
        Assert.Contains(new TransactionCreatedEvent(result.Value.Id), result.Value.DomainEvents);
    }

    [Fact]
    public void Create_ShouldReturnInvalidAmountError_WhenAmountZero()
    {
        var result = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            0m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        );
        Assert.Equal(new InvalidAmountError(), result.FirstError);
    }

    [Fact]
    public void Create_ShouldReturnEmptyTransactionTitleError_WhenTitleEmpy()
    {
        var result = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "",
            "Test description Transaction",
            "Test counterparty Transaction"
        );

        Assert.Equal(new EmptyTransactionTitleError(), result.FirstError);
    }

    [Fact]
    public void Create_ShouldReturnInvalidGuidError_WhenUserIdEmpty()
    {
        var result = Transaction.Create(
            Guid.Empty,
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        );

        Assert.Equal(new InvalidGuidError(), result.FirstError);
    }

    [Fact]
    public void Create_ShouldReturnInvalidGuidError_WhenCategoryIdEmpty()
    {
        var result = Transaction.Create(
            Guid.NewGuid(),
            Guid.Empty,
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        );

        Assert.Equal(new InvalidGuidError(), result.FirstError);
    }

    [Fact]

    public void ChangeCategory_ShouldChangeCategoryIdAndSource_WhenCategoryIdValid()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;
        var categoryId = Guid.NewGuid();

        var result = transaction.ChangeCategory(categoryId, CategorySource.Manual);

        Assert.True(result.Success);
        Assert.Equal(categoryId, transaction.CategoryId);
    }

    [Fact]
    public void ChangeCategory_ShouldReturnInvalidGuidError_WhenCategoryIdEmpty()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeCategory(Guid.Empty, CategorySource.Manual);

        Assert.Equal(new InvalidGuidError(), result.FirstError);
    }

    [Fact]
    public void ChangeAmount_ShouldChangeAmount_WhenAmountGreaterZero()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeAmount(5000.0m, TransactionType.Expense);

        Assert.True(result.Success);
        Assert.Equal(5000.0m, transaction.Amount);
    }

    [Fact]
    public void ChangeAmount_ShouldReturnInvalidAmountError_WhenAmountNegative()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeAmount(-5000.0m, TransactionType.Expense);

        Assert.Equal(new InvalidAmountError(), result.FirstError);
    }

    [Fact]
    public void ChangeDate_ShouldChangeDate()
    {
        var newDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeDate(newDate);

        Assert.True(result.Success);
        Assert.Equal(newDate, transaction.Date);
    }

    [Fact]
    public void ChangeTitle_ShouldChangeTitle_WhenTitleNotEmpty()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeTitle("dildo gekauft");

        Assert.True(result.Success);
        Assert.Equal("dildo gekauft", transaction.Title);
    }

    [Fact]
    public void ChangeTitle_ShouldReturnEmptyTransactionTitleError_WhenTitleEpmty()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeTitle("");

        Assert.False(result.Success);
        Assert.Equal(new EmptyTransactionTitleError(), result.FirstError);
    }

    [Fact]
    public void ChangeDescription_ShouldChangeDescription()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeDescription("Hallo");

        Assert.True(result.Success);
        Assert.Equal("Hallo", transaction.Description);
    }

    [Fact]
    public void ChangeCounterParty_ShouldChangeCounterParty()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.ChangeCounterParty("Hallo");

        Assert.True(result.Success);
        Assert.Equal("Hallo", transaction.CounterParty);
    }

    [Fact]
    public void Delete_ShouldAddTransactionDeletedEvent()
    {
        var transaction = Transaction.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CategorySource.Manual,
            100.1m,
            TransactionType.Expense,
            DateOnly.FromDateTime(DateTime.Now),
            "Test title transaction",
            "Test description Transaction",
            "Test counterparty Transaction"
        ).Value;

        var result = transaction.Delete();

        Assert.True(result.Success);
        Assert.Equal(
            [new TransactionCreatedEvent(transaction.Id), new TransactionDeletedEvent()],
            transaction.DomainEvents
        );
    }

    /*TODO
    Create when Amount is negative
    [ИмяТестируемогоМетода]_[СценарийУсловия]_[ОжидаемыйРезультат] поменять названия всех тестов вот так
    Посмотреть что за Theory and Incline Data
    
    
    */
}

