using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.IntegrationTests.TestInfrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.Persistence.Repositories;

public class UserRepositoryTests : BaseTest
{
    private readonly UserRepository _repo;

    public UserRepositoryTests(PostgresFixture postgres)
        : base(postgres)
    {
        this._repo = new UserRepository(this._dbContext);
    }

    [Fact]
    public async Task EmailExists_ShouldReturnTrue_WhenExists()
    {
        var user = User.Create(
            Email.Create("em@a.mail").Value,
            Hash.Create("apodwqpojdq").Value
        ).Value;
        this._dbContext.Users.Add(user);
        await this._dbContext.SaveChangesAsync();

        var result = await this._repo.EmailExistsAsync("em@a.mail");
        Assert.True(result);
    }

    [Fact]
    public async Task EmailExists_ShouldReturnFalse_WhenDoesntExists()
    {
        var result = await this._repo.EmailExistsAsync("pajowd@pioqwn.de");
        Assert.False(result);
    }

    [Fact]
    public async Task Add_ShouldAdd()
    {
        var user = User.Create(
            Email.Create("em@a.mail").Value,
            Hash.Create("apodwqpojdq").Value
        ).Value;
        this._repo.Add(user);
        await this._dbContext.SaveChangesAsync();

        Assert.True(await this._dbContext.Users.AnyAsync());
    }
}