using Domain.Entities;
using Domain.Enums;
using Domain.Events.Users;
using Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_ShouldReturnUserAndAddUserCreatedEvent_WhenUserValid()
    {
        // Execute
        var domainResult = User.Create(
            Email.Create("email@asd.de").Value,
            Hash.Create("pohdqpowjdq").Value
        );

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(Email.Create("email@asd.de").Value, domainResult.Value.Email);
        Assert.Contains(new UserCreatedEvent(domainResult.Value.Id, domainResult.Value.Email), domainResult.Value.DomainEvents);
    }

    [Fact]
    public void ChangeEmail_ShouldChangeEmailAndAddEmailChangedEvent_WhenCorrect()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@asd.de").Value,
            Hash.Create("pohdqpowjdq").Value
        ).Value;

        // Execute
        var domainResult = user.ChangeEmail(Email.Create("email2@asd.de").Value);

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(Email.Create("email2@asd.de").Value, user.Email);
        Assert.Contains(
            new EmailChangedEvent(
                user.Id,
                Email.Create("email2@asd.de").Value,
                Email.Create("email@asd.de").Value
            ),
            user.DomainEvents
        );
    }

    [Fact]
    public void ChangePasswordHash_ShouldChangePasswordHashAndAddPasswordChangedEvent_WhenCorrect()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@asd.de").Value,
            Hash.Create("pohdqpowjdq").Value
        ).Value;

        // Execute
        var domainResult = user.ChangePasswordHash(Hash.Create("12pi3hnd1d2dpon1").Value);

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(Hash.Create("12pi3hnd1d2dpon1").Value, user.PasswordHash);
        Assert.Contains(
            new PasswordChangedEvent(
                user.Id,
                Email.Create("email@asd.de").Value
            ),
            user.DomainEvents
        );
    }

    [Fact]
    public void ChangeRole_ShouldChangeRoleAndAddRoleChangedEvent_WhenRoleValid()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@asd.de").Value,
            Hash.Create("pohdqpowjdq").Value
        ).Value;

        // Execute
        var domainResult = user.ChangeRole(Role.Admin);

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal(Role.Admin, user.Role);
        Assert.Contains(
            new RoleChangedEvent(
                user.Id,
                Email.Create("email@asd.de").Value,
                Role.Admin,
                Role.User
            ),
            user.DomainEvents
        );
    }

    [Fact]
    public void Delete_ShouldSucceedAndAddDomainEvent()
    {
        // Setup
        var user = User.Create(
            Email.Create("email@asd.de").Value,
            Hash.Create("pohdqpowjdq").Value
        ).Value;

        // Execute
        var domainResult = user.Delete();

        // Assert
        Assert.True(domainResult.Success);
        Assert.Contains(
            new UserDeletedEvent(Email.Create("email@asd.de").Value),
            user.DomainEvents
        );
    }
}
