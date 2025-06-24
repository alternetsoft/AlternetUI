using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known text trimming rules.
    /// </summary>
    [Flags]
    public enum TrimTextRules
    {
        /// <summary>
        /// Trims ( and ) characters.
        /// </summary>
        TrimRoundBrackets = 1,

        /// <summary>
        /// Trims [ and ] characters.
        /// </summary>
        TrimSquareBrackets = 2,

        /// <summary>
        /// Trims { and } characters.
        /// </summary>
        TrimFigureBrackets = 4,

        /// <summary>
        /// Trims angle brackets (less-than and greater-than characters).
        /// </summary>
        TrimAngleBrackets = 8,

        /// <summary>
        /// Trims all types of brackets.
        /// </summary>
        TrimBrackets
            = TrimFigureBrackets | TrimSquareBrackets | TrimRoundBrackets | TrimAngleBrackets,

        /// <summary>
        /// Trims white characters.
        /// </summary>
        TrimWhiteChars = 16,

        /// <summary>
        /// Trims white chars and all types of brackets.
        /// </summary>
        TrimBracketsAndWhiteChars = TrimBrackets | TrimWhiteChars,

        /// <summary>
        /// Trims spaces.
        /// </summary>
        TrimSpaces = 32,

        /// <summary>
        /// Do not trim characters at the start of the string,
        /// </summary>
        NoLeading = 64,

        /// <summary>
        /// Do not trim characters at the end of the string.
        /// </summary>
        NoTrailing = 128,
    }
}