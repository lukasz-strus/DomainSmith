using System.Linq;
using DomainSmith.Abstraction.Common;
using Microsoft.CodeAnalysis;

namespace DomainSmith.Abstraction.Extensions;

public static class SymbolExtensions
{
    extension(ISymbol symbol)
    {
        public bool HasExcludeFromGenerationAttribute()
            => symbol.GetAttributes()
                .Any(a => a.AttributeClass?.ToDisplayString() == typeof(ExcludeFromGenerationAttribute).FullName);

        public bool HasNoResultPatternAttribute()
            => symbol.GetAttributes()
                .Any(a => a.AttributeClass?.ToDisplayString() == typeof(NoResultPatternAttribute).FullName);
    }
}