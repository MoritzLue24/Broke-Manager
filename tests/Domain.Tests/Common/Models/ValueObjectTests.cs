using Domain.Common.Models;

namespace Domain.Tests.Common.Models;

public class ValueObjectTests
{
    private class ValueObjectA(int a, int b) : ValueObject
    {
        public int A = a, B = b;
        protected override IEnumerable<object?> GetEqualityComponents()
            => [this.A, this.B];
    }
    private class ValueObjectB(int a, int b) : ValueObject
    {
        public int A = a, B = b;
        protected override IEnumerable<object?> GetEqualityComponents()
            => [this.A, this.B];
    }
    private class NoValueObject(int a, int b)
    {
        public int A = a, B = b;
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenOtherIsValueObjectAndPropertiesEquals()
    {
        // Setup
        ValueObjectA valueObjectA = new(1, 2);
        ValueObjectB valueObjectB = new(1, 2);

        // Execute & Assert
        Assert.True(valueObjectA.Equals(valueObjectB));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsValueObjectButPropertiesDifferentOrder()
    {
        // Setup
        ValueObjectA valueObjectA = new(2, 1);
        ValueObjectB valueObjectB = new(1, 2);

        // Execute & Assert
        Assert.False(valueObjectA.Equals(valueObjectB));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsNotValueObject()
    {
        // Setup
        ValueObjectA valueObjectA = new(1, 2);
        NoValueObject noValueObject = new(1, 2);

        // Execute & Assert
        Assert.False(valueObjectA.Equals(noValueObject));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsNull()
    {
        // Setup
        ValueObjectA valueObjectA = new(1, 2);

        // Execute & Assert
        Assert.False(valueObjectA.Equals(null));
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrue_WhenBothNotNullAndPropertiesEquals()
    {
        // Setup
        ValueObjectA valueObjectA = new(1, 2);
        ValueObjectB valueObjectB = new(1, 2);

        // Execute & Assert
        Assert.True(valueObjectA == valueObjectB);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnFalse_WhenBothNotNullButPropertiesDifferentOrder()
    {
        // Setup
        ValueObjectA valueObjectA = new(1, 2);
        ValueObjectB valueObjectB = new(2, 1);

        // Execute & Assert
        Assert.False(valueObjectA == valueObjectB);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnFalse_WhenRhsNull()
    {
        // Setup
        ValueObjectA valueObjectA = new(1, 2);

        // Execute & Assert
        Assert.False(valueObjectA == null);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrue_WhenBothNull()
    {
        // Setup
        ValueObjectA? valueObjectA = null;

        // Execute & Assert
        Assert.True(valueObjectA == null);
    }

    [Fact]
    public void GetHashCode_ShouldReturnCombinedHashCode()
    {
        // Setup
        var a = 1;
        var b = 2;
        ValueObjectA valueObjectA = new(a, b);

        // Execute & assert
        Assert.Equal(HashCode.Combine(a.GetHashCode(), b.GetHashCode()), valueObjectA.GetHashCode());
    }
}
