using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

using Infrastructure.Persistence.Repositories;
using Infrastructure.Tests.TestInfrastructure.Persistence;

namespace Infrastructure.Tests.Persistence.Repositories;

public class TransactionRepositoryTests : BaseTest
{
    private readonly TransactionRepository _repo;

    public TransactionRepositoryTests(PostgresFixture postgres)
        : base(postgres)
    {
        this._repo = new TransactionRepository(this._dbContext);
    }

    /// GetDefaultByUserIdAsync :)
    [Fact]
    public async Task Add_ShouldAddCategory_WhenContextValid()
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

        // Assert
        // TODO
    }
}
