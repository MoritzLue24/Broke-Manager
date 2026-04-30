using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class HashTests
{
    [Fact]
    public void Create_ShouldReturnHash_WhenNotEmpty()
    {
        // Execute
        var domainResult = Hash.Create("pqiobawdh0812bnip102ibd");

        // Assert
        Assert.True(domainResult.Success);
        Assert.Equal("pqiobawdh0812bnip102ibd", domainResult.Value.Value);
        Assert.Throws<InvalidOperationException>(() => {var _ = domainResult.Error;});
    }

    [Fact]
    public void Create_ShouldReturnInvalidHashFormat_WhenEmpty()
    {
        // Execute
        var domainResult = Hash.Create("");

        // Assert
        Assert.False(domainResult.Success);
        Assert.Equal(DomainErrorCode.InvaildHashFormat, domainResult.Error);
        Assert.Throws<InvalidOperationException>(() => domainResult.Value);
    }
}