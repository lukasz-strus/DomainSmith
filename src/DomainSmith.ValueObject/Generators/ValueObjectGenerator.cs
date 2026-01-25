using DomainSmith.Abstraction.Extensions;
using DomainSmith.Abstraction.Generators;
using DomainSmith.ValueObject.Generators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.ValueObject.Generators;

[Generator]
internal sealed class ValueObjectGenerator : BaseGenerator<MemberDeclarationSyntax, ClassToAugment>
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
        builder.SetIsResultPattern(!info.NoResultPattern);

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

        var name = syntax.GetName();
        var ns = syntax.GetNamespace();
        var usings = syntax.GetUsings();
        var allAttributes = syntax.GetAllAttributes(context);
        var properties = syntax.GetAugmentableProperties(context);
        var noResultPattern = allAttributes.HasNoResultPattern(context);

        return new ClassToAugment(name, ns, usings, properties, noResultPattern);
    }
}