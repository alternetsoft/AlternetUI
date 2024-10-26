using System;
using System.Collections.Generic;
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
        /// Initializes a new instance of the <see cref="TextAndFontStyle"/> struct.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <param name="style">Font style.</param>
        public TextAndFontStyle(string text, FontStyle style = FontStyle.Regular)
        {
            Text = text;
            FontStyle = style;
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
