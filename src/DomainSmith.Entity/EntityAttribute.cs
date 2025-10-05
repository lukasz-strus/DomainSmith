namespace DomainSmith.Entity;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityAttribute(Type idType) : Attribute
{
    public Type IdType { get; } = idType;
}