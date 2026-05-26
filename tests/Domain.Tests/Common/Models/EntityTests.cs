using Domain.Common.Models;

namespace Domain.Tests.Common.Models;

public class EntityTests
{
    private class EntityA(Guid id) : Entity(id) { }
    private class EntityB(Guid id) : Entity(id) { }
    private class NoEntity(Guid id) { public Guid Id = id; }
    private class ParentEntity(Guid id) : EntityA(id);

    [Fact]
    public void Equals_ShouldReturnTrue_WhenOtherIsEntityAndIdEquals()
    {
        // Setup
        var id = Guid.NewGuid();
        EntityA entityA = new(id);
        EntityB entityB = new(id);

        // Execute & Assert
        Assert.True(entityA.Equals(entityB));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsEntityButIdNotEquals()
    {
        // Setup
        EntityA entityA = new(Guid.NewGuid());
        EntityB entityB = new(Guid.NewGuid());

        // Execute & Assert
        Assert.False(entityA.Equals(entityB));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsNotEntity()
    {
        // Setup
        var id = Guid.NewGuid();
        EntityA entityA = new(id);
        NoEntity noEntity = new(id);

        // Execute & Assert
        Assert.False(entityA.Equals(noEntity));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenOtherIsParentAndIdEquals()
    {
        // Setup
        var id = Guid.NewGuid();
        EntityA entityA = new(id);
        ParentEntity entityParent = new(id);

        // Execute & Assert
        Assert.True(entityA.Equals(entityParent));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsNull()
    {
        // Setup
        EntityA entityA = new(Guid.NewGuid());

        // Execute & Assert
        Assert.False(entityA.Equals(null));
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrue_WhenBothNotNullAndIdEquals()
    {
        // Setup
        var id = Guid.NewGuid();
        EntityA entityA = new(id);
        EntityB entityB = new(id);

        // Execute & Assert
        Assert.True(entityA == entityB);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnFalse_WhenBothNotNullButIdNotEquals()
    {
        // Setup
        EntityA entityA = new(Guid.NewGuid());
        EntityB entityB = new(Guid.NewGuid());

        // Execute & Assert
        Assert.False(entityA == entityB);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnFalse_WhenRhsNull()
    {
        // Setup
        EntityA entityA = new(Guid.NewGuid());

        // Execute & Assert
        Assert.False(entityA == null);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrue_WhenBothNull()
    {
        // Setup
        EntityA? entityA = null;

        // Execute & Assert
        Assert.True(entityA == null);
    }

    [Fact]
    public void GetHashCode_ShouldReturnIdHashCode()
    {
        // Setup
        var id = Guid.NewGuid();
        EntityA entityA = new(id);

        // Execute & assert
        Assert.Equal(id.GetHashCode(), entityA.GetHashCode());
    }
}
