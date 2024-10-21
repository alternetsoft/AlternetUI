using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI.Extensions
{
    public static partial class ExtensionsPublic
    {
        /// <summary>
        /// Checks whether string has the specified prefix (position of the <paramref name="prefix"/>
        /// occurrence is at the beginning of the string).
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <param name="prefix">Prefix text to use for check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasPrefix(this string s, string prefix)
        {
            var index = s.IndexOf(prefix);
            return index == 0;
        }

        /// <summary>
        /// Checks whether string has the specified suffix (position of the <paramref name="suffix"/>
        /// occurrence is at the end of the string).
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <param name="suffix">Suffix text to use for check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSuffix(this string s, string suffix)
        {
            var index = s.LastIndexOf(suffix);
            var okIndex = s.Length - suffix.Length;
            if (index == okIndex)
                return true;
            return false;
        }

        /// <summary>
        /// Removes underscore characters ('_') from string.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveUnderscore(this string s)
        {
            return s.Replace("_", string.Empty);
        }

        /// <summary>
        /// Trims end of line characters from the string.
        /// </summary>
        /// <param name="s">String.</param>
        /// <returns>String without all end of line characters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimEndEol(this string s)
        {
            return s.TrimEnd('\r', '\n');
        }

        /// <summary>
        /// Reports whether the specified Unicode character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <param name="ch">A Unicode character to seek.</param>
        /// <returns>
        /// <c>true</c> if that character is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsChar(this string s, char ch)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return s.IndexOf(ch) >= 0;
        }

        /// <summary>
        /// Reports whether space character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if space is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsSpace(this string s)
        {
            return ContainsChar(s, ' ');
        }

        /// <summary>
        /// Reports whether semicolon character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if semicolon is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsSemicolon(this string s)
        {
            return ContainsChar(s, ';');
        }

        /// <summary>
        /// Reports whether dot character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if dot is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsDot(this string s)
        {
            return ContainsChar(s, '.');
        }

        /// <summary>
        /// Adds question character to the end of the string.
        /// </summary>
        /// <param name="s">Text string.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddQuestion(this string s)
        {
            return $"{s}?";
        }

        /// <summary>
        /// Adds character to the end of the string.
        /// </summary>
        /// <param name="s">Text string.</param>
        /// <param name="ch">Character.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AddChar(this string s, char ch)
        {
            return $"{s}{ch}";
        }

        /// <summary>
        /// Reports whether comma character is found in the string.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <returns>
        /// <c>true</c> if comma is found, or <c>false</c> if it is not.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsComma(this string s)
        {
            return ContainsChar(s, ',');
        }
    }
}
