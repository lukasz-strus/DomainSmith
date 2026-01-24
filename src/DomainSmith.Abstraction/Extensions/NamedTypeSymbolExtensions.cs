using DomainSmith.Abstraction.Common.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace DomainSmith.Abstraction.Extensions;

public static class NamedTypeSymbolExtensions
{
    extension(INamedTypeSymbol idTypeSymbol)
    {
        public IdMetadata GetIdMetadata()
        {
            var isRecord = false;
            var isClass = false;
            string? valueType = null;

            for (var baseType = idTypeSymbol.BaseType; baseType != null; baseType = baseType.BaseType)
            {
                var baseName = baseType.ConstructedFrom.ToDisplayString();

                if (baseName == "DomainSmith.Abstraction.Core.Primitives.EntityIdRecord<T>")
                {
                    isRecord = true;
                    valueType = baseType.TypeArguments.FirstOrDefault()?.ToDisplayString();
                    break;
                }

                if (baseName == "DomainSmith.Abstraction.Core.Primitives.EntityIdClass<T>")
                {
                    isClass = true;
                    valueType = baseType.TypeArguments.FirstOrDefault()?.ToDisplayString();
                    break;
                }
            }

            return new IdMetadata(isRecord, isClass, valueType);
        }

        public List<ParameterInfo> GetEntityCtorArgsFromPublicProperties()
        {
            return idTypeSymbol
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public)
                .Where(p => !p.IsStatic)
                .Where(p => p.Name != "Id")
                .OrderBy(p => p.Locations.FirstOrDefault()?.SourceSpan.Start ?? int.MaxValue)
                .Select(p => new ParameterInfo(
                    p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                    p.Name.ToCamel()
                ))
                .ToList();
        }
    }
}