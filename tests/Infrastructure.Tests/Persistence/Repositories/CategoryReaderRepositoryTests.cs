using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Tests.Persistence.Repositories;

public class CategoryReaderRepositoryTests : IClassFixture<PostgresFixture>
{
    private readonly DatabaseManager _db;
    private readonly CategoryReaderRepository _repo;

    public CategoryReaderRepositoryTests(PostgresFixture postgres)
    {
        _db = new DatabaseManager(postgres.ConnectionString);
        _repo = new CategoryReaderRepository(_db.Context);
    }

    /// GetDefaultByUserIdAsync :)
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnId_WhenExists()
    {
        // Setup
        await _db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var defaultCategory = Category.Create(
            user.Id,
            "Default",
            true
        ).Value;
        _db.Context.Users.Add(user);
        _db.Context.Categories.Add(defaultCategory);
        await _db.Context.SaveChangesAsync();

        // Execute
        var id = await _repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(id);
        Assert.Equal(defaultCategory.Id, id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenOnlyNormalCategoryExists()
    {
        // Setup
        await _db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        _db.Context.Users.Add(user);
        _db.Context.Categories.Add(category);
        await _db.Context.SaveChangesAsync();

        // Execute
        var id = await _repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.Null(id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnCorrectId_WhenMultipleCategoryExists()
    {
        // Setup
        await _db.ResetAsync();
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
        _db.Context.Users.Add(user);
        _db.Context.Categories.Add(category);
        _db.Context.Categories.Add(defaultCategory);
        await _db.Context.SaveChangesAsync();

        // Execute
        var id = await _repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(id);
        Assert.Equal(defaultCategory.Id, id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenNoCategoryExists()
    {
        // Setup
        await _db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        _db.Context.Users.Add(user);
        await _db.Context.SaveChangesAsync();

        // Execute
        var id = await _repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.Null(id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenCategoryNotOwned()
    {
        // Setup
        await _db.ResetAsync();
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
        _db.Context.Users.Add(userA);
        _db.Context.Users.Add(userB);
        _db.Context.Categories.Add(defaultCategory);
        await _db.Context.SaveChangesAsync();

        // Execute
        var id = await _repo.GetDefaultByUserIdAsync(userA.Id);

        // Assert
        Assert.Null(id);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnTrue_WhenCategoryExists()
    {
        // Setup
        await _db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        _db.Context.Users.Add(user);
        _db.Context.Categories.Add(category);
        await _db.Context.SaveChangesAsync();

        // Execute
        var exists = await _repo.ExistsForUser(user.Id, category.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnFalse_WhenCategoryNotOwned()
    {
        // Setup
        await _db.ResetAsync();
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
        _db.Context.Users.Add(userA);
        _db.Context.Users.Add(userB);
        _db.Context.Categories.Add(category);
        await _db.Context.SaveChangesAsync();

        // Execute
        var exists = await _repo.ExistsForUser(userA.Id, category.Id);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnFalse_WhenCategoryDoesntExists()
    {
        // Setup
        await _db.ResetAsync();
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        _db.Context.Users.Add(user);
        await _db.Context.SaveChangesAsync();

        // Execute
        var exists = await _repo.ExistsForUser(user.Id, category.Id);

        // Assert
        Assert.False(exists);
    }
}