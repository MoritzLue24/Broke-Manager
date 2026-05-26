using Domain.Common;

namespace Domain.Tests.Common;

public class UnitTests
{
    [Fact]
    public void Value_ShouldReturnTheSameUnit()
    {
        // Setup
        Unit a = Unit.Value;
        Unit b = Unit.Value;

        // Assert
        Assert.Equal(a, b);
    }
}
