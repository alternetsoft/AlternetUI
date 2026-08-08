using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public static partial class TextRenderer
    {
        /// <summary>Provides the size, in dips, of the specified text when drawn with the
        /// specified font in the specified device context, using the specified size to create
        /// an initial bounding rectangle for the text.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <returns>The <see cref="SizeD" />, in dips, of <paramref name="text" />
        /// drawn with the specified <paramref name="font" />.</returns>
        public static SizeD MeasureText(Graphics dc, ReadOnlySpan<char> text, Font? font, SizeD proposedSize)
        {
            /*
            int.MaxValue can be specified in the proposedSize parameter to indicate that there
            is no limit on the width or height of the text. The method will return the size of the text without any constraints.
            
            With Size: measures the text as if it were drawn inside a rectangle of that size.
            Width (proposedSize.Width): maximum line width before wrapping/truncation.
            Height (proposedSize.Height): maximum height allowed; text may be clipped if it exceeds this.
            */

            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font, proposedSize);
            throw new NotImplementedException();
        }

        /// <summary>Provides the size, in dips, of the specified text when drawn with
        /// the specified device context, font, and formatting instructions, using the specified
        /// size to create the initial bounding rectangle for the text.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <param name="flags">The formatting instructions to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in dips, of <paramref name="text" /> drawn
        /// with the specified <paramref name="font" /> and format.</returns>
        public static SizeD MeasureText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font? font,
            SizeD proposedSize,
            TextFormatFlags flags)
        {
            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font, proposedSize, flags);
            throw new NotImplementedException();
        }

        /// <summary>Draws the specified text at the specified location using the specified
        /// device context, font, color, back color, and formatting instructions</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="pt">The <see cref="PointD" /> that represents the upper-left corner
        /// of the drawn text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the text.</param>
        /// <param name="backColor">The <see cref="Color" /> to apply to the background area
        /// of the drawn text. If <paramref name="backColor" /> is <see cref="Color.Transparent" />,
        /// the background is not filled.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        public static void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font? font,
            PointD pt,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor, backColor, flags);
            else
                throw new NotImplementedException();
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
                handler.DrawText(dc, text, font, bounds, foreColor, backColor, flags);
            else
                throw new NotImplementedException();
        }
    }
}
