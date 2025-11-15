using System.Diagnostics.CodeAnalysis;

namespace DomainSmith.ValueObject;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public sealed class ValueObjectAttribute : Attribute;
