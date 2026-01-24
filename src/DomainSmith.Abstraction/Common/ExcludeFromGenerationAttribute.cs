using System;

namespace DomainSmith.Abstraction.Common;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ExcludeFromGenerationAttribute : Attribute;