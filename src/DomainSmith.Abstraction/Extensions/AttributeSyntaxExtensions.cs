using System.Collections.Generic;
using System.Linq;
using DomainSmith.Abstraction.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.Abstraction.Extensions;

public static class AttributeSyntaxExtensions
{
    public static bool HasNoResultPattern(this IEnumerable<(AttributeSyntax Attr, IMethodSymbol? Symbol)> allAttributes,
        GeneratorSyntaxContext context)
    {
        var isNoResultPatternAssembly = context.SemanticModel.Compilation.Assembly
            .GetAttributes()
            .Any(a => a.AttributeClass?.ToDisplayString() == typeof(NoResultPatternAttribute).FullName);

        var isNoResultPatternLocal = allAttributes
            .Any(x => x.Symbol?.ContainingType.ToDisplayString() == typeof(NoResultPatternAttribute).FullName);

        return isNoResultPatternLocal || isNoResultPatternAssembly;
    }

    public static IEnumerable<AttributeSyntax> GetAttributesByFullName(
        this IEnumerable<(AttributeSyntax Attr, IMethodSymbol? Symbol)> allAttributes, string attributeFullName)
    {
        return allAttributes
            .Where(x => x.Symbol?.ContainingType.ToDisplayString() == attributeFullName)
            .Select(x => x.Attr);
    }

    public static bool TryGetIdType(this IEnumerable<AttributeSyntax> entityAttributes, GeneratorSyntaxContext context,
        out string typeArg,
        out INamedTypeSymbol? idTypeSymbol)
    {
        typeArg = "int";
        idTypeSymbol = null;

        foreach (var attr in entityAttributes)
        {
            if (attr.ArgumentList?.Arguments.Count <= 0)
                continue;

            if (attr.ArgumentList?.Arguments[0].Expression is not TypeOfExpressionSyntax typeOfExpr)
                continue;

            typeArg = typeOfExpr.Type.ToString();
            idTypeSymbol = context.SemanticModel.GetTypeInfo(typeOfExpr.Type).Type as INamedTypeSymbol;
            return idTypeSymbol is not null;
        }

        return false;
    }
}