namespace DomainSmith.Abstraction.Common.Models;

public readonly record struct IdMetadata(bool IsRecord, bool IsClass, string? ValueType);