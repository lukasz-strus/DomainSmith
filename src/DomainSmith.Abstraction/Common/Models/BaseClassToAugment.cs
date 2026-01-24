using DomainSmith.Abstraction.Helpers;
using System;
using System.Collections.Generic;

namespace DomainSmith.Abstraction.Common.Models;

public abstract class BaseClassToAugment(
    string name,
    string? ns,
    List<string> usings,
    List<PropertyInfo> properties,
    bool noResultPattern)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = ns;
    public List<string> Usings { get; } = usings;
    public List<PropertyInfo> Properties { get; } = properties;
    public bool NoResultPattern { get; } = noResultPattern;
}

public abstract class BaseArgClassToAugment(
    string name,
    string typeArg,
    string? ns,
    List<string> usings,
    bool isIdRecord,
    bool isIdClass,
    string? idValueType,
    List<PropertyInfo> properties,
    bool noResultPattern) : BaseClassToAugment(name, ns, usings, properties, noResultPattern)
{
    public string TypeArg { get; } = typeArg;
    public bool IsIdRecord { get; } = isIdRecord;
    public bool IsIdClass { get; } = isIdClass;
    public string? IdValueType { get; } = idValueType;

    public string GetIdValueExpression()
    {
        var valueExpr = TypeArg.ToGeneratingExpression();

        if (valueExpr is not null)
            return valueExpr;

        if (this is { IsIdRecord: false, IsIdClass: false } ||
            string.IsNullOrEmpty(IdValueType))
            return "default";

        valueExpr = IdValueType.ToGeneratingExpression();

        return $"new {TypeArg}({valueExpr})";
    }
}