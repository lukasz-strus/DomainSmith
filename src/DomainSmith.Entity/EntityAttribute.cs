using System.Diagnostics.CodeAnalysis;

namespace DomainSmith.Entity;

[ExcludeFromCodeCoverage]   
[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityAttribute(Type idType) : Attribute
{
    public Type IdType { get; } = idType;
}