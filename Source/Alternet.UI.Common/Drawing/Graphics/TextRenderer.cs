using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods used to measure and render text.
    /// This class is created for the compatibility with the WinForms.
    /// </summary>
    public static class TextRenderer
    {
        private static Graphics? measure;

        private static Graphics Measure
        {
            get
            {
                Graphics.RequireMeasure(Display.MaxScaleFactor, ref measure);
                return measure;
            }
        }

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
        public static void DrawText(Graphics dc, string text, Font font, PointD pt, Color foreColor)
        {
            dc.DrawText(text, pt, font, foreColor, Color.Empty);
        }

        /// <summary>Provides the size, in pixels, of the specified text when drawn with
        /// the specified font.</summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in pixels, of <paramref name="text" /> drawn
        /// on a single line with the specified <paramref name="font" />. You can manipulate
        /// how the text is drawn by using one of the
        /// <see cref="TextRenderer.DrawText(Graphics,string,Font,RectD,Color,TextFormatFlags)" />
        /// overloads that takes a <see cref="TextFormatFlags" /> parameter.
        /// For example, the default behavior of the <see cref="TextRenderer" /> is to
        /// add padding to the bounding rectangle of the drawn text to accommodate overhanging glyphs.
        /// If you need to draw a line of text without these extra spaces you should use the versions
        /// of <see cref="TextRenderer.DrawText(Graphics,string,Font,PointD,Color)" /> and
        /// <see cref="TextRenderer.MeasureText(Graphics,string,Font)" /> that take
        /// a <see cref="SizeD" /> and <see cref="TextFormatFlags" /> parameter. For an example,
        /// see <see cref="TextRenderer.MeasureText(Graphics,string,Font,SizeD,TextFormatFlags)" />.
        /// </returns>
        public static SizeD MeasureText(string text, Font font)
        {
            return Measure.MeasureText(text, font);
        }

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
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            PointD pt,
            Color foreColor,
            Color backColor)
        {
            dc.DrawText(text, pt, font, foreColor, backColor);
        }

        /// <summary>Draws the specified text within the specified bounds, using the
        /// specified device context, font, and color.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        public static void DrawText(Graphics dc, string text, Font font, RectD bounds, Color foreColor)
        {
            throw new NotImplementedException();
        }

        /// <summary>Draws the specified text within the specified bounds using the
        /// specified device context, font, color, and back color.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="backColor">The <see cref="Color" /> to apply to the area represented
        /// by <paramref name="bounds" />.</param>
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            RectD bounds,
            Color foreColor,
            Color backColor)
        {
            throw new NotImplementedException();
        }

        /// <summary>Provides the size, in pixels, of the specified text when drawn with the
        /// specified font, using the specified size to create an initial bounding rectangle.</summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <returns>The <see cref="SizeD" />, in pixels, of <paramref name="text" /> drawn with
        /// the specified <paramref name="font" />.</returns>
        public static SizeD MeasureText(string text, Font font, SizeD proposedSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>Provides the size, in pixels, of the specified text drawn with the
        /// specified font in the specified device context.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in pixels, of <paramref name="text" /> drawn
        /// in a single line with the specified <paramref name="font" /> in the specified
        /// device context.</returns>
        public static SizeD MeasureText(Graphics dc, string text, Font font)
        {
            return dc.MeasureText(text, font);
        }

        /// <summary>Provides the size, in pixels, of the specified text when drawn with the
        /// specified font in the specified device context, using the specified size to create
        /// an initial bounding rectangle for the text.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <returns>The <see cref="SizeD" />, in pixels, of <paramref name="text" />
        /// drawn with the specified <paramref name="font" />.</returns>
        public static SizeD MeasureText(Graphics dc, string text, Font font, SizeD proposedSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>Provides the size, in pixels, of the specified text when drawn with the specified
        /// font and formatting instructions, using the specified size to create the initial
        /// bounding rectangle for the text.</summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <param name="flags">The formatting instructions to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in pixels, of <paramref name="text" /> drawn
        /// with the specified <paramref name="font" /> and format.</returns>
        public static SizeD MeasureText(string text, Font font, SizeD proposedSize, TextFormatFlags flags)
        {
            throw new NotImplementedException();
        }

        /// <summary>Provides the size, in pixels, of the specified text when drawn with
        /// the specified device context, font, and formatting instructions, using the specified
        /// size to create the initial bounding rectangle for the text.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <param name="proposedSize">The <see cref="SizeD" /> of the initial bounding rectangle.</param>
        /// <param name="flags">The formatting instructions to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in pixels, of <paramref name="text" /> drawn
        /// with the specified <paramref name="font" /> and format.</returns>
        public static SizeD MeasureText(
            Graphics dc,
            string text,
            Font font,
            SizeD proposedSize,
            TextFormatFlags flags)
        {
            throw new NotImplementedException();
        }

        /// <summary>Draws the specified text at the specified location using the specified
        /// device context, font, color, and formatting instructions.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="pt">The <see cref="PointD" /> that represents the upper-left corner
        /// of the drawn text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            PointD pt,
            Color foreColor,
            TextFormatFlags flags)
        {
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
        /// of the drawn text.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            PointD pt,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags)
        {
            throw new NotImplementedException();
        }

        /// <summary>Draws the specified text within the specified bounds using the
        /// specified device context, font, color, and formatting instructions.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags" /> values.</param>
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            RectD bounds,
            Color foreColor,
            TextFormatFlags flags)
        {
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
            string text,
            Font font,
            RectD bounds,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags)
        {
            throw new NotImplementedException();
        }
    }
}
