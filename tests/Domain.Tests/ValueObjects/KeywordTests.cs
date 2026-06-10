using Domain.Common;
using Domain.Entities;

namespace Domain.Tests.ValueObjects;

public class KeywordTests
{
    [Fact]
    public void Create_ShouldReturnKeyword_WhenNotEmpty()
    {

        var result = MatchingRule.Create("penis");
        Assert.Equal("penis", result.Value.Keyword);
    }

    [Fact]
    public void Create_ShouldReturnKeywordEmpty_WhenEmpty()
    {

        var result = MatchingRule.Create("");
        Assert.Equal(new EmptyKeywordError(), result.FirstError);
    }
}
