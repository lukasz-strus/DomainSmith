using DomainSmith.Abstraction.Extensions;
using DomainSmith.AggregateRoot.Generators.Models;
using DomainSmith.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.AggregateRoot.Generators.Extensions;

public static class ClassDeclarationSyntaxExtensions
{
    public static List<EntityCollectionInfo> ExtractEntityCollections(
        this ClassDeclarationSyntax classSyntax,
        GeneratorSyntaxContext context)
    {
        var existingReadOnlyCollectionElements = classSyntax.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(p => context.SemanticModel.GetTypeInfo(p.Type).Type as INamedTypeSymbol)
            .Where(t => t is not null && t.IsGenericType)
            .Where(t => t!.ConstructedFrom.ToDisplayString() == "System.Collections.Generic.IReadOnlyCollection<T>")
            .Select(t => t!.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
            .ToList();

        var fields = classSyntax.Members
            .OfType<FieldDeclarationSyntax>()
            .SelectMany(f => f.Declaration.Variables.Select(v => (Field: f, Var: v)))
            .Select(x =>
            {
                var fieldType = context.SemanticModel.GetTypeInfo(x.Field.Declaration.Type).Type;
                return (x.Field, x.Var, FieldType: fieldType);
            })
            .ToList();

        var hashSetFields = fields
            .Select(x => new
            {
                FieldName = x.Var.Identifier.Text,
                FieldSyntax = x.Field,
                FieldType = x.FieldType as INamedTypeSymbol
            })
            .Where(x =>
                x.FieldType is { IsGenericType: true } &&
                x.FieldType.ConstructedFrom.ToDisplayString() == "System.Collections.Generic.HashSet<T>")
            .Where(x =>
            {
                var fieldSymbol =
                    context.SemanticModel.GetDeclaredSymbol(x.FieldSyntax.Declaration.Variables[0]) as IFieldSymbol;
                return fieldSymbol is null || !fieldSymbol.HasExcludeFromGenerationAttribute();
            })
            .Select(x => new
            {
                x.FieldName,
                ElementType = x.FieldType!.TypeArguments[0] as INamedTypeSymbol
            })
            .Where(x => x.ElementType is not null)
            .ToList();

        var collections = new List<EntityCollectionInfo>();

        foreach (var field in hashSetFields)
        {
            var elementType = field.ElementType!;
            var elementTypeName = elementType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

            var entityIdType = TryGetEntityIdType(elementType);
            if (string.IsNullOrWhiteSpace(entityIdType))
                continue;

            var propertyName = field.FieldName.TrimUnderscore().ToPascalPlural();
            var shouldGenerateProperty = !existingReadOnlyCollectionElements.Contains(elementTypeName);

            var entityArgs = elementType.GetEntityCtorArgsFromPublicProperties();

            var elementNoResultPattern = elementType.HasNoResultPatternAttribute();
            var elementIsResultPattern = !elementNoResultPattern;

            if (entityIdType != null)
                collections.Add(new EntityCollectionInfo(
                    propertyName,
                    field.FieldName,
                    elementTypeName,
                    entityIdType,
                    shouldGenerateProperty,
                    entityArgs,
                    elementIsResultPattern
                ));
        }

        return collections;
    }

    private static string? TryGetEntityIdType(INamedTypeSymbol entityTypeSymbol)
    {
        var entityAttr = entityTypeSymbol.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == typeof(EntityAttribute).FullName);

        if (entityAttr is null || entityAttr.ConstructorArguments.Length < 1)
            return null;

        var arg = entityAttr.ConstructorArguments[0];

        if (arg.Kind != TypedConstantKind.Type || arg.Value is not INamedTypeSymbol idTypeSym)
            return null;

        return idTypeSym.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
    }
}