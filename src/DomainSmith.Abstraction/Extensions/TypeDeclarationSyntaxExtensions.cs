using System.Collections.Generic;
using System.Linq;
using DomainSmith.Abstraction.Common.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.Abstraction.Extensions;

public static class TypeDeclarationSyntaxExtensions
{
    public static List<PropertyInfo> GetAugmentableProperties(this TypeDeclarationSyntax syntax,
        GeneratorSyntaxContext context) =>
        syntax.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(p => !p.IsExcludedFromGeneration(context))
            .Select(p => p.ToPropertyInfo(context))
            .ToList();

    public static string GetName(this TypeDeclarationSyntax syntax)
        => syntax.Identifier.Text;

    public static string? GetNamespace(this TypeDeclarationSyntax syntax)
        => syntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
           ?? syntax.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

    public static List<string> GetUsings(this TypeDeclarationSyntax syntax)
    {
        var usings = syntax
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name};")
            .Distinct()
            .ToList() ?? [];

        usings.Add("using DomainSmith.Abstraction.Core.Primitives;");

        return usings;
    }

    public static List<(AttributeSyntax Attr, IMethodSymbol? Symbol)> GetAllAttributes(
        this TypeDeclarationSyntax syntax, GeneratorSyntaxContext context) =>
        syntax.AttributeLists
            .SelectMany(list => list.Attributes)
            .Select(attr => (
                Attr: attr,
                Symbol: context.SemanticModel.GetSymbolInfo(attr).Symbol as IMethodSymbol
            ))
            .ToList();
}