using DomainSmith.Abstraction.Common.Models;

namespace DomainSmith.Entity.Generators.Models;

public sealed class ClassToAugment(
    string name,
    string typeArg,
    string? ns,
    List<string> usings,
    bool isEntityIdRecord,
    bool isEntityIdClass,
    string? idValueType,
    List<PropertyInfo> properties,
    bool noResultPattern) : BaseArgClassToAugment(
    name,
    typeArg,
    ns,
    usings,
    isEntityIdRecord,
    isEntityIdClass,
    idValueType,
    properties,
    noResultPattern)
{
}