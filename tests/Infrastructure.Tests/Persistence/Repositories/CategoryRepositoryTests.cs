using Domain.Entities;
using Domain.ValueObjects;

using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Tests.Persistence.Repositories;

public class CategoryReaderRepositoryTests : IClassFixture<PostgresFixture>
{
    private readonly DatabaseManager _db;
    private readonly CategoryRepository _repo;

    public CategoryReaderRepositoryTests(PostgresFixture postgres)
    {
        this._db = new DatabaseManager(postgres.ConnectionString);
        this._repo = new CategoryRepository(this._db.Context);
    }

    /// GetDefaultByUserIdAsync :)
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnId_WhenExists()
    {
        // Setup
        await this._db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var defaultCategory = Category.Create(
            user.Id,
            "Default",
            true
        ).Value;
        this._db.Context.Users.Add(user);
        this._db.Context.Categories.Add(defaultCategory);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(id);
        Assert.Equal(defaultCategory.Id, id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenOnlyNormalCategoryExists()
    {
        // Setup
        await this._db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        this._db.Context.Users.Add(user);
        this._db.Context.Categories.Add(category);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.Null(id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnCorrectId_WhenMultipleCategoryExists()
    {
        // Setup
        await this._db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        var defaultCategory = Category.Create(
            user.Id,
            "Default",
            true
        ).Value;
        this._db.Context.Users.Add(user);
        this._db.Context.Categories.Add(category);
        this._db.Context.Categories.Add(defaultCategory);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(id);
        Assert.Equal(defaultCategory.Id, id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenNoCategoryExists()
    {
        // Setup
        await this._db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        this._db.Context.Users.Add(user);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.Null(id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenCategoryNotOwned()
    {
        // Setup
        await this._db.ResetAsync();
        var userA = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var userB = User.Create(
            Email.Create("ema23il@mail.de").Value,
            Hash.Create("pi1n212331j23pojk1").Value
        ).Value;
        var defaultCategory = Category.Create(
            userB.Id,
            "Default",
            true
        ).Value;
        this._db.Context.Users.Add(userA);
        this._db.Context.Users.Add(userB);
        this._db.Context.Categories.Add(defaultCategory);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(userA.Id);

        // Assert
        Assert.Null(id);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnTrue_WhenCategoryExists()
    {
        // Setup
        await this._db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        this._db.Context.Users.Add(user);
        this._db.Context.Categories.Add(category);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var exists = await this._repo.ExistsForUserAsync(user.Id, category.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnFalse_WhenCategoryNotOwned()
    {
        // Setup
        await this._db.ResetAsync();
        var userA = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var userB = User.Create(
            Email.Create("ema2il@mail.de").Value,
            Hash.Create("piasd1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            userB.Id,
            "Essen",
            false
        ).Value;
        this._db.Context.Users.Add(userA);
        this._db.Context.Users.Add(userB);
        this._db.Context.Categories.Add(category);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var exists = await this._repo.ExistsForUserAsync(userA.Id, category.Id);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnFalse_WhenCategoryDoesntExists()
    {
        // Setup
        await this._db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        this._db.Context.Users.Add(user);
        await this._db.Context.SaveChangesAsync();

        // Execute
        var exists = await this._repo.ExistsForUserAsync(user.Id, category.Id);

        // Assert
        Assert.False(exists);
    }
}
