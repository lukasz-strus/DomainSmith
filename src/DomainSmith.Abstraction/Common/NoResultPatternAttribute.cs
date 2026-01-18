using System;

namespace DomainSmith.Abstraction.Common;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
public sealed class NoResultPatternAttribute : Attribute;