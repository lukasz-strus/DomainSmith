namespace DomainSmith.Abstraction.Common.Models;

public sealed class ParameterInfo(string type, string name)
{
    public string Type { get; } = type;
    public string Name { get; } = name;
}