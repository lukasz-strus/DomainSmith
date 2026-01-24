using System.Collections.Generic;
using System.Linq;
using DomainSmith.Abstraction.Common.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.Abstraction.Extensions;

public static class TypeDeclarationSyntaxExtensions
{
    extension(TypeDeclarationSyntax syntax)
    {
        public List<PropertyInfo> GetAugmentableProperties(GeneratorSyntaxContext context) =>
            syntax.Members
                .OfType<PropertyDeclarationSyntax>()
                .Where(p => !p.IsExcludedFromGeneration(context))
                .Select(p => p.ToPropertyInfo(context))
                .ToList();

        public string GetName()
            => syntax.Identifier.Text;

        public string? GetNamespace()
            => syntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
               ?? syntax.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

        public List<string> GetUsings()
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

        public List<(AttributeSyntax Attr, IMethodSymbol? Symbol)> GetAllAttributes(GeneratorSyntaxContext context) =>
            syntax.AttributeLists
                .SelectMany(list => list.Attributes)
                .Select(attr => (
                    Attr: attr,
                    Symbol: context.SemanticModel.GetSymbolInfo(attr).Symbol as IMethodSymbol
                ))
                .ToList();
    }
}