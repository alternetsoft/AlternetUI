#pragma warning disable
#nullable disable
using System;
using System.Globalization;

namespace Alternet.UI.Utilities
{
    static class IdentifierParser
    {
        public static ReadOnlySpan<char> ParseIdentifier(this ref CharacterReader r)
        {
            if (IsValidIdentifierStart(r.Peek))
            {
                return r.TakeWhile(c => IsValidIdentifierChar(c));
            }
            else
            {
                return ReadOnlySpan<char>.Empty;
            }
        }

        private static bool IsValidIdentifierStart(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        private static bool IsValidIdentifierChar(char c)
        {
            if (IsValidIdentifierStart(c))
            {
                return true;
            }
            else
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                return cat == UnicodeCategory.NonSpacingMark ||
                       cat == UnicodeCategory.SpacingCombiningMark ||
                       cat == UnicodeCategory.ConnectorPunctuation ||
                       cat == UnicodeCategory.Format ||
                       cat == UnicodeCategory.DecimalDigitNumber;
            }
        }
    }
}
