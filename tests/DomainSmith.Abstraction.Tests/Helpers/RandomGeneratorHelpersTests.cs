using DomainSmith.Abstraction.Helpers;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Helpers;

public sealed class RandomGeneratorHelpersTests
{
    [Theory]
    [InlineData("short", "(short)new Random().Next(short.MinValue, short.MaxValue)")]
    [InlineData("Int16", "(short)new Random().Next(short.MinValue, short.MaxValue)")]
    [InlineData("System.Int16", "(short)new Random().Next(short.MinValue, short.MaxValue)")]
    [InlineData("ushort", "(ushort)new Random().Next(0, short.MaxValue)")]
    [InlineData("UInt16", "(ushort)new Random().Next(0, short.MaxValue)")]
    [InlineData("System.UInt16", "(ushort)new Random().Next(0, short.MaxValue)")]
    [InlineData("int", "new Random().Next()")]
    [InlineData("Int32", "new Random().Next()")]
    [InlineData("System.Int32", "new Random().Next()")]
    [InlineData("uint", "(uint)new Random().Next(0, int.MaxValue)")]
    [InlineData("UInt32", "(uint)new Random().Next(0, int.MaxValue)")]
    [InlineData("System.UInt32", "(uint)new Random().Next(0, int.MaxValue)")]
    [InlineData("long", "(long)new Random().Next()")]
    [InlineData("Int64", "(long)new Random().Next()")]
    [InlineData("System.Int64", "(long)new Random().Next()")]
    [InlineData("ulong", "(ulong)new Random().Next(0, int.MaxValue)")]
    [InlineData("UInt64", "(ulong)new Random().Next(0, int.MaxValue)")]
    [InlineData("System.UInt64", "(ulong)new Random().Next(0, int.MaxValue)")]
    [InlineData("Int128", "(Int128)new Random().Next()")]
    [InlineData("System.Int128", "(Int128)new Random().Next()")]
    [InlineData("UInt128", "(UInt128)new Random().Next(0, int.MaxValue)")]
    [InlineData("System.UInt128", "(UInt128)new Random().Next(0, int.MaxValue)")]
    public void ToGeneratingExpression_ForNumericTypes_ShouldReturnExpectedExpression(string input, string expected)
    {
        // Act
        var result = input.ToGeneratingExpression();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Guid", "Guid.NewGuid()")]
    [InlineData("System.Guid", "Guid.NewGuid()")]
    public void ToGeneratingExpression_ForGuidTypes_ShouldReturnNewGuidExpression(string input, string expected)
    {
        // Act
        var result = input.ToGeneratingExpression();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("string", "Guid.NewGuid().ToString()")]
    [InlineData("String", "Guid.NewGuid().ToString()")]
    [InlineData("System.String", "Guid.NewGuid().ToString()")]
    public void ToGeneratingExpression_ForStringTypes_ShouldReturnGuidToStringExpression(string input, string expected)
    {
        // Act
        var result = input.ToGeneratingExpression();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("DateTime")]
    [InlineData("System.DateTime")]
    [InlineData("bool")]
    [InlineData("System.Boolean")]
    [InlineData("decimal")]
    [InlineData("System.Decimal")]
    [InlineData("float")]
    [InlineData("System.Single")]
    [InlineData("double")]
    [InlineData("System.Double")]
    [InlineData("SomeCustomType")]
    public void ToGeneratingExpression_ForUnsupportedTypes_ShouldReturnNull(string input)
    {
        // Act
        var result = input.ToGeneratingExpression();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToGeneratingExpression_ForNull_ShouldReturnNull()
    {
        // Act
        string? input = null;
        var result = input.ToGeneratingExpression();

        // Assert
        result.Should().BeNull();
    }
}