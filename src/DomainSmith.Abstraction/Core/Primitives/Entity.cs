using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DomainSmith.Abstraction.Core.Primitives;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    [ExcludeFromCodeCoverage]
    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public TId Id { get; protected set; } = default!;

    public bool Equals(Entity<TId> other)
    {
        return other != null && EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((Entity<TId>)obj);
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return EqualityComparer<TId>.Default.GetHashCode(Id) * 48;
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }
}