using DomainSmith.Abstraction.Common.Models;

namespace DomainSmith.AggregateRoot.Generators.Models;

public sealed class ClassToAugment(
    string name,
    string typeArg,
    string? ns,
    List<string> usings,
    bool isAggregateRootIdRecord,
    bool isAggregateRootIdClass,
    string? idValueType,
    List<PropertyInfo> properties,
    bool noResultPattern,
    List<EntityCollectionInfo> entityCollections)
    : BaseArgClassToAugment(
        name,
        typeArg,
        ns,
        usings,
        isAggregateRootIdRecord,
        isAggregateRootIdClass,
        idValueType,
        properties,
        noResultPattern)
{
    public List<EntityCollectionInfo> EntityCollections { get; } = entityCollections;
}