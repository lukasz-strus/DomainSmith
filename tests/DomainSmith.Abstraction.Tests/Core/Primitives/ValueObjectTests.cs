using DomainSmith.Abstraction.Core.Primitives;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Primitives;

public sealed class ValueObjectTests
{
    private class TestValueObject : ValueObject
    {
        public int A { get; }
        public string B { get; }

        public TestValueObject(int a, string b)
        {
            A = a;
            B = b;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return A;
            yield return B;
        }
    }

    [Fact]
    public void Operator_Equality_ShouldReturnTrue_ForEqualObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(1, "abc");

        (vo1 == vo2).Should().BeTrue();
    }

    [Fact]
    public void Operator_Equality_ShouldReturnFalse_ForDifferentObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(2, "def");

        (vo1 == vo2).Should().BeFalse();
    }

    [Fact]
    public void Operator_Equality_ShouldReturnTrue_ForBothNull()
    {
        TestValueObject vo1 = null;
        TestValueObject vo2 = null;

        (vo1 == vo2).Should().BeTrue();
    }

    [Fact]
    public void Operator_Equality_ShouldReturnFalse_IfOneIsNull()
    {
        var vo1 = new TestValueObject(1, "abc");
        TestValueObject vo2 = null!;

        (vo1 == vo2).Should().BeFalse();
        (vo2 == vo1).Should().BeFalse();
    }

    [Fact]
    public void Operator_Inequality_ShouldReturnTrue_ForDifferentObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(2, "def");

        (vo1 != vo2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForEqualObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(1, "abc");

        vo1.Equals(vo2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(2, "def");

        vo1.Equals(vo2).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_IfOtherIsNull()
    {
        var vo1 = new TestValueObject(1, "abc");

        vo1.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_Object_ShouldReturnTrue_ForEqualObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        object vo2 = new TestValueObject(1, "abc");

        vo1.Equals(vo2).Should().BeTrue();
    }

    [Fact]
    public void Equals_Object_ShouldReturnFalse_ForDifferentType()
    {
        var vo1 = new TestValueObject(1, "abc");
        object vo2 = "not a value object";

        vo1.Equals(vo2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_ForEqualObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(1, "abc");

        vo1.GetHashCode().Should().Be(vo2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ShouldBeDifferent_ForDifferentObjects()
    {
        var vo1 = new TestValueObject(1, "abc");
        var vo2 = new TestValueObject(2, "def");

        vo1.GetHashCode().Should().NotBe(vo2.GetHashCode());
    }
}
