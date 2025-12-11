using DomainSmith.Abstraction.Core.Result;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Result;

public sealed class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessResult()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult()
    {
        var error = new Error("ERR", "Błąd");
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SuccessT_ShouldReturnSuccessResultWithValue()
    {
        var value = 42;
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(value);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
        result.Value().Should().Be(value);
    }

    [Fact]
    public void FailureT_ShouldReturnFailureResultWithDefaultValue()
    {
        var error = new Error("ERR", "Błąd");
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        Action act = () => result.Value();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenValueIsNotNull()
    {
        var value = "test";
        var result = DomainSmith.Abstraction.Core.Result.Result.Create(value, new Error("ERR", "Błąd"));

        result.IsSuccess.Should().BeTrue();
        result.Value().Should().Be(value);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenValueIsNull()
    {
        var error = new Error("ERR", "Błąd");
        var result = DomainSmith.Abstraction.Core.Result.Result.Create<string>(null, error);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void FirstFailureOrSuccess_ShouldReturnFirstFailure()
    {
        var error = new Error("ERR", "Błąd");
        var success = DomainSmith.Abstraction.Core.Result.Result.Success();
        var failure = DomainSmith.Abstraction.Core.Result.Result.Failure(error);

        var result = DomainSmith.Abstraction.Core.Result.Result.FirstFailureOrSuccess(success, failure, success);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void FirstFailureOrSuccess_ShouldReturnSuccess_WhenNoFailures()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.FirstFailureOrSuccess(
            DomainSmith.Abstraction.Core.Result.Result.Success(),
            DomainSmith.Abstraction.Core.Result.Result.Success());

        result.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public void Value_ShouldThrow_WhenFailure()
    {
        var error = new Error("ERR", "Błąd");
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<string>(error);

        Action act = () => result.Value();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnSuccessResult()
    {
        Result<int> result = 123;

        result.IsSuccess.Should().BeTrue();
        result.Value().Should().Be(123);
    }

    [Fact]
    public void ShouldThhrowInvalidOperationException_WhenAttemptingToRetrieveValueOfFailureResult()
    {
        var result =
            DomainSmith.Abstraction.Core.Result.Result.Failure<DomainSmith.Abstraction.Core.Result.Result>(
                new Error("ERR", "Błąd"));

        result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CustomResultCtor_ShouldThrow_WhenErrorIsNoneAndIsSuccessIsFalse()
    {
        var error = Error.None;

        Action act = () => new CustomResult(false, error);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CustomResultCtor_ShouldThrow_WhenErrorIsNotNoneAndIsSuccessIsTrue()
    {
        var error = new Error("ERR", "Błąd");

        Action act = () => new CustomResult(true, error);

        act.Should().Throw<InvalidOperationException>();
    }

    private class CustomResult(bool isSuccess, Error error)
        : DomainSmith.Abstraction.Core.Result.Result(isSuccess, error);
}