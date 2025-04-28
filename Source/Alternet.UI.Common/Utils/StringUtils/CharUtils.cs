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
        /// <summary>
        /// Represents the backspace control character ('\b').
        /// This character is used to move the cursor back one position in text.
        /// </summary>
        public const char BackspaceChar = '\b';

        /// <summary>
        /// Represents the Unicode right arrow symbol (→).
        /// Unicode: U+2192
        /// </summary>
        public const char RightArrow = '\u2192';

        /// <summary>
        /// Represents the Unicode left arrow symbol (←).
        /// Unicode: U+2190
        /// </summary>
        public const char LeftArrow = '\u2190';
    }
}
