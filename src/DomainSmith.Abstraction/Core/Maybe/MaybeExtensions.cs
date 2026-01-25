using System;
using System.Threading.Tasks;

namespace DomainSmith.Abstraction.Core.Maybe;

public static class MaybeExtensions
{
    public static async Task<Maybe<TOut>> Bind<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<Maybe<TOut>>> func) =>
        maybe.HasValue ? await func(maybe.Value) : Maybe<TOut>.None;


    public static async Task<TOut> Match<TIn, TOut>(
        this Task<Maybe<TIn>> resultTask,
        Func<TIn, TOut> onSuccess,
        Func<TOut> onFailure)
    {
        var maybe = await resultTask;

        return maybe.HasValue ? onSuccess(maybe.Value) : onFailure();
    }
}