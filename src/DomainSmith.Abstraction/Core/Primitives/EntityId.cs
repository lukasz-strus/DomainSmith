using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DomainSmith.Abstraction.Core.Primitives;

[ExcludeFromCodeCoverage]
public abstract record EntityIdRecord<T>(T Value)
    where T : notnull;

[ExcludeFromCodeCoverage]
public abstract class EntityIdClass<T>(T value) : ValueObject
    where T : notnull
{
    public T Value { get; init; } = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}