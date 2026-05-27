using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.IntegrationTests.TestInfrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.IntegrationTests.Persistence.Repositories;

public class TransactionRepositoryTests : BaseTest
{
    private readonly TransactionRepository _repo;

    public TransactionRepositoryTests(PostgresFixture postgres)
        : base(postgres)
    {
        this._repo = new TransactionRepository(this._dbContext);
    }

    [Fact]
    public async Task GetAllByUserId_ShouldReturnAll_WhenMultiple()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var defaultCategory = Category.Create(
            user.Id,
            "Default",
            true
        ).Value;
        var transactionA = Transaction.Create(
            user.Id,
            defaultCategory.Id,
            CategorySource.Manual,
            20,
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Sometitle",
            "somedesc",
            "somecounter"
        ).Value;
        var transactionB = Transaction.Create(
            user.Id,
            defaultCategory.Id,
            CategorySource.Manual,
            20,
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Sometitle",
            "somedesc",
            "somecounter"
        ).Value;
        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(defaultCategory);
        this._dbContext.Transactions.Add(transactionA);
        this._dbContext.Transactions.Add(transactionB);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var result = await this._repo.GetAllByUserIdAsync(user.Id);

        // Assert
        Assert.Contains(transactionA, result);
        Assert.Contains(transactionB, result);
    }

    [Fact]
    public async Task GetAllByUserId_ShouldReturnEmpty_WhenNone()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;

        // Execute
        var result = await this._repo.GetAllByUserIdAsync(user.Id);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnId_WhenExists()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var defaultCategory = Category.Create(
            user.Id,
            "Default",
            true
        ).Value;
        var transaction = Transaction.Create(
            user.Id,
            defaultCategory.Id,
            CategorySource.Manual,
            20,
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Sometitle",
            "somedesc",
            "somecounter"
        ).Value;
        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(defaultCategory);
        this._dbContext.Transactions.Add(transaction);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var result = await this._repo.GetByIdAsync(transaction.Id);

        // Assert
        Assert.Equal(transaction.Id, result?.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenNotExists()
    {
        // Execute
        var result = await this._repo.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    /// ADD :)
    [Fact]
    public async Task Add_ShouldAdd()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var defaultCategory = Category.Create(
            user.Id,
            "Default",
            true
        ).Value;
        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(defaultCategory);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var transaction = Transaction.Create(
            user.Id,
            defaultCategory.Id,
            CategorySource.Manual,
            20,
            TransactionType.Income,
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Sometitle",
            "somedesc",
            "somecounter"
        );
        this._repo.Add(transaction.Value);
        await this._dbContext.SaveChangesAsync();

        // Assert
        Assert.True(this._dbContext.Transactions.Any());
    }
}
