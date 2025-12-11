using DomainSmith.Abstraction.Generators;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Generators;

public sealed class BaseBuilderTests
{
    private sealed class TestBuilder : BaseBuilder
    {
        public string UsingsText => Usings.ToString();
        public string NamespaceText => Namespace.ToString();
        public string ClassNameText => ClassName.ToString();

        protected override bool IsEmpty() =>
            Usings.Length == 0 || Namespace.Length == 0 || ClassName.Length == 0;

        protected override string BuildSource() => $"// built {ClassName}";
    }

    [Fact]
    public void SetUsings_ShouldMergeWithStandardUsings_WithoutDuplicates()
    {
        // Arrange
        var builder = new TestBuilder();
        var customUsings = new List<string>
        {
            "using System;",
            "using MyApp.Core;",
            "using System.Collections.Generic;"
        };

        // Act
        builder.SetUsings(customUsings);

        // Assert
        var output = builder.UsingsText;
        output.Should().NotBeNullOrWhiteSpace();
        output.Should().Contain("using MyApp.Core;");
        output.Should().Contain("using System;");
        output.Should().Contain("using System.Text.Json;");
        output.Should().Contain("using System.Net.Http;");
        output.Split('\n').Should().Contain(new[]
        {
            "using System.Collections.Generic;"
        });
        output.Split('\n').Should().ContainSingle(s => s == "using System.Collections.Generic;");
    }

    [Fact]
    public void SetNamespace_ShouldFormatOrClearNamespace()
    {
        // Arrange
        var builder = new TestBuilder();

        builder.SetNamespace(null);
        builder.NamespaceText.Should().BeEmpty();

        builder.SetNamespace(string.Empty);
        builder.NamespaceText.Should().BeEmpty();

        builder.SetNamespace("   ");
        builder.NamespaceText.Should().BeEmpty();

        builder.SetNamespace("Test.Namespace");
        builder.NamespaceText.Should().Be("namespace Test.Namespace;");
    }

    [Fact]
    public void SetClassName_ShouldStoreName()
    {
        // Arrange
        var builder = new TestBuilder();

        // Act
        builder.SetClassName("MyType");

        // Assert
        builder.ClassNameText.Should().Be("MyType");
    }

    [Fact]
    public void Build_WhenEmpty_ShouldReturnEmptyString()
    {
        // Arrange
        var builder = new TestBuilder();

        // Act
        var output = builder.Build();

        // Assert
        output.Should().BeEmpty();
    }

    [Fact]
    public void Build_WhenReady_ShouldReturnBuiltSource()
    {
        // Arrange
        var builder = new TestBuilder();
        builder.SetUsings(new List<string> { "using System;" });
        builder.SetNamespace("Test.Namespace");
        builder.SetClassName("ReadyType");

        // Act
        var output = builder.Build();

        // Assert
        output.Should().Be("// built ReadyType");
    }

    [Fact]
    public void Clear_ShouldResetAllBuffers()
    {
        // Arrange
        var builder = new TestBuilder();
        builder.SetUsings(new List<string> { "using System;" });
        builder.SetNamespace("Test.Namespace");
        builder.SetClassName("MyType");

        // Act
        builder.Clear();

        // Assert
        builder.UsingsText.Should().BeEmpty();
        builder.NamespaceText.Should().BeEmpty();
        builder.ClassNameText.Should().BeEmpty();
    }
}