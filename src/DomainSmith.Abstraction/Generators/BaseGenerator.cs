using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.Abstraction.Generators;

public abstract class BaseGenerator<TSyntax, TInfo> : IIncrementalGenerator
    where TSyntax : MemberDeclarationSyntax
    where TInfo : class
{
    protected abstract string AttributeFullName { get; }
    protected abstract string GenerateSource(TInfo info);
    protected abstract TInfo? CreateInfo(TSyntax syntax, GeneratorSyntaxContext context);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var declarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is TSyntax { AttributeLists.Count: > 0 },
                transform: (ctx, _) => GetSemanticTargetForGeneration(ctx, AttributeFullName))
            .Where(m => m is not null);

        context.RegisterSourceOutput(declarations, (spc, info) =>
        {
            if (info is null) return;
            var source = GenerateSource(info);
            spc.AddSource($"{GetFileName(info)}.g.cs", source);
        });
    }

    private TInfo? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, string attributeName)
    {
        var syntax = (TSyntax)context.Node;

        return syntax.AttributeLists.SelectMany(
                attributeList => attributeList.Attributes,
                (_, attribute) => context.SemanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol)
            .All(symbol => symbol?.ContainingType.ToDisplayString() != attributeName) ? null : CreateInfo(syntax, context);
    }

    protected virtual string GetFileName(object info) => 
        info.GetType().GetProperty("Name")?.GetValue(info)?.ToString() ?? "Generated";
}