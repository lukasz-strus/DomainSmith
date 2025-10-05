using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DomainSmith.Abstraction.Generators;

internal abstract class BaseGenerator<TSyntax, TInfo> : IIncrementalGenerator
    where TSyntax : MemberDeclarationSyntax
    where TInfo : class
{
    protected abstract string AttributeFullName { get; }
    protected abstract string GenerateSource(TInfo info);
    protected abstract TInfo? CreateInfo(TSyntax syntax, GeneratorSyntaxContext context);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        // if (!Debugger.IsAttached) Debugger.Launch();
#endif
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

        var methodSymbols = syntax.AttributeLists.SelectMany(
            attributeList => attributeList.Attributes,
            (_, attribute) => context.SemanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol);

        if (methodSymbols.All(symbol => symbol?.ContainingType.ToDisplayString() != attributeName))
            return null;

        return CreateInfo(syntax, context);
    }

    protected virtual string GetFileName(object info) =>
        info.GetType().GetProperty("Name")?.GetValue(info)?.ToString() ?? "Generated";
}