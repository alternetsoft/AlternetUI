using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class IdentifierHelpers
{
    // A conservative list of C# reserved keywords (hard keywords).
    // Contextual keywords are not included here because they are valid identifiers in many contexts.
    // This list covers the common reserved words that cannot be used as identifiers unless escaped with '@'.
    private static readonly HashSet<string> ReservedKeywords = new(StringComparer.Ordinal)
    {
        "abstract","as","base","bool","break","byte","case","catch","checked","char","class","const",
        "continue","decimal","default","delegate","do","double","else","enum","event","explicit","extern",
        "false","finally","fixed","for","foreach","goto","if","implicit","in","interface","internal","is",
        "lock","long","namespace","new","null","object","operator","out","override","params","private",
        "protected","public","readonly","ref","return","sbyte","sealed","short","sizeof","stackalloc",
        "static","string","struct","switch","this","throw","true","try","typeof","uint","ulong","unchecked",
        "unsafe","ushort","using","virtual","void","volatile","while"
    };

    // Regex that matches a single C# identifier (without @ prefix).
    // Start: letter or underscore. Continuation: letter, digit, connecting punctuation, combining marks, formatting chars.
    // Uses Unicode categories to be permissive and spec-compliant.
    private static readonly Regex IdentifierRegex = new(
        @"^[_\p{L}][_\p{L}\p{Nd}\p{Mn}\p{Mc}\p{Pc}\p{Cf}]*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IsFirstCharUpperAZ(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return false;
        char firstChar = s![0];
        return (firstChar >= 'A' && firstChar <= 'Z');
    }

    /// <summary>
    /// Returns true if <paramref name="name"/> is a valid C# method identifier (or explicit-interface-qualified
    /// method name when <paramref name="allowExplicitInterfaceImplementation"/> is true).
    /// Rules enforced:
    /// - non-empty
    /// - optional leading '@' escape is supported (e.g. @class is valid)
    /// - respects Unicode identifier rules
    /// - does not equal a C# reserved keyword unless escaped with '@'
    /// - if <paramref name="allowExplicitInterfaceImplementation"/> is true, names like "IFoo.Bar" are allowed
    ///   (each part must be a valid identifier or an @-escaped identifier).
    /// </summary>
    public static bool IsValidCSharpMethodName(string? name, bool allowExplicitInterfaceImplementation = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // If explicit interface implementations allowed, accept exactly one '.' that separates interface and member
        if (allowExplicitInterfaceImplementation && name?.IndexOf('.') is int dot && dot > 0)
        {
            var parts = name.Split('.');
            // require exactly two parts (interface and member) or more? commonly it's "IInterface.Method".
            // we'll require at least two parts and validate each part as identifier (allow multiple dots for nested types).
            foreach (var part in parts)
            {
                if (!IsIdentifierOrEscaped(part))
                    return false;
            }
            return true;
        }

        // Otherwise it must be a single identifier (possibly @-escaped)
        return IsIdentifierOrEscaped(name);
    }

    private static bool IsIdentifierOrEscaped(string? s)
    {
        if (string.IsNullOrEmpty(s))
            return false;

        // @-prefixed identifiers (verbatim identifiers) are allowed: they bypass the reserved-keyword check,
        // but the remainder must still be a valid identifier.
        if (s![0] == '@')
        {
            var rest = s.Substring(1);
            if (string.IsNullOrEmpty(rest))
                return false; // "@" alone is not a valid identifier
            return IdentifierRegex.IsMatch(rest);
        }

        // Not escaped: must match identifier syntax and must not be a reserved keyword
        if (!IdentifierRegex.IsMatch(s))
            return false;

        if (ReservedKeywords.Contains(s))
            return false;

        return true;
    }
}