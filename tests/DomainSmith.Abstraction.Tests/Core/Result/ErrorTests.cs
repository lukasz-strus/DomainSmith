using DomainSmith.Abstraction.Core.Result;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Result;

public sealed class ErrorTests
{
    [Fact]
    public void ImplicitOperator_ShouldReturnEmptyString_WhenErrorIsNull()
    {
        // Arrange
        Error? error = null;

        // Act
        string code = error;

        // Assert
        code.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnCode_WhenErrorIsNotNull()
    {
        // Arrange
        var error = new Error("SomeCode", "Some message");

        // Act
        string code = error;

        // Assert
        code.Should().Be("SomeCode");
    }

    [Fact]
    public void None_ShouldReturnErrorWithEmptyCodeAndMessage()
    {
        // Act
        var none = typeof(Error)
            .GetProperty("None",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public)!
            .GetValue(null) as Error;

        // Assert
        none.Should().NotBeNull();
        none!.Code.Should().BeEmpty();
        none.Message.Should().BeEmpty();
    }
}