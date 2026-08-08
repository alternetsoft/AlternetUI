using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public static partial class TextRenderer
    {
        /// <summary>
        /// Converts the specified <see cref="TextFormatFlags" /> to a <see cref="TextFormat.Record" />.
        /// </summary>
        /// <param name="flags">The formatting instructions to convert.</param>
        /// <returns>A <see cref="TextFormat.Record" /> that represents the specified formatting instructions.</returns>
        public static TextFormat.Record AsTextFormat(TextFormatFlags flags)
        {
            TextFormat.Record record = new();
            record.VerticalAlignment = ToVerticalAlignment(flags);
            record.HorizontalAlignment = ToHorizontalAlignment(flags);
            return record;
        }

        /// <summary>Draws the specified text within the specified bounds using the specified
        /// device context, font, color, back color, and formatting instructions.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the text.</param>
        /// <param name="backColor">The <see cref="Color" /> to apply to the area represented
        /// by <paramref name="bounds" />.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        public static void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font? font,
            RectD bounds,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
            {
                handler.DrawText(dc, text, font, bounds, foreColor, backColor, flags);
            }

            TextFormat.Record record = AsTextFormat(flags);

            // !! backColor

            var result = dc.DrawText(text, font, foreColor.AsBrush, bounds, record);
        }
    }
}
