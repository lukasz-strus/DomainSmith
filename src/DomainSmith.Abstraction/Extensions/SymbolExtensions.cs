using System.Linq;
using DomainSmith.Abstraction.Common;
using Microsoft.CodeAnalysis;

namespace DomainSmith.Abstraction.Extensions;

public static class SymbolExtensions
{
    public static bool HasExcludeFromGenerationAttribute(this ISymbol symbol)
        => symbol.GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == typeof(ExcludeFromGenerationAttribute).FullName);

    public static bool HasNoResultPatternAttribute(this ISymbol symbol)
        => symbol.GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == typeof(NoResultPatternAttribute).FullName);
}