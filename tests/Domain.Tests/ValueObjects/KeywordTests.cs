using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class KeywordTests
{
    [Fact]
    public void Create_ShouldReturnKeyword_WhenNotEmpty()
    {
        
        var result= Keyword.Create("penis");

        Assert.True(result.Success);
        Assert.Equal("penis", result.Value.Value);
        Assert.Throws<InvalidOperationException>(() => {var _ = result.Error;});
    }

    [Fact]
    public void Create_ShouldReturnKeywordEmpty_WhenEmpty()
    {
        
        var result = Keyword.Create("");

        Assert.False(result.Success);
        Assert.Equal(new EmptyKeywordError(), result.Error);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}