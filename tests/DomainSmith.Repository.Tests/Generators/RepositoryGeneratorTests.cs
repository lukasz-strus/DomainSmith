using DomainSmith.AggregateRoot.Generators;
using DomainSmith.Repository.Generators;
using DomainSmith.Tests.Helpers.Creators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DomainSmith.Repository.Tests.Generators;

public sealed class RepositoryGeneratorTests
{
    [Fact]
    public async Task RepositoryGenerator_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSource);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new AggregateRootGenerator(), new RepositoryGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSource =
        """
        using DomainSmith.AggregateRoot;
        using DomainSmith.Abstraction.Common;

        namespace TestNamespace;

        [AggregateRoot(typeof(Guid))]
        public partial class Owner
        {
            public string FirstName { get; private set; }
            public string LastName { get; private set; }
            public string Email { get; private set; }
        }
        """;
}