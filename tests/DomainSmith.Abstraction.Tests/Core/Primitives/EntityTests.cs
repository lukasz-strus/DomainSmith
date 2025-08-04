using DomainSmith.Abstraction.Core.Primitives;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Primitives;

public sealed class EntityTests
{
    [Fact]
    public void Entities_WithSameId_ShouldBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var areEqual = entity1 == entity2;

        // Assert
        areEqual.Should().BeTrue();
        entity1.Equals(entity2).Should().BeTrue();
        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }

    [Fact]
    public void Entities_WithDifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var areEqual = entity1 == entity2;

        // Assert
        areEqual.Should().BeFalse();
        entity1.Equals(entity2).Should().BeFalse();
        entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }

    [Fact]
    public void Entity_ShouldNotBeEqualToNull()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        var isEqual = entity == null!;

        // Assert
        isEqual.Should().BeFalse();
        entity!.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Entity_ShouldBeEqualToItself()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        // ReSharper disable once EqualExpressionComparison
        var isEqual = entity == entity;

        // Assert
        isEqual.Should().BeTrue();
        // ReSharper disable once EqualExpressionComparison
        entity.Equals(entity).Should().BeTrue();
    }

    [Fact]
    public void EntitiesOfDifferent_TypesShouldNotBeEqual()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new AnotherTestEntity(Guid.NewGuid());

        // Act
        // ReSharper disable once SuspiciousTypeConversion.Global
        var areEqual = entity1.Equals(entity2);

        // Assert
        areEqual.Should().BeFalse();
    }

    [Fact]
    public void EntitiesOfDifferentTypes_ShouldNotBeEqualByOperator()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new AnotherTestEntity(Guid.NewGuid());

        // Act
        var areEqual = entity1 != entity2;

        // Assert
        areEqual.Should().BeTrue();
    }

    private sealed class TestEntity(Guid id) : Entity<Guid>(id);

    private sealed class AnotherTestEntity(Guid id) : Entity<Guid>(id);
}