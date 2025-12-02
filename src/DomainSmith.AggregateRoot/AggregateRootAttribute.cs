using System.Diagnostics.CodeAnalysis;

namespace DomainSmith.AggregateRoot;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public sealed class AggregateRootAttribute(Type idType) : Attribute
{
    public Type IdType { get; } = idType;
}