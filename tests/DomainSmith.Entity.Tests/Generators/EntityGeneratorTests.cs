using DomainSmith.Entity.Generators;
using DomainSmith.Tests.Helpers.Creators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DomainSmith.Entity.Tests.Generators;

public sealed class EntityGeneratorTests
{
    [Fact]
    public async Task EntityGenerator_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSource =
        """
        using DomainSmith.Abstraction.Core.Primitives;
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(Guid))]
        public sealed partial class TestEntity
        {

        }
        """;
}