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
    public void Create_ShouldSucced_When_TransactionValid()
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

    public void Should_Succed_CategoryChange()
    {
        var transaction = CreateValidTransaction();
        var id = Guid.NewGuid();
        var result = transaction.ChangeCategory(id, CategorySource.Manual);
        
        Assert.True(result.Success);
        Assert.Equal(id, transaction.Id);
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
    
}

