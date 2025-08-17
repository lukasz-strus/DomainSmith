using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace DomainSmith.Abstraction.Generators;

internal abstract class BaseBuilder
{
    private static readonly List<string> StandardUsings =
    [
        "using System;",
        "using System.Collections.Generic;",
        "using System.IO;",
        "using System.Linq;",
        "using System.Net.Http;",
        "using System.Threading;",
        "using System.Threading.Tasks;",
        "using System.Collections.Generic;",
        "using System.Text.Json;"
    ];

    protected StringBuilder Usings { get; set; } = new();
    protected StringBuilder Namespace { get; set; } = new();
    protected StringBuilder ClassName { get; set; } = new();

    internal void SetUsings(List<string> usings)
    {
        var usingsSet = new HashSet<string>(usings);
        foreach (var systemUsing in StandardUsings.Where(systemUsing => !usingsSet.Contains(systemUsing)))
        {
            usingsSet.Add(systemUsing);
        }

        Usings.Clear();
        Usings.Append(string.Join("\n", usingsSet));
    }

    internal void SetNamespace(string? @namespace)
    {
        Namespace.Clear();
        Namespace.Append(string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"namespace {@namespace};");
    }

    internal void SetClassName(string className)
    {
        ClassName.Clear();
        ClassName.Append(className);
    }

    internal virtual void Clear()
    {
        Usings.Clear();
        Namespace.Clear();
        ClassName.Clear();
    }

    internal string Build() => IsEmpty() ? string.Empty : BuildSource();

    protected abstract bool IsEmpty();

    protected abstract string BuildSource();
}