using System.Diagnostics.CodeAnalysis;

namespace DomainSmith.Abstraction.Core.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    [ExcludeFromCodeCoverage]
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }
}