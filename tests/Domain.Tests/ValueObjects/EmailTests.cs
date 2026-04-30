using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_ShouldReturnEmail_WhenFormatCorrect()
    {
        // Execute
        var domainResult = Email.Create("very-valid@email.com");

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal("very-valid@email.com", domainResult.Value.Value);
        Assert.Throws<InvalidOperationException>(() => {var _ = domainResult.Error;});
    }

    [Theory]
    [InlineData("not-valid@")]
    [InlineData("@asd.com")]
    [InlineData("@")]
    [InlineData("")]
    [InlineData("not-valid@@asd.com")]
    public void Create_ShouldReturnInvalidEmailFormat_WhenFormatIncorrect(string input)
    {
        // Execute
        var domainResult = Email.Create(input);

        // Assert
        Assert.False(domainResult.Success);
        Assert.Equal(DomainErrorCode.InvalidEmailFormat, domainResult.Error);
        Assert.Throws<InvalidOperationException>(() => domainResult.Value);
    }
}