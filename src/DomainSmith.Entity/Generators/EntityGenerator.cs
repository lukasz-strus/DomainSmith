using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DomainSmith.Abstraction.Generators;

namespace DomainSmith.Entity.Generators;

[Generator]
internal sealed class EntityGenerator : BaseGenerator<ClassDeclarationSyntax, EntityGenerator.ClassToAugment>
{
    protected override string AttributeFullName => typeof(EntityAttribute).FullName!;

    protected override string GenerateSource(ClassToAugment info)
    {
        var builder = new EntityBuilder();

        builder.SetUsings(info.Usings);
        builder.SetNamespace(info.Namespace);
        builder.SetClassName(info.Name);
        builder.SetTypeAgr(info.TypeArg);
        builder.SetIdValue(GetIdValueExpression(info.TypeArg));
        builder.SetExtensionName(info.Name);
        builder.SetExtensionReference(info.Namespace, info.Name);

        var source = builder.Build();
        builder.Clear();

        return source;
    }

    private static string GetIdValueExpression(string typeArg)
    {
        return typeArg switch
        {
            "int" => "new Random().Next()",
            "Guid" => "Guid.NewGuid()",
            "string" => "Guid.NewGuid().ToString()",
            _ => "default"
        };
    }

    protected override ClassToAugment CreateInfo(ClassDeclarationSyntax classSyntax, GeneratorSyntaxContext context)
    {
        var name = classSyntax.Identifier.Text;
        var ns = classSyntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString() ??
                 classSyntax.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

        var typeArg = "int";
        foreach (var attr in classSyntax.AttributeLists
                     .SelectMany(attrList => attrList.Attributes, (attrList, attr) => new { attrList, attr })
                     .Select(@t => new
                     {
                         @t, symbol = context.SemanticModel.GetSymbolInfo(@t.attr).Symbol as IMethodSymbol
                     })
                     .Where(@t => @t.symbol?.ContainingType.ToDisplayString() == AttributeFullName)
                     .Select(@t => @t.@t.attr))
        {
            if (!(attr.ArgumentList?.Arguments.Count > 0)) continue;
            var expr = attr.ArgumentList.Arguments[0].Expression;
            if (expr is TypeOfExpressionSyntax typeOfExpr)
            {
                typeArg = typeOfExpr.Type.ToString();
            }
        }

        var usings = classSyntax
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name};")
            .Distinct()
            .ToList() ?? [];

        usings.Add("using DomainSmith.Abstraction.Core.Primitives;");

        return new ClassToAugment(name, typeArg, ns, usings);
    }

    public sealed class ClassToAugment(string name, string typeArg, string? ns, List<string> usings)
    {
        public string Name { get; } = name;
        public string TypeArg { get; } = typeArg;
        public string? Namespace { get; } = ns;
        public List<string> Usings { get; } = usings;
    }
}