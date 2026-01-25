using System;

namespace DomainSmith.Abstraction.Extensions;

public static class StringExtensions
{
    public static string ToCamel(this string s) =>
        string.IsNullOrWhiteSpace(s) ? s : char.ToLowerInvariant(s[0]) + s.Substring(1);

    public static string ToPascalPlural(this string s) =>
        string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);

    public static string TrimUnderscore(this string s) =>
        s.StartsWith("_", StringComparison.Ordinal) ? s.Substring(1, s.Length - 1) : s;
}