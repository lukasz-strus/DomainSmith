using DomainSmith.Abstraction.Common.Models;

namespace DomainSmith.AggregateRoot.Generators.Models;

public sealed class EntityCollectionInfo(
    string propertyName,
    string backingFieldName,
    string elementType,
    string elementIdType,
    bool generateProperty,
    List<ParameterInfo> ctorArgs,
    bool elementIsResultPattern)
{
    public string PropertyName { get; } = propertyName;
    public string BackingFieldName { get; } = backingFieldName;
    public string ElementType { get; } = elementType;
    public string ElementIdType { get; } = elementIdType;
    public bool GenerateProperty { get; } = generateProperty;

    public List<ParameterInfo> CtorArgs { get; } = ctorArgs;

    public bool ElementIsResultPattern { get; } = elementIsResultPattern;
}