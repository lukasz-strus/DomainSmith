using DomainSmith.Abstraction.Generators;
using DomainSmith.Entity.Generators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DomainSmith.Abstraction.Extensions;

namespace DomainSmith.Entity.Generators;

[Generator]
internal sealed class EntityGenerator : BaseGenerator<ClassDeclarationSyntax, ClassToAugment>
{
    protected override string AttributeFullName => typeof(EntityAttribute).FullName!;

    protected override string GenerateSource(ClassToAugment info)
    {
        var builder = new EntityBuilder();

        builder.SetUsings(info.Usings);
        builder.SetNamespace(info.Namespace);
        builder.SetClassName(info.Name);
        builder.SetTypeAgr(info.TypeArg);
        builder.SetIdValue(info.GetIdValueExpression());
        builder.SetExtensionName(info.Name);
        builder.SetProperties(info.Properties);
        builder.SetIsResultPattern(!info.NoResultPattern);

        var source = builder.Build();
        builder.Clear();

        return source;
    }

    protected override ClassToAugment? CreateInfo(ClassDeclarationSyntax classSyntax, GeneratorSyntaxContext context)
    {
        var name = classSyntax.GetName();
        var ns = classSyntax.GetNamespace();
        var usings = classSyntax.GetUsings();
        var allAttributes = classSyntax.GetAllAttributes(context);
        var entityAttributes = allAttributes.GetAttributesByFullName(AttributeFullName);

        if (!entityAttributes.TryGetIdType(context, out var typeArg, out var idTypeSymbol))
            return null;

        if (idTypeSymbol == null)
            return null;

        var idMetadata = idTypeSymbol.GetIdMetadata();
        var properties = classSyntax.GetAugmentableProperties(context);
        var noResultPattern = allAttributes.HasNoResultPattern(context);

        return new ClassToAugment(
            name,
            typeArg,
            ns,
            usings,
            idMetadata.IsRecord,
            idMetadata.IsClass,
            idMetadata.ValueType,
            properties,
            noResultPattern
        );
    }
}