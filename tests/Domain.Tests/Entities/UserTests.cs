using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;

namespace Domain.Tests.Entities;

public class UserTests
{
    private static DomainResult<User> CreateValidUserResult()
        => User.Create(
            CreateValidEmail(),
            CreateValidHash()
        );

    private static User CreateValidUser()
        => User.Create(
            CreateValidEmail(),
            CreateValidHash()
        ).Value;

    private static Email CreateValidEmail()
        => Email.Create("valid@email.com").Value;

    private static Hash CreateValidHash()
        => Hash.Create("apsnid").Value;

    [Fact]
    public void Create_ShouldReturnUser()
    {
        // Execute
        var domainResult = CreateValidUserResult();

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(CreateValidEmail(), domainResult.Value.Email);
        Assert.Throws<InvalidOperationException>(() => { var _ = domainResult.Error; });
    }

    [Fact]
    public void ChangeEmail_ShouldChangeEmail_WhenCorrect()
    {
        // Execute
        var user = CreateValidUser();
        var domainResult = user.ChangeEmail(CreateValidEmail());

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(CreateValidEmail(), user.Email);
        Assert.Throws<InvalidOperationException>(() => { var _ = domainResult.Error; });
    }

    [Fact]
    public void ChangePasswordHash_ShouldChangePasswordHash_WhenCorrect()
    {
        // Execute
        var user = CreateValidUser();
        var domainResult = user.ChangePasswordHash(CreateValidHash());

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(CreateValidHash(), user.PasswordHash);
        Assert.Throws<InvalidOperationException>(() => { var _ = domainResult.Error; });
    }

    [Fact]
    public void ChangeRole_ShouldChangeRole()
    {
        // Execute
        var user = CreateValidUser();
        var domainResult = user.ChangeRole(Role.Admin);

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(Role.Admin, user.Role);
        Assert.Throws<InvalidOperationException>(() => { var _ = domainResult.Error; });
    }
}