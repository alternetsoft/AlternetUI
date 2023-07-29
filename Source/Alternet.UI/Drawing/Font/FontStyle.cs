using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies style information applied to text.
    /// </summary>
    /// <remarks>
    /// This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum FontStyle
    {
        /// <summary>
        /// Normal text.
        /// </summary>
        Regular = 0,

        /// <summary>
        /// Bold text.
        /// </summary>
        Bold = 1 << 0,

        /// <summary>
        /// Italic text.
        /// </summary>
        Italic = 1 << 1,

        /// <summary>
        /// Underlined text.
        /// </summary>
        Underlined = 1 << 2,

        /// <summary>
        /// Text with a line through the middle.
        /// </summary>
        Strikethrough = 1 << 3,
    }
}