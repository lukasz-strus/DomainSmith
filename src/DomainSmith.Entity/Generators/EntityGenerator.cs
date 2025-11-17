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
        builder.SetIdValue(GetIdValueExpression(info));
        builder.SetExtensionName(info.Name);
        builder.SetProperties(info.Properties);

        var source = builder.Build();
        builder.Clear();

        return source;
    }

    private static string GetIdValueExpression(ClassToAugment info)
    {
        var valueExpr = GetGeneratingExpression(info.TypeArg);

        if (valueExpr is not null)
            return valueExpr;

        if (info is { IsEntityIdRecord: false, IsEntityIdClass: false } || string.IsNullOrEmpty(info.IdValueType))
            return "default";

        valueExpr = GetGeneratingExpression(info.IdValueType);

        return $"new {info.TypeArg}({valueExpr})";
    }

    private static string? GetGeneratingExpression(string? valueType) =>
        valueType switch
        {
            "short" or "Int16" or "System.Int16" => "(short)new Random().Next(short.MinValue, short.MaxValue)",
            "ushort" or "UInt16" or "System.UInt16" => "(ushort)new Random().Next(0, short.MaxValue)",
            "int" or "Int32" or "System.Int32" => "new Random().Next()",
            "uint" or "UInt32" or "System.UInt32" => "(uint)new Random().Next(0, int.MaxValue)",
            "long" or "Int64" or "System.Int64" => "(long)new Random().Next()",
            "ulong" or "UInt64" or "System.UInt64" => "(ulong)new Random().Next(0, int.MaxValue)",
            "Int128" or "System.Int128" => "(Int128)new Random().Next()",
            "UInt128" or "System.UInt128" => "(UInt128)new Random().Next(0, int.MaxValue)",
            "Guid" or "System.Guid" => "Guid.NewGuid()",
            "string" or "String" or "System.String" => "Guid.NewGuid().ToString()",
            _ => null
        };

    protected override ClassToAugment? CreateInfo(ClassDeclarationSyntax classSyntax, GeneratorSyntaxContext context)
    {
        var name = classSyntax.Identifier.Text;
        var ns = classSyntax.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
                 ?? classSyntax.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();

        var usings = classSyntax
            .FirstAncestorOrSelf<CompilationUnitSyntax>()?
            .DescendantNodesAndSelf()
            .OfType<UsingDirectiveSyntax>()
            .Select(x => $"using {x.Name};")
            .Distinct()
            .ToList() ?? [];

        usings.Add("using DomainSmith.Abstraction.Core.Primitives;");


        var attributeSyntaxes = classSyntax.AttributeLists
            .SelectMany(list => list.Attributes)
            .Select(attr => new
            {
                attr,
                symbol = context.SemanticModel.GetSymbolInfo(attr).Symbol as IMethodSymbol
            })
            .Where(x => x.symbol?.ContainingType.ToDisplayString() == AttributeFullName)
            .Select(x => x.attr);

        var typeArg = "int";
        INamedTypeSymbol? idTypeSymbol = null;
        foreach (var attr in attributeSyntaxes)
        {
            if (!(attr.ArgumentList?.Arguments.Count > 0)
                || attr.ArgumentList.Arguments[0].Expression is not TypeOfExpressionSyntax typeOfExpr)
                continue;

            typeArg = typeOfExpr.Type.ToString();
            idTypeSymbol = context.SemanticModel.GetTypeInfo(typeOfExpr.Type).Type as INamedTypeSymbol;
            break;
        }

        if (idTypeSymbol == null) return null;

        var isEntityIdRecord = false;
        var isEntityIdClass = false;
        string? idValueType = null;
        var idTypeFullName = idTypeSymbol.ToDisplayString();
        for (var baseType = idTypeSymbol.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            var baseName = baseType.ConstructedFrom.ToDisplayString();
            if (baseName == "DomainSmith.Abstraction.Core.Primitives.EntityIdRecord<T>")
            {
                isEntityIdRecord = true;
                idValueType = baseType.TypeArguments.FirstOrDefault()?.ToDisplayString();
                break;
            }

            if (baseName == "DomainSmith.Abstraction.Core.Primitives.EntityIdClass<T>")
            {
                isEntityIdClass = true;
                idValueType = baseType.TypeArguments.FirstOrDefault()?.ToDisplayString();
                break;
            }
        }

        var properties = classSyntax.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(p =>
            {
                var symbol = context.SemanticModel.GetDeclaredSymbol(p);
                return symbol is not null &&
                       !symbol.GetAttributes().Any(a =>
                           a.AttributeClass?.ToDisplayString() == typeof(ExcludeFromGenerationAttribute).FullName!);
            })
            .Select(p =>
            {
                var symbol = context.SemanticModel.GetDeclaredSymbol(p);
                var isAutoGenerated = symbol?.GetAttributes().Any(a =>
                    a.AttributeClass?.ToDisplayString() == typeof(AutoGeneratedAttribute).FullName!) == true;
                return new PropertyInfo(
                    p.Type.ToString(),
                    p.Identifier.Text,
                    isAutoGenerated
                );
            })
            .ToList();

        return new ClassToAugment(
            name,
            typeArg,
            ns,
            usings,
            idTypeFullName,
            isEntityIdRecord,
            isEntityIdClass,
            idValueType,
            properties
        );
    }

    public sealed class ClassToAugment(
        string name,
        string typeArg,
        string? ns,
        List<string> usings,
        string? idTypeFullName,
        bool isEntityIdRecord,
        bool isEntityIdClass,
        string? idValueType,
        List<PropertyInfo> properties)
    {
        public string Name { get; } = name;
        public string TypeArg { get; } = typeArg;
        public string? Namespace { get; } = ns;
        public List<string> Usings { get; } = usings;
        public string? IdTypeFullName { get; } = idTypeFullName;
        public bool IsEntityIdRecord { get; } = isEntityIdRecord;
        public bool IsEntityIdClass { get; } = isEntityIdClass;
        public string? IdValueType { get; } = idValueType;
        public List<PropertyInfo> Properties { get; } = properties;
    }

    public sealed class PropertyInfo(string type, string name, bool autoGenerated)
    {
        public string Type { get; } = type;
        public string Name { get; } = name;

        public bool AutoGenerated { get; set; } = autoGenerated;
    }
}