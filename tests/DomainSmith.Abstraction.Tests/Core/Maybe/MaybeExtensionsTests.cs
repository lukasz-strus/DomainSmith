using DomainSmith.Abstraction.Core.Maybe;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Maybe;

public sealed class MaybeExtensionsTests
{
    [Fact]
    public async Task Bind_WhenMaybeHasValue_ShouldInvokeFunc_AndReturnItsResult()
    {
        // Arrange
        var maybe = Maybe<int>.From(2);
        var invoked = false;

        // Act
        var result = await maybe.Bind(x =>
        {
            invoked = true;
            return Task.FromResult(Maybe<string>.From((x * 10).ToString()));
        });

        // Assert
        invoked.Should().BeTrue();
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("20");
    }

    [Fact]
    public async Task Bind_WhenFuncReturnsNone_ShouldReturnNone()
    {
        // Arrange
        var maybe = Maybe<int>.From(1);

        // Act
        var result = await maybe.Bind(_ => Task.FromResult(Maybe<string>.None));

        // Assert
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public async Task Match_WhenTaskReturnsSome_ShouldCallOnSuccess_AndReturnItsValue()
    {
        // Arrange
        Task<Maybe<int>> task = Task.FromResult(Maybe<int>.From(5));
        var successCalled = false;
        var failureCalled = false;

        // Act
        var result = await task.Match(
            onSuccess: x =>
            {
                successCalled = true;
                return x + 1;
            },
            onFailure: () =>
            {
                failureCalled = true;
                return -1;
            });

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
        result.Should().Be(6);
    }
}