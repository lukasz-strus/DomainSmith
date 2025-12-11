using DomainSmith.Abstraction.Core.Result;
using DomainSmith.Abstraction.Core.Result.Extensions;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Result.Extensions;

public sealed class ResultExtensionsTests
{
    private static readonly Error TestError = new("ERR", "Błąd");

    [Fact]
    public void Ensure_Should_Return_Original_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);
        var called = false;

        var ensured = result.Ensure(_ =>
        {
            called = true;
            return true;
        }, new Error("E", "err"));

        ensured.Should().BeSameAs(result);
        called.Should().BeFalse();
    }

    [Fact]
    public void Ensure_Should_Return_Success_If_Predicate_True()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var ensured = result.Ensure(x => x > 0, TestError);

        ensured.IsSuccess.Should().BeTrue();
        ensured.Value().Should().Be(5);
    }

    [Fact]
    public void Ensure_Should_Return_Failure_If_Predicate_False()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var ensured = result.Ensure(x => x < 0, TestError);

        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task Ensure_AsyncTask_Should_Return_Original_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);
        var called = false;

        var ensured = await Task.FromResult(result).Ensure(_ =>
        {
            called = true;
            return true;
        }, TestError);

        ensured.Should().BeSameAs(result);
        called.Should().BeFalse();
    }

    [Fact]
    public async Task Ensure_AsyncTask_Should_Return_Success_If_Predicate_True()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var ensured = await Task.FromResult(result).Ensure(x => x > 0, TestError);

        ensured.IsSuccess.Should().BeTrue();
        ensured.Value().Should().Be(5);
    }

    [Fact]
    public async Task Ensure_AsyncTask_Should_Return_Failure_If_Predicate_False()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var ensured = await Task.FromResult(result).Ensure(x => x < 0, TestError);

        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task Ensure_AsyncPredicate_Should_Return_Original_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);
        var called = false;

        var ensured = await result.Ensure(async _ =>
        {
            called = true;
            await Task.Yield();
            return true;
        }, TestError);

        ensured.Should().BeSameAs(result);
        called.Should().BeFalse();
    }

    [Fact]
    public async Task Ensure_AsyncPredicate_Should_Return_Success_If_Predicate_True()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var ensured = await result.Ensure(async x =>
        {
            await Task.Yield();
            return x > 0;
        }, TestError);

        ensured.IsSuccess.Should().BeTrue();
        ensured.Value().Should().Be(5);
    }

    [Fact]
    public async Task Ensure_AsyncPredicate_Should_Return_Failure_If_Predicate_False()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var ensured = await result.Ensure(async x =>
        {
            await Task.Yield();
            return x < 0;
        }, TestError);

        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(TestError);
    }

    [Fact]
    public void Match_Should_Invoke_OnSuccess_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success();

        var value = result.Match(() => 1, _ => 2);

        value.Should().Be(1);
    }

    [Fact]
    public void Match_Should_Invoke_OnFailure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure(TestError);

        var value = result.Match(() => 1, _ => 2);

        value.Should().Be(2);
    }

    [Fact]
    public void MatchT_Should_Invoke_OnSuccess_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(10);

        var value = result.Match(x => x + 1, _ => 0);

        value.Should().Be(11);
    }

    [Fact]
    public void MatchT_Should_Invoke_OnFailure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var value = result.Match(x => x + 1, _ => 0);

        value.Should().Be(0);
    }

    [Fact]
    public async Task Match_Async_Should_Invoke_OnSuccess_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success();

        var value = await Task.FromResult(result).Match(() => 1, _ => 2);

        value.Should().Be(1);
    }

    [Fact]
    public async Task Match_Async_Should_Invoke_OnFailure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure(TestError);

        var value = await Task.FromResult(result).Match(() => 1, _ => 2);

        value.Should().Be(2);
    }

    [Fact]
    public async Task MatchT_Async_Should_Invoke_OnSuccess_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(10);

        var value = await Task.FromResult(result).Match(x => x + 1, _ => 0);

        value.Should().Be(11);
    }

    [Fact]
    public async Task MatchT_Async_Should_Invoke_OnFailure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var value = await Task.FromResult(result).Match(x => x + 1, _ => 0);

        value.Should().Be(0);
    }

    [Fact]
    public void Map_Should_Return_Success_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success();

        var mapped = result.Map(() => DomainSmith.Abstraction.Core.Result.Result.Success(123));

        mapped.IsSuccess.Should().BeTrue();
        mapped.Value().Should().Be(123);
    }

    [Fact]
    public void Map_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure(TestError);

        var mapped = result.Map(() => DomainSmith.Abstraction.Core.Result.Result.Success(123));

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Should().Be(TestError);
    }

    [Fact]
    public void MapT_Should_Return_Success_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var mapped = result.Map(x => x * 2);

        mapped.IsSuccess.Should().BeTrue();
        mapped.Value().Should().Be(10);
    }

    [Fact]
    public void MapT_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var mapped = result.Map(x => x * 2);

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task MapT_Async_Should_Return_Success_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var mapped = await Task.FromResult(result).Map(x => x * 2);

        mapped.IsSuccess.Should().BeTrue();
        mapped.Value().Should().Be(10);
    }

    [Fact]
    public async Task MapT_Async_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var mapped = await Task.FromResult(result).Map(x => x * 2);

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task Bind_ResultTValue_FuncTaskResult_Should_Return_Result_Of_Func_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(7);

        var bound = await result.Bind(_ => Task.FromResult(DomainSmith.Abstraction.Core.Result.Result.Success()));

        bound.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Bind_ResultTValue_FuncTaskResult_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var bound = await result.Bind(_ => Task.FromResult(DomainSmith.Abstraction.Core.Result.Result.Success()));

        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task Bind_Should_Return_Result_Of_Func_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var bound = await result.Bind(x => Task.FromResult(DomainSmith.Abstraction.Core.Result.Result.Success(x * 2)));

        bound.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Bind_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var bound = await result.Bind(x => Task.FromResult(DomainSmith.Abstraction.Core.Result.Result.Success(x * 2)));

        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task BindT_Should_Return_Result_Of_Func_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var bound = await result.Bind(x => Task.FromResult(DomainSmith.Abstraction.Core.Result.Result.Success(x * 2)));

        bound.IsSuccess.Should().BeTrue();
        bound.Value().Should().Be(10);
    }

    [Fact]
    public async Task BindT_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<int>(TestError);

        var bound = await result.Bind(x => Task.FromResult(DomainSmith.Abstraction.Core.Result.Result.Success(x * 2)));

        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task BindT_Class_Should_Return_Result_Of_Func_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success("abc");

        var bound = await result.Bind<string, string>(x => Task.FromResult(x + "d"));

        bound.IsSuccess.Should().BeTrue();
        bound.Value().Should().Be("abcd");
    }

    [Fact]
    public async Task BindT_Class_Should_Return_Failure_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<string>(TestError);

        var bound = await result.Bind<string, string>(x => Task.FromResult(x + "d"));

        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task BindT_Class_With_Error_Should_Return_Failure_If_Func_Returns_Null()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success("abc");

        var bound = await result.Bind<string, string>(_ => Task.FromResult<string?>(null), TestError);

        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task BindT_Class_With_Error_Should_Return_Failure_If_Original_Is_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<string>(TestError);

        var bound = await result.Bind<string, string>(_ => Task.FromResult<string?>("ok"), TestError);

        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task BindT_AsyncTask_Should_Return_Result_Of_Func_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var bound = await Task.FromResult(result).Bind(x => Task.FromResult(x * 2));

        bound.IsSuccess.Should().BeTrue();
        bound.Value().Should().Be(10);
    }

    [Fact]
    public async Task BindScalar_Should_Return_Result_Of_Func_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success(5);

        var bound = await result.BindScalar(x => Task.FromResult(x * 2));

        bound.IsSuccess.Should().BeTrue();
        bound.Value().Should().Be(10);
    }

    [Fact]
    public async Task Tap_Should_Invoke_Action_If_Success()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Success("abc");
        string? tapped = null;

        var tappedResult = await Task.FromResult(result).Tap(x => tapped = x);

        tapped.Should().Be("abc");
        tappedResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Tap_Should_Not_Invoke_Action_If_Failure()
    {
        var result = DomainSmith.Abstraction.Core.Result.Result.Failure<string>(TestError);
        var tapped = false;

        var tappedResult = await Task.FromResult(result).Tap(_ => tapped = true);

        tapped.Should().BeFalse();
        tappedResult.IsFailure.Should().BeTrue();
    }
}