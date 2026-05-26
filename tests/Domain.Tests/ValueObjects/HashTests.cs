using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class HashTests
{
    [Fact]
    public void Create_ShouldReturnHash_WhenNotEmpty()
    {
        var domainResult = Hash.Create("pqiobawdh0812bnip102ibd");
        Assert.Equal("pqiobawdh0812bnip102ibd", domainResult.Value.Value);
    }

    [Fact]
    public void Create_ShouldReturnInvalidHashFormat_WhenEmpty()
    {
        // Execute
        var domainResult = Hash.Create("");
        Assert.Equal(new InvalidHashFormatError(), domainResult.FirstError);
    }
}
