using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines the interface for a text renderer handler.
    /// </summary>
    public interface ITextRendererHandler
    {
        /// <summary>
        /// Draws the specified text at the specified location using the specified device context,
        /// font, and color.
        /// </summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="pt">The <see cref="PointD" /> that represents the upper-left corner
        /// of the drawn text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        void DrawText(Graphics dc, ReadOnlySpan<char> text, Font font, PointD pt, Color foreColor);

        /// <summary>Draws the specified text at the specified location, using the specified
        /// device context, font, color, and back color.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="pt">The <see cref="PointD" /> that represents the upper-left corner
        /// of the drawn text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="backColor">The <see cref="Color" /> to apply to the background area
        /// of the drawn text.</param>
        void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            PointD pt,
            Color foreColor,
            Color backColor);

        /// <summary>Draws the specified text within the specified bounds, using the
        /// specified device context, font, and color.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        void DrawText(Graphics dc, ReadOnlySpan<char> text, Font font, RectD bounds, Color foreColor);

        /// <summary>Draws the specified text within the specified bounds using the
        /// specified device context, font, color, and back color.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="backColor">The <see cref="Color" /> to apply to the area represented
        /// by <paramref name="bounds" />.</param>
        void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            RectD bounds,
            Color foreColor,
            Color backColor);

        /// <summary>Provides the size, in dips, of the specified text drawn with the
        /// specified font in the specified device context.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in dips, of <paramref name="text" /> drawn
        /// in a single line with the specified <paramref name="font" /> in the specified
        /// device context.</returns>
        SizeD MeasureText(Graphics dc, ReadOnlySpan<char> text, Font font);

        /// <summary>Provides the size, in dips, of the specified text when drawn with the
        /// specified font in the specified device context, using the specified size to create
        /// an initial bounding rectangle for the text.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <returns>The <see cref="SizeD" />, in dips, of <paramref name="text" />
        /// drawn with the specified <paramref name="font" />.</returns>
        SizeD MeasureText(Graphics dc, ReadOnlySpan<char> text, Font font, SizeD proposedSize);

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
        SizeD MeasureText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            SizeD proposedSize,
            TextFormatFlags flags);

        /// <summary>Draws the specified text at the specified location using the specified
        /// device context, font, color, and formatting instructions.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="pt">The <see cref="PointD" /> that represents the upper-left corner
        /// of the drawn text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            PointD pt,
            Color foreColor,
            TextFormatFlags flags);


        /// <summary>Draws the specified text at the specified location using the specified
        /// device context, font, color, back color, and formatting instructions</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="pt">The <see cref="PointD" /> that represents the upper-left corner
        /// of the drawn text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the text.</param>
        /// <param name="backColor">The <see cref="Color" /> to apply to the background area
        /// of the drawn text.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            PointD pt,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags);

        /// <summary>Draws the specified text within the specified bounds using the
        /// specified device context, font, color, and formatting instructions.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            RectD bounds,
            Color foreColor,
            TextFormatFlags flags);

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
        void DrawText(
            Graphics dc,
            ReadOnlySpan<char> text,
            Font font,
            RectD bounds,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags);
    }
}



