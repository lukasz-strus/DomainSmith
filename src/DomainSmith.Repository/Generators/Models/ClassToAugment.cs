using DomainSmith.Abstraction.Common.Models;

namespace DomainSmith.Repository.Generators.Models;

public sealed class ClassToAugment(
    string name,
    string typeArg,
    string? ns,
    List<string> usings,
    bool isAggregateRootIdRecord,
    bool isAggregateRootIdClass,
    string? idValueType,
    List<PropertyInfo> properties,
    bool noResultPattern)
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
}