using Domain.Entities;
using Domain.ValueObjects;

using Infrastructure.Persistence.Repositories;
using Infrastructure.Tests.Persistence.Common;

namespace Infrastructure.Tests.Persistence.Repositories;

public class CategoryReaderRepositoryTests : BaseTest
{
    private readonly CategoryRepository _repo;

    public CategoryReaderRepositoryTests(PostgresFixture postgres)
        : base(postgres)
    {
        this._repo = new CategoryRepository(this._dbContext);
    }

    /// GetDefaultByUserIdAsync :)
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnId_WhenExists()
    {
        Console.WriteLine("GetDefaultByUserIdAsync_ShouldReturnId_WhenExists");

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
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(id);
        Assert.Equal(defaultCategory.Id, id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenOnlyNormalCategoryExists()
    {
        Console.WriteLine("GetDefaultByUserIdAsync_ShouldReturnNull_WhenOnlyNormalCategoryExists");

        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(category);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.Null(id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnCorrectId_WhenMultipleCategoryExists()
    {
        Console.WriteLine("GetDefaultByUserIdAsync_ShouldReturnCorrectId_WhenMultipleCategoryExists");

        // Setup
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
        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(category);
        this._dbContext.Categories.Add(defaultCategory);
        await this._dbContext.SaveChangesAsync();

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
        Console.WriteLine("GetDefaultByUserIdAsync_ShouldReturnNull_WhenNoCategoryExists");

        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        this._dbContext.Users.Add(user);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(user.Id);

        // Assert
        Assert.Null(id);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultByUserIdAsync_ShouldReturnNull_WhenCategoryNotOwned()
    {
        Console.WriteLine("GetDefaultByUserIdAsync_ShouldReturnNull_WhenCategoryNotOwned");

        // Setup
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
        this._dbContext.Users.Add(userA);
        this._dbContext.Users.Add(userB);
        this._dbContext.Categories.Add(defaultCategory);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var id = await this._repo.GetDefaultByUserIdAsync(userA.Id);

        // Assert
        Assert.Null(id);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnTrue_WhenCategoryExists()
    {
        Console.WriteLine("ExistsForUser_ShouldReturnTrue_WhenCategoryExists");

        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(category);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var exists = await this._repo.ExistsForUserAsync(user.Id, category.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnFalse_WhenCategoryNotOwned()
    {
        Console.WriteLine("ExistsForUser_ShouldReturnFalse_WhenCategoryNotOwned");

        // Setup
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
        this._dbContext.Users.Add(userA);
        this._dbContext.Users.Add(userB);
        this._dbContext.Categories.Add(category);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var exists = await this._repo.ExistsForUserAsync(userA.Id, category.Id);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnFalse_WhenCategoryDoesntExists()
    {
        Console.WriteLine("ExistsForUser_ShouldReturnFalse_WhenCategoryDoesntExists");

        // Setup
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        this._dbContext.Users.Add(user);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var exists = await this._repo.ExistsForUserAsync(user.Id, category.Id);

        // Assert
        Assert.False(exists);
    }
}
