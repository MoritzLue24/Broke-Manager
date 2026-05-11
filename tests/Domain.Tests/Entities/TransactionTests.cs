using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;

namespace Domain.Tests.Entities;

public class TransactionTests
{
    
    private static Transaction CreateValidTransaction()
    {
        return Transaction.Create(
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
    }

    [Fact]
    public void Create_ShouldSucceed_When_TransactionValid()
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

        Assert.True(result.Success);
        Assert.Equal(CategorySource.Manual, result.Value.CategorySource);
        Assert.Equal(TransactionType.Expense, result.Value.Type);
        Assert.Equal("Test title transaction", result.Value.Title);
        Assert.Equal("Test description Transaction", result.Value.Description);
        Assert.Equal("Test counterparty Transaction", result.Value.CounterParty);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });
    }

    [Fact]
    public void Should_Fail_When_TitleEmpy()
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

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.TransactionTitleEmpty, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void Should_Fail_When_UserId_Empty()
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

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.InvalidGuid, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }
    
    [Fact]
    public void Should_Fail_When_CategoryId_Empty()
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

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.InvalidGuid, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]

    public void Should_Succeed_CategoryChange()
    {
        var transaction = CreateValidTransaction();
        var id = Guid.NewGuid();
        var result = transaction.ChangeCategory(id, CategorySource.Manual);
        
        Assert.True(result.Success);
        Assert.Equal(id, transaction.CategoryId);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });

    }

    [Fact]
    public void Should_Fail_CategoryChange_When_CategoryEmpty()
    {
        var transaction = CreateValidTransaction();
        var result = transaction.ChangeCategory(Guid.Empty, CategorySource.Manual);
        
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.InvalidGuid, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void Should_Succeed_ChangeAmount()
    {
        var transaction = CreateValidTransaction();
        var result = transaction.ChangeAmount(5000.0m, TransactionType.Expense);

        Assert.True(result.Success);
        Assert.Equal(5000.0m, transaction.Amount);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });

    }

    [Fact]
    public void Should_Fail_ChangeAmount_When_AmountIsNegative()
    {
        var transaction = CreateValidTransaction();
        var result = transaction.ChangeAmount(-5000.0m, TransactionType.Expense);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.InvalidAmount, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }

    [Fact]
    public void Should_Succeed_ChangeTitle()
    {
        var transaction = CreateValidTransaction();
        var result = transaction.ChangeTitle("dildo gekauft");

        Assert.True(result.Success);
        Assert.Equal("dildo gekauft", transaction.Title);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Error; });
    }

    [Fact]
    public void Should_Fail_ChangeTitle_When_TitleEpmty()
    {
        var transaction = CreateValidTransaction();
        var result = transaction.ChangeTitle("");

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCode.TransactionTitleEmpty, result.Error);
        Assert.Throws<InvalidOperationException>(() => { var _ = result.Value; });
    }
    
    /*TODO
    Create When Amount is zero
    Create when Amount is negative
    [ИмяТестируемогоМетода]_[СценарийУсловия]_[ОжидаемыйРезультат] поменять названия всех тестов вот так
    Посмотреть что за Theory and Incline Data
    
    
    */
}

