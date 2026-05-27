using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.IntegrationTests.TestInfrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.IntegrationTests.Persistence.Repositories;

public class CategoryReaderRepositoryTests : BaseTest
{
    private readonly CategoryRepository _repo;

    public CategoryReaderRepositoryTests(PostgresFixture postgres)
        : base(postgres)
    {
        this._repo = new CategoryRepository(this._dbContext);
    }

    [Fact]
    public async Task GetById_ShouldReturnKeywords_WhenKeywordsExists()
    {
        var user = User.Create(
            Email.Create("email@mail.de").Value,
            Hash.Create("pi1n231j23pojk1").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "Essen",
            false
        ).Value;
        category.AddRule(MatchingRule.Create("Edeka").Value);
        category.AddRule(MatchingRule.Create("Rewe").Value);

        this._dbContext.Users.Add(user);
        this._dbContext.Categories.Add(category);
        await this._dbContext.SaveChangesAsync();

        // Execute
        var result = await this._repo.GetByIdAsync(category.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Essen", result.Name);
        Assert.Equal(
            [MatchingRule.Create("Edeka").Value, MatchingRule.Create("Rewe").Value],
            result.MatchingRules
        );
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenNotExists()
    {

        var result = await this._repo.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    /// GetDefaultByUserIdAsync :)
    [Fact]
    public async Task GetDefaultIdByUserIdAsync_ShouldReturnId_WhenExists()
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
        var result = await this._repo.GetDefaultIdByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(defaultCategory.Id, result);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultIdByUserIdAsync_ShouldReturnNull_WhenOnlyNormalCategoryExists()
    {
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
        var result = await this._repo.GetDefaultIdByUserIdAsync(user.Id);

        // Assert
        Assert.Null(result);
    }

    /// GetDefaultByUserIdAsync :(
    [Fact]
    public async Task GetDefaultIdByUserIdAsync_ShouldReturnNull_WhenCategoryNotOwned()
    {
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
        var id = await this._repo.GetDefaultIdByUserIdAsync(userA.Id);

        // Assert
        Assert.Null(id);
    }

    [Fact]
    public async Task ExistsForUser_ShouldReturnTrue_WhenCategoryExists()
    {
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
}
