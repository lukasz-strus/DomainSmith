using DomainSmith.Abstraction.Extensions;
using DomainSmith.Abstraction.Generators;
using DomainSmith.AggregateRoot;
using DomainSmith.Repository.Generators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.Repository.Generators;

[Generator]
internal sealed class RepositoryGenerator : BaseGenerator<ClassDeclarationSyntax, ClassToAugment>
{
    protected override string AttributeFullName => typeof(AggregateRootAttribute).FullName!;

    protected override string GenerateSource(ClassToAugment info)
    {
        var builder = new RepositoryBuilder();

        builder.SetUsings(info.Usings);
        builder.SetNamespace(info.Namespace);
        builder.SetClassName(info.Name);
        builder.SetIdType(info.TypeArg);

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
        var aggregateRootAttributes = allAttributes.GetAttributesByFullName(AttributeFullName);

        if (!aggregateRootAttributes.TryGetIdType(context, out var typeArg, out var idTypeSymbol))
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

    protected override string GetFileName(object info) =>
        info is ClassToAugment x
            ? $"I{x.Name}Repository"
            : base.GetFileName(info);
}