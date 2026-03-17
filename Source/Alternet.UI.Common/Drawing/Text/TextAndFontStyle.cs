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
        /// Gets or sets foreground color of the text.
        /// If not assigned, the default foreground color of the text is used.
        /// </summary>
        public Color? ForeColor;

        /// <summary>
        /// Gets or sets background color of the text.
        /// If not assigned, the default background color of the text is used.
        /// </summary>
        public Color? BackColor;

        /// <summary>
        /// Gets or sets an object that contains an additional data.
        /// </summary>
        /// <remarks>This property can be used to associate custom data with the object. The value can be
        /// any object, or null if no data is associated.</remarks>
        public object? Tag;

        /// <summary>
        /// Gets bounds of the item after painting. This property is used to store the measured bounds of the text after it has been rendered.
        /// </summary>
        public RectD? MeasuredBounds;

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
        /// Initializes a new instance of the TextAndFontStyle class with the specified text, font style, and optional
        /// foreground color.
        /// </summary>
        /// <param name="text">The text to be displayed or styled.</param>
        /// <param name="style">The font style to apply to the text.</param>
        /// <param name="foreColor">The foreground color to use for the text, or null to use the default color.</param>
        public TextAndFontStyle(string text, FontStyle style, Color? foreColor)
        {
            Text = text;
            FontStyle = style;
            ForeColor = foreColor;
        }

        /// <summary>
        /// Initializes a new instance of the TextAndFontStyle class with the specified text, font style, and optional
        /// foreground and background colors.
        /// </summary>
        /// <param name="text">The text to be displayed with the specified style and colors.</param>
        /// <param name="style">The font style to apply to the text.</param>
        /// <param name="foreColor">The foreground color to use for the text, or null to use the default color.</param>
        /// <param name="backColor">The background color to use behind the text, or null to use the default color.</param>
        public TextAndFontStyle(string text, FontStyle style, Color? foreColor, Color? backColor)
        {
            Text = text;
            FontStyle = style;
            ForeColor = foreColor;
            BackColor = backColor;
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
