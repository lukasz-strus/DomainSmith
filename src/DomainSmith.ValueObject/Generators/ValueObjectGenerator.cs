using DomainSmith.Abstraction.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.ValueObject.Generators;

[Generator]
internal sealed class ValueObjectGenerator : BaseGenerator<MemberDeclarationSyntax, ValueObjectGenerator.ClassToAugment>
{
    protected override string AttributeFullName => typeof(ValueObjectAttribute).FullName!;

    protected override string GenerateSource(ClassToAugment info)
    {
        var builder = new ValueObjectBuilder();

        builder.SetUsings(info.Usings);
        builder.SetNamespace(info.Namespace);
        builder.SetClassName(info.Name);
        builder.SetExtensionName(info.Name);
        builder.SetExtensionReference(info.Namespace, info.Name);
        builder.SetProperties(info.Properties);

        var source = builder.Build();
        builder.Clear();

        return source;
    }

    protected override ClassToAugment? CreateInfo(MemberDeclarationSyntax declaration, GeneratorSyntaxContext context)
    {
        TypeDeclarationSyntax? syntax = declaration switch
        {
            ClassDeclarationSyntax classSyntax => classSyntax,
            RecordDeclarationSyntax recordSyntax => recordSyntax,
            _ => null
        };

        if (syntax is null) return null;

        var name = syntax.Identifier.Text;

        var ns = syntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
                 ?? syntax.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

        var usings = syntax
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name};")
            .Distinct()
            .ToList() ?? [];

        var attributeSyntaxes = syntax.AttributeLists
            .SelectMany(list => list.Attributes)
            .Select(attr => new
            {
                attr,
                symbol = context.SemanticModel.GetSymbolInfo(attr).Symbol as IMethodSymbol
            })
            .Where(x => x.symbol?.ContainingType.ToDisplayString() == AttributeFullName)
            .Select(x => x.attr);

        if (!attributeSyntaxes.Any()) return null;

        var properties = syntax.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(p => context.SemanticModel.GetDeclaredSymbol(p) is not null)
            .Select(p => (Type: p.Type.ToString(), Name: p.Identifier.Text))
            .ToList();

        return new ClassToAugment(name, ns, usings, properties);
    }

    public sealed class ClassToAugment(
        string name,
        string? ns,
        List<string> usings,
        List<(string Type, string Name)> properties)
    {
        public string Name { get; } = name;
        public string? Namespace { get; } = ns;
        public List<string> Usings { get; } = usings;
        public List<(string Type, string Name)> Properties { get; } = properties;
    }
}