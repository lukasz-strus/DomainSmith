using System;
using System.Threading.Tasks;

namespace DomainSmith.Abstraction.Core.Result.Extensions;

public static class ResultExtensions
{
    extension(Task<Result> resultTask)
    {
        public async Task<T> Match<T>(Func<T> onSuccess, Func<Result, T> onFailure)
        {
            var result = await resultTask;

            return result.Match(onSuccess, onFailure);
        }
    }

    extension(Result result)
    {
        public T Match<T>(Func<T> onSuccess, Func<Result, T> onFailure) =>
            result.IsSuccess ? onSuccess() : onFailure(result);

        public Result<T> Map<T>(Func<Result<T>> func) =>
            result.IsSuccess ? func() : Result.Failure<T>(result.Error);
    }

    extension<TValue>(Result<TValue> result)
    {
        public async Task<Result> Bind(Func<TValue, Task<Result>> func) =>
            result.IsSuccess ? await func(result.Value()) : Result.Failure(result.Error);

        public async Task<Result<T>>
            Bind<T>(Func<TValue, Task<Result<T>>> func) =>
            result.IsSuccess ? await func(result.Value()) : Result.Failure<T>(result.Error);

        public async Task<Result<T>> Bind<T>(Func<TValue, Task<T>> func)
            where T : class =>
            result.IsSuccess ? await func(result.Value()) : Result.Failure<T>(result.Error);

        public async Task<Result<T>> Bind<T>(Func<TValue, Task<T?>> func,
            Error error) where T : class
        {
            if (result.IsFailure)
            {
                return Result.Failure<T>(result.Error);
            }

            var value = await func(result.Value());

            return value is null ? Result.Failure<T>(error) : Result.Success(value);
        }

        public async Task<Result<T>> BindScalar<T>(Func<TValue, Task<T>> func)
            where T : struct
        {
            return result.IsSuccess ? await func(result.Value()) : Result.Failure<T>(result.Error);
        }

        public T Match<T>(Func<TValue, T> onSuccess,
            Func<Result<TValue>, T> onFailure) =>
            result.IsSuccess ? onSuccess(result.Value()) : onFailure(result);

        public Result<T> Map<T>(Func<TValue, T> func) =>
            result.IsSuccess ? Result.Success(func(result.Value())) : Result.Failure<T>(result.Error);

        public async Task<Result<TValue>> Ensure(Func<TValue, Task<bool>> predicate, Error error)
        {
            if (result.IsFailure)
            {
                return result;
            }

            return await predicate(result.Value()) ? Result.Success(result.Value()) : Result.Failure<TValue>(error);
        }

        public Result<TValue> Ensure(Func<TValue, bool> predicate, Error error)
        {
            if (result.IsFailure)
            {
                return result;
            }

            return predicate(result.Value()) ? Result.Success(result.Value()) : Result.Failure<TValue>(error);
        }
    }

    extension<TValue>(Task<Result<TValue>> resultTask)
    {
        public async Task<Result<T>> Bind<T>(Func<TValue, Task<T>> func)
        {
            var result = await resultTask;

            return result.IsSuccess ? Result.Success(await func(result.Value())) : Result.Failure<T>(result.Error);
        }

        public async Task<Result> Tap(Action<TValue> action)
        {
            var result = await resultTask;

            if (result.IsSuccess)
            {
                action(result.Value());
            }

            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }

        public async Task<Result<T>> Map<T>(Func<TValue, T> func)
        {
            var result = await resultTask;

            return result.IsSuccess ? Result.Success(func(result.Value())) : Result.Failure<T>(result.Error);
        }

        public async Task<T> Match<T>(Func<TValue, T> onSuccess,
            Func<Result<TValue>, T> onFailure)
        {
            var result = await resultTask;

            return result.Match(onSuccess, onFailure);
        }

        public async Task<Result<TValue>> Ensure(Func<TValue, bool> predicate, Error error)
        {
            var result = await resultTask;

            if (result.IsFailure)
            {
                return result;
            }

            return predicate(result.Value()) ? Result.Success(result.Value()) : Result.Failure<TValue>(error);
        }
    }
}