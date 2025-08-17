using System.Collections.Generic;

namespace DomainSmith.Abstraction.Core.Primitives;

public abstract record EntityIdRecord<T>(T Value)
    where T : notnull;

public abstract class EntityIdClass<T>(T value) : ValueObject
    where T : notnull
{
    public T Value { get; init; } = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}