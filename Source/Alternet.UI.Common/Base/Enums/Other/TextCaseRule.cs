using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines rules for changing the case of text.
    /// </summary>
    public enum TextCaseRule
    {
        /// <summary>
        /// No case conversion is applied; the text remains unchanged.
        /// </summary>
        None,

        /// <summary>
        /// Convert all characters to lowercase using the current culture.
        /// </summary>
        Lower,

        /// <summary>
        /// Convert all characters to uppercase using the current culture.
        /// </summary>
        Upper,

        /// <summary>
        /// Convert all characters to lowercase using the invariant culture.
        /// </summary>
        LowerInvariant,

        /// <summary>
        /// Convert all characters to uppercase using the invariant culture.
        /// </summary>
        UpperInvariant,

        /// <summary>
        /// Capitalize only the first character of the string, leaving the rest unchanged.
        /// </summary>
        SentenceCase,
    }
}
