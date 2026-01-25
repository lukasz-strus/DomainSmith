using DomainSmith.Abstraction.Core.Maybe;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Maybe;

public sealed class MaybeTests
{
    [Fact]
    public void None_ShouldHaveNoValue()
    {
        // Act
        var maybe = Maybe<string>.None;

        // Assert
        maybe.HasNoValue.Should().BeTrue();
        maybe.HasValue.Should().BeFalse();
    }

    [Fact]
    public void From_WithNonNullReference_ShouldHaveValue_AndExposeValue()
    {
        // Act
        var maybe = Maybe<string>.From("abc");

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.HasNoValue.Should().BeFalse();
        maybe.Value.Should().Be("abc");
    }

    [Fact]
    public void From_WithNullReference_ShouldHaveNoValue()
    {
        // Act
        var maybe = Maybe<string>.From(null!);

        // Assert
        maybe.HasNoValue.Should().BeTrue();
        maybe.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldCreateMaybe_EquivalentToFrom()
    {
        // Arrange
        const string value = "xyz";

        // Act
        Maybe<string> fromImplicit = value;
        var fromFactory = Maybe<string>.From(value);

        // Assert
        fromImplicit.Equals(fromFactory).Should().BeTrue();
    }

    [Fact]
    public void Value_WhenNone_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var maybe = Maybe<string>.None;

        // Act
        var act = () => _ = maybe.Value;

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("The value can not be accessed because it does not exist.");
    }

    [Fact]
    public void Equals_WhenBothNone_ShouldBeTrue()
    {
        // Arrange
        var a = Maybe<string>.None;
        var b = Maybe<string>.None;

        // Act / Assert
        a.Equals(b).Should().BeTrue();
        a.Equals((object)b).Should().BeTrue();
        a.GetHashCode().Should().Be(0);
        b.GetHashCode().Should().Be(0);
    }

    [Fact]
    public void Equals_WhenOneNone_AndOtherHasValue_ShouldBeFalse()
    {
        // Arrange
        var none = Maybe<string>.None;
        var some = Maybe<string>.From("a");

        // Act / Assert
        none.Equals(some).Should().BeFalse();
        some.Equals(none).Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenBothHaveSameValue_ShouldBeTrue()
    {
        // Arrange
        var a = Maybe<int>.From(123);
        var b = Maybe<int>.From(123);

        // Act / Assert
        a.Equals(b).Should().BeTrue();
        a.Equals((object)b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_ObjectOverload_WhenObjectIsUnderlyingValue_ShouldBeTrue()
    {
        // Arrange
        var maybe = Maybe<int>.From(10);

        // Act / Assert
        // ReSharper disable once SuspiciousTypeConversion.Global
        maybe.Equals((object)10).Should().BeTrue();
    }

    [Fact]
    public void Equals_ObjectOverload_WhenNull_ShouldBeFalse()
    {
        // Arrange
        var maybe = Maybe<int>.From(10);

        // Act / Assert
        maybe.Equals(((object?)null)!).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WhenHasValue_ShouldMatchUnderlyingValueHashCode()
    {
        // Arrange
        var maybe = Maybe<string>.From("hash");

        // Act
        var hash = maybe.GetHashCode();

        // Assert
        hash.Should().Be("hash".GetHashCode());
    }
}