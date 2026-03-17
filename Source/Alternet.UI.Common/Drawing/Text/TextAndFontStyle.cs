using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains text and font style.
    /// </summary>
    public struct TextAndFontStyle
    {
        /// <summary>
        /// Gets or sets text.
        /// </summary>
        public string Text;

        /// <summary>
        /// Gets or sets font style.
        /// </summary>
        public FontStyle FontStyle;

        /// <summary>
        /// Gets or sets an object that contains an additional data.
        /// </summary>
        /// <remarks>This property can be used to associate custom data with the object. The value can be
        /// any object, or null if no data is associated.</remarks>
        public object? Tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAndFontStyle"/> struct
        /// with the specified text.
        /// </summary>
        /// <param name="text">Text string.</param>
        public TextAndFontStyle(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAndFontStyle"/> struct with the specified
        /// text and font style.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <param name="style">Font style.</param>
        public TextAndFontStyle(string text, FontStyle style)
        {
            Text = text;
            FontStyle = style;
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from <see cref="string"/> to
        /// <see cref="TextAndFontStyle"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator TextAndFontStyle(string value)
        {
            return new(value);
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            string[] names = { nameof(Text), nameof(FontStyle) };
            object[] values = { "<" + Text + ">", FontStyle };

            return StringUtils.ToStringWithOrWithoutNames(names, values);
        }
    }
}
