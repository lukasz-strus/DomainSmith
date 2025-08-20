﻿using DomainSmith.Entity.Generators;
using DomainSmith.Tests.Helpers.Creators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DomainSmith.Entity.Tests.Generators;

public sealed class EntityGeneratorTests
{
    #region Properties

    [Fact]
    public async Task EntityGenerator_WithStringProperty_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithStringProperty);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithStringProperty =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(Guid))]
        public sealed partial class TestEntity
        {
            public string Name { get; private set; }
        }
        """;

    [Fact]
    public async Task EntityGenerator_WithExcludedStringProperty_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithExcludedStringProperty);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithExcludedStringProperty =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(Guid))]
        public sealed partial class TestEntity
        {
            [ExcludeFromGeneration] public string Name { get; private set; }
            
            public string Name2 { get; private set; }
        }
        """;


    [Fact]
    public async Task EntityGenerator_WithIntProperty_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithIntProperty);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithIntProperty =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(Guid))]
        public sealed partial class TestEntity
        {
            public int Counter { get; private set; }
        }
        """;

    #endregion

    #region EntityId

    [Fact]
    public async Task EntityGenerator_WithShortId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithShortId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithShortId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(short))]
        public sealed partial class TestEntity
        {

        }
        """;


    [Fact]
    public async Task EntityGenerator_WithValueShortId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueShortId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueShortId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(short Value) : EntityIdRecord<short>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithUShortId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithUShortId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithUShortId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(ushort))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueUShortId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueUShortId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueUShortId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(ushort Value) : EntityIdRecord<ushort>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithIntId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithIntId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithIntId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(int))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueIntId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueIntId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueIntId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(int Value) : EntityIdRecord<int>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithUIntId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithUIntId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithUIntId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(uint))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueUIntId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueUIntId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueUIntId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(uint Value) : EntityIdRecord<uint>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithLongId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithLongId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithLongId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(long))]
        public sealed partial class TestEntity
        {

        }
        """;


    [Fact]
    public async Task EntityGenerator_WithValueLongId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueLongId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueLongId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(long Value) : EntityIdRecord<long>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithULongId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithULongId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithULongId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(ulong))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueULongId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueULongId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueULongId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(ulong Value) : EntityIdRecord<ulong>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithInt128Id_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithInt128Id);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithInt128Id =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(Int128))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueInt128Id_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueInt128Id);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueInt128Id =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(Int128 Value) : EntityIdRecord<Int128>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithUInt128Id_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithUInt128Id);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithUInt128Id =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(UInt128))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueUInt128Id_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueUInt128Id);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueUInt128Id =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(UInt128 Value) : EntityIdRecord<UInt128>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithGuidId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithGuidId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }


    private const string InputSourceWithGuidId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(Guid))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueGuidId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueGuidId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueGuidId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(Guid Value) : EntityIdRecord<Guid>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithStringId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithStringId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithStringId =
        """
        using DomainSmith.Entity;

        namespace TestNamespace;

        [Entity(typeof(string))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithValueStringId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithValueStringId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithValueStringId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(string Value) : EntityIdRecord<string>(Value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    [Fact]
    public async Task EntityGenerator_WithClassValueStringId_ShouldGenerateCode()
    {
        // Arrange
        var inputCompilation = CompilationCreator.CreateCompilation(InputSourceWithClassValueStringId);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntityGenerator());

        // Act
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out _);
        var output = outputCompilation.SyntaxTrees.Last().ToString();

        // Assert
        await Verify(output);
    }

    private const string InputSourceWithClassValueStringId =
        """
        using DomainSmith.Entity;
        using DomainSmith.Abstraction.Core.Primitives;

        namespace TestNamespace;

        public sealed record TestEntityId(string value) : EntityIdClass<string>(value);

        [Entity(typeof(TestEntityId))]
        public sealed partial class TestEntity
        {

        }
        """;

    #endregion
}