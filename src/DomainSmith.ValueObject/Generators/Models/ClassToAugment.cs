using DomainSmith.Abstraction.Common.Models;

namespace DomainSmith.ValueObject.Generators.Models;

public sealed class ClassToAugment(
    string name,
    string? ns,
    List<string> usings,
    List<PropertyInfo> properties,
    bool noResultPattern) : BaseClassToAugment(name, ns, usings, properties, noResultPattern)
{
}