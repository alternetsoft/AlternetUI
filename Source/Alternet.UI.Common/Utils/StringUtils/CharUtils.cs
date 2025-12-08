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
    }
}