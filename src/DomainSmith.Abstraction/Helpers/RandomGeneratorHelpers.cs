namespace DomainSmith.Abstraction.Helpers;

internal static class RandomGeneratorHelpers
{
    internal static string? ToGeneratingExpression(this string? valueType) =>
        valueType switch
        {
            "short" or "Int16" or "System.Int16" => "(short)new Random().Next(short.MinValue, short.MaxValue)",
            "ushort" or "UInt16" or "System.UInt16" => "(ushort)new Random().Next(0, short.MaxValue)",
            "int" or "Int32" or "System.Int32" => "new Random().Next()",
            "uint" or "UInt32" or "System.UInt32" => "(uint)new Random().Next(0, int.MaxValue)",
            "long" or "Int64" or "System.Int64" => "(long)new Random().Next()",
            "ulong" or "UInt64" or "System.UInt64" => "(ulong)new Random().Next(0, int.MaxValue)",
            "Int128" or "System.Int128" => "(Int128)new Random().Next()",
            "UInt128" or "System.UInt128" => "(UInt128)new Random().Next(0, int.MaxValue)",
            "Guid" or "System.Guid" => "Guid.NewGuid()",
            "string" or "String" or "System.String" => "Guid.NewGuid().ToString()",
            _ => null
        };
}