using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_ShouldReturnEmail_WhenFormatCorrect()
    {
        var domainResult = Email.Create("very-valid@email.com");
        Assert.Equal("very-valid@email.com", domainResult.Value.Value);
    }

    [Theory]
    [InlineData("not-valid@")]
    [InlineData("@asd.com")]
    [InlineData("@")]
    [InlineData("")]
    [InlineData("not-valid@@asd.com")]
    public void Create_ShouldReturnInvalidEmailFormat_WhenFormatIncorrect(string input)
    {
        var domainResult = Email.Create(input);
        Assert.Equal(new InvalidEmailFormatError(), domainResult.FirstError);
    }
}
