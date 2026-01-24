using System;

namespace DomainSmith.Abstraction.Extensions;

public static class StringExtensions
{
    extension(string s)
    {
        public string ToCamel() =>
            string.IsNullOrWhiteSpace(s) ? s : char.ToLowerInvariant(s[0]) + s.Substring(1);

        public string ToPascalPlural() =>
            string.IsNullOrWhiteSpace(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);

        public string TrimUnderscore() =>
            s.StartsWith("_", StringComparison.Ordinal) ? s.Substring(1, s.Length - 1) : s;
    }
}