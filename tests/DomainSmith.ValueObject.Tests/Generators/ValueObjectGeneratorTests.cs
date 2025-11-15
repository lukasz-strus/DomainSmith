using DomainSmith.Tests.Helpers.Creators;
using DomainSmith.ValueObject.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DomainSmith.ValueObject.Tests.Generators;

public sealed class ValueObjectGeneratorTests
{
    [Fact]
    public async Task ValueObjectGenerator_WithMoneyRecord_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceMoneyRecord);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new ValueObjectGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceMoneyRecord =
        """
        using DomainSmith.ValueObject;

        namespace TestNamespace;

        [ValueObject]
        public partial record Money
        {
            public decimal Amount { get; init; }
            public string Currency { get; init; } = "USD";
        }
        """;

    [Fact]
    public async Task ValueObjectGenerator_WithMoneyClass_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceMoneyClass);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new ValueObjectGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceMoneyClass =
        """
        using DomainSmith.ValueObject;

        namespace TestNamespace;

        [ValueObject]
        public partial class Money
        {
            public decimal Amount { get; init; }
            public string Currency { get; init; } = "USD";
        }
        """;
}
