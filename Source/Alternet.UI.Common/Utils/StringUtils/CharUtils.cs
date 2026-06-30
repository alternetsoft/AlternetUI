using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which are related to <see cref="char"/>.
    /// </summary>
    public static class CharUtils
    {
        /// <summary>Null character '\0'.</summary>
        public const char Null = '\0';

        /// <summary>
        /// Represents the backspace control character ('\b') as a constant value.
        /// This character is used to move the cursor back one position in text.
        /// </summary>
        public const char BackspaceChar = '\b';

        /// <summary>
        /// Represents the Unicode right arrow symbol (→) as a constant value.
        /// Unicode: U+2192
        /// </summary>
        public const char RightArrow = '\u2192';

        /// <summary>
        /// Represents the Unicode left arrow symbol (←) as a constant value.
        /// Unicode: U+2190
        /// </summary>
        public const char LeftArrow = '\u2190';

        /// <summary>
        /// Represents the backslash character ('\') as a constant value.
        /// </summary>
        public const char BackSlash = '\\';   // \

        /// <summary>
        /// Represents the forward slash ('/') character as a constant value.
        /// </summary>
        public const char Slash = '/';

        /// <summary>
        /// Represents the apostrophe (') character as a constant value.
        /// </summary>
        public const char Apostrophe = '\'';

        /// <summary>
        /// Represents the tab character ('\t') as a constant value.
        /// </summary>
        public const char Tab = '\t';

        /// <summary>Line feed / newline '\n'.</summary>
        public const char NewLine = '\n';

        /// <summary>Carriage return '\r'.</summary>
        public const char CarriageReturn = '\r';

        /// <summary>Vertical tab '\v'.</summary>
        public const char VerticalTab = '\v';

        /// <summary>Form feed '\f'.</summary>
        public const char FormFeed = '\f';

        /// <summary>Escape character (U+001B).</summary>
        public const char Escape = '\u001B';

        /// <summary>Unicode byte order mark (BOM, U+FEFF).</summary>
        public const char ByteOrderMark = '\uFEFF';

        /// <summary>Space ' '.</summary>
        public const char Space = ' ';

        /// <summary>Non-breaking space (U+00A0).</summary>
        public const char NonBreakingSpace = '\u00A0';

        /// <summary>Zero-width space (U+200B).</summary>
        public const char ZeroWidthSpace = '\u200B';

        /// <summary>Double quote '"' .</summary>
        public const char DoubleQuote = '"';

        /// <summary>Comma ','.</summary>
        public const char Comma = ',';

        /// <summary>Dot (period) '.'.</summary>
        public const char Dot = '.';

        /// <summary>Colon ':'.</summary>
        public const char Colon = ':';

        /// <summary>Semicolon ';'.</summary>
        public const char Semicolon = ';';

        /// <summary>Question mark '?'.</summary>
        public const char Question = '?';

        /// <summary>Exclamation mark '!'.</summary>
        public const char Exclamation = '!';

        /// <summary>Hyphen (minus) '-'.</summary>
        public const char HyphenMinus = '-';

        /// <summary>Underscore '_'.</summary>
        public const char Underscore = '_';

        /// <summary>Plus '+'.</summary>
        public const char Plus = '+';

        /// <summary>Equals '='.</summary>
        public const char EqualsChar = '=';

        /// <summary>Asterisk '*'.</summary>
        public const char Asterisk = '*';

        /// <summary>Percent '%'.</summary>
        public const char Percent = '%';

        /// <summary>Hash / pound '#'.</summary>
        public const char Hash = '#';

        /// <summary>Dollar sign '$'.</summary>
        public const char Dollar = '$';

        /// <summary>At sign '@'.</summary>
        public const char At = '@';

        /// <summary>Ampersand '&amp;'.</summary>
        public const char Ampersand = '&';

        /// <summary>Pipe / vertical bar '|'.</summary>
        public const char VerticalBar = '|';

        /// <summary>Tilde '~'.</summary>
        public const char Tilde = '~';

        /// <summary>Backtick '`'.</summary>
        public const char Backtick = '`';

        // Brackets / grouping
        /// <summary>Left parenthesis '('.</summary>
        public const char LeftParen = '(';

        /// <summary>Right parenthesis ')'.</summary>
        public const char RightParen = ')';

        /// <summary>Left bracket '['.</summary>
        public const char LeftBracket = '[';

        /// <summary>Right bracket ']'.</summary>
        public const char RightBracket = ']';

        /// <summary>Left brace '{'.</summary>
        public const char LeftBrace = '{';

        /// <summary>Right brace '}'.</summary>
        public const char RightBrace = '}';

        // Dashes and quotes (Unicode variants)
        /// <summary>En dash (U+2013).</summary>
        public const char EnDash = '\u2013';

        /// <summary>Em dash (U+2014).</summary>
        public const char EmDash = '\u2014';

        /// <summary>Left double quotation mark (U+201C).</summary>
        public const char LeftDoubleQuote = '\u201C';

        /// <summary>Right double quotation mark (U+201D).</summary>
        public const char RightDoubleQuote = '\u201D';

        /// <summary>Left single quotation mark (U+2018).</summary>
        public const char LeftSingleQuote = '\u2018';

        /// <summary>Right single quotation mark (U+2019).</summary>
        public const char RightSingleQuote = '\u2019';

        /// <summary>Directory separator for current platform (runtime value).</summary>
        public static char DirectorySeparator => System.IO.Path.DirectorySeparatorChar;

        /// <summary>Alternate directory separator for current platform (runtime value).</summary>
        public static char AltDirectorySeparator => System.IO.Path.AltDirectorySeparatorChar;

        /// <summary>
        /// Char mapping array for replacing characters in a string when DrawText is called.
        /// The array should be indexed by the character's Unicode value,
        /// and the value at that index is the character to replace it with.
        /// If the value is '\0', the character is not replaced.
        /// </summary>
        public static char[] DrawTextMapping = CreateDrawTextMapping();

        /// <summary>
        /// Gets or sets a value indicating whether to replace certain
        /// characters in DrawText with their mapped values.
        /// This flag is used by <see cref="NormalizeForDrawText"/>
        /// to determine if character replacement should occur.
        /// By default, this is set to true on Linux, meaning that character replacement will occur.
        /// </summary>
        public static bool UseDrawTextMapping;

        /// <summary>
        /// An override of the default character mapping for DrawText, allowing for custom character replacements.
        /// If specified, this delegate will be used in <see cref="NormalizeForDrawText"/>.
        /// </summary>
        public static Func<string?, string?>? NormalizeStrForDrawText;

        static CharUtils()
        {
            if (App.IsLinuxOS)
                UseDrawTextMapping = true;
        }

        /// <summary>
        /// Creates an empty character map array.
        /// </summary>
        /// <returns>An empty character map array.</returns>
        public static char[] CreateCharMap()
        {
            var result = new char[char.MaxValue + 1];
            return result;
        }

        /// <summary>
        /// Sets char mapping in the provided map array.
        /// </summary>
        /// <param name="map">The character map array.</param>
        /// <param name="from">The character to be replaced.</param>
        /// <param name="to">The character to replace with.</param>
        public static void SetCharMapping(char[] map, char from, char to)
        {
            map[from] = to;
        }

        /// <summary>
        /// Creates a character map for DrawText, replacing certain characters with their visual equivalents.
        /// </summary>
        /// <returns>A character map array for DrawText.</returns>
        public static char[] CreateDrawTextMapping()
        {
            var map = CreateCharMap();

            map['\u202F'] = Space; // narrow no-break space → NBSP
            map['\u200B'] = Space; // zero-width space → space

            return map;
        }

        /// <summary>
        /// Sets char mapping in the provided map array for multiple characters.
        /// </summary>
        /// <param name="map">The character map array.</param>
        /// <param name="from">The characters to be replaced.</param>
        /// <param name="to">The characters to replace with.</param>
        /// <exception cref="ArgumentException">Thrown when the 'from' and 'to'
        /// arrays have different lengths.</exception>
        public static void SetCharMapping(char[] map, char[] from, char[] to)
        {
            if (from.Length != to.Length)
                throw new ArgumentException("The 'from' and 'to' arrays must have the same length.");
            for (int i = 0; i < from.Length; i++)
            {
                map[from[i]] = to[i];
            }
        }

        /// <summary>
        /// Gets whether the specified string has any characters that need to be
        /// replaced according to the provided character map.
        /// </summary>
        /// <param name="input">The input string to check.</param>
        /// <param name="map">The character map array.</param>
        /// <returns>True if the input string contains characters
        /// that need to be replaced; otherwise, false.</returns>
        public static bool HasCharsToReplace(ReadOnlySpan<char> input, char[] map)
        {
            if (input.IsEmpty)
                return false;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (map[c] != '\0')
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Replaces characters in the input string according to the character map
        /// provided by <see cref="DrawTextMapping"/> if <see cref="UseDrawTextMapping"/> is true.
        /// If <see cref="UseDrawTextMapping"/> is false, the input string is returned unchanged.
        /// If <see cref="NormalizeStrForDrawText"/> is set, it will
        /// be used to normalize the string instead.
        /// </summary>
        /// <param name="input">The input string to normalize.</param>
        /// <returns>The normalized string.</returns>
        public static string? NormalizeForDrawText(this string? input)
        {
            if(NormalizeStrForDrawText is not null)
                return NormalizeStrForDrawText(input) ?? input;

            if (UseDrawTextMapping)
                return ReplaceMappedChars(input, DrawTextMapping);
            return input;
        }

        /// <summary>
        /// Replaces characters in the input string according to the provided character map.
        /// </summary>
        /// <param name="input">The input string to process.</param>
        /// <param name="map">The character map array.</param>
        /// <returns>A new string with characters replaced according to the map.</returns>
        public static string? ReplaceMappedChars(string? input, char[] map)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            if (!HasCharsToReplace(input.AsSpan(), map))
                return input;

            var result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char sub = map[c];
                result[i] = sub == '\0' ? c : sub;
            }

            return new string(result);
        }

        /// <summary>
        /// Replaces characters in the input span according to the provided character map.
        /// </summary>
        /// <param name="input">The input span to process.</param>
        /// <param name="map">The character map array.</param>
        /// <returns>A new span with characters replaced according to the map.</returns>
        public static ReadOnlySpan<char> ReplaceMappedChars(ReadOnlySpan<char> input, char[] map)
        {
            if (input.IsEmpty)
                return input;
            if (!HasCharsToReplace(input, map))
                return input;

            var result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char sub = map[c];
                result[i] = sub == '\0' ? c : sub;
            }

            return result;
        }
    }
}