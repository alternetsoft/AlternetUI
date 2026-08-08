using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods used to measure and render text.
    /// This class is created for the compatibility with the WinForms.
    /// Default implementation is limited and does not support all features of the WinForms TextRenderer.
    /// Assign <see cref="Handler"/> property to a custom implementation of <see cref="ITextRendererHandler"/>
    /// in order to use the full functionality of the WinForms TextRenderer.
    /// </summary>
    public static partial class TextRenderer
    {
        private static Graphics? measure;
        private static ITextRendererHandler? handler;

        /// <summary>
        /// Gets measurement graphics object. This object is used to measure text size.
        /// </summary>
        public static Graphics Measure
        {
            get
            {
                Graphics.RequireMeasure(ref measure, new(Display.MaxScaleFactor));
                return measure;
            }

            set
            {
                measure = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ITextRendererHandler" /> that is used to measure and render text.
        /// </summary>
        public static ITextRendererHandler? Handler
        {
            get => handler;
            set => handler = value;
        }

        /// <summary>
        /// This method is used to update scale factor of the measurement graphics object
        /// when the application scale factor is changed.
        /// </summary>
        /// <param name="scaleFactor">The new scale factor.</param>
        public static void SetMeasureScaleFactor(float scaleFactor)
        {
            Graphics.RequireMeasure(ref measure, new(scaleFactor));
        }

        /// <summary>
        /// Converts <see cref="TextFormatFlags"/> to <see cref="TextVerticalAlignment"/>.
        /// </summary>
        /// <param name="flags">The <see cref="TextFormatFlags"/> to convert.</param>
        /// <returns>The corresponding <see cref="TextVerticalAlignment"/>.</returns>
        public static TextVerticalAlignment ToVerticalAlignment(TextFormatFlags flags)
        {
            if ((flags & TextFormatFlags.VerticalCenter) != 0)
                return TextVerticalAlignment.Center;
            if ((flags & TextFormatFlags.Bottom) != 0)
                return TextVerticalAlignment.Bottom;
            return TextVerticalAlignment.Top;
        }

        /// <summary>
        /// Converts <see cref="TextFormatFlags"/> to <see cref="TextHorizontalAlignment"/>.
        /// </summary>
        /// <param name="flags">The <see cref="TextFormatFlags"/> to convert.</param>
        /// <returns>The corresponding <see cref="TextHorizontalAlignment"/>.</returns>
        public static TextHorizontalAlignment ToHorizontalAlignment(TextFormatFlags flags)
        {
            if ((flags & TextFormatFlags.HorizontalCenter) != 0)
                return TextHorizontalAlignment.Center;
            if ((flags & TextFormatFlags.Right) != 0)
                return TextHorizontalAlignment.Right;
            return TextHorizontalAlignment.Left;
        }

        private static void BeforeDrawText(Graphics graphics, [NotNull] ref Font? font)
        {
            font ??= Control.DefaultFont;
        }

        private static void BeforeMeasureText(Graphics graphics, [NotNull] ref Font? font)
        {
            font ??= Control.DefaultFont;
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
        public static void DrawText(Graphics dc, ReadOnlySpan<char> text, Font? font, PointD pt, Color foreColor)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor);
            else
                dc.DrawText(text, pt, font, foreColor, Color.Empty);
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
            ReadOnlySpan<char> text,
            Font? font,
            PointD pt,
            Color foreColor,
            Color backColor)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor, backColor);
            else
                dc.DrawText(text, pt, font, foreColor, backColor);
        }

        /// <summary>Draws the specified text within the specified bounds, using the
        /// specified device context, font, and color.</summary>
        /// <param name="dc">The device context in which to draw the text.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the drawn text.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the bounds of the text.</param>
        /// <param name="foreColor">The <see cref="Color" /> to apply to the drawn text.</param>
        public static void DrawText(Graphics dc, ReadOnlySpan<char> text, Font? font, RectD bounds, Color foreColor)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, bounds, foreColor);
            else
                dc.DrawText(text, bounds, font, foreColor, Color.Transparent);
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
            ReadOnlySpan<char> text,
            Font? font,
            RectD bounds,
            Color foreColor,
            Color backColor)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, bounds, foreColor, backColor);
            else
                dc.DrawText(text, bounds, font, foreColor, backColor);
        }

        /// <summary>Provides the size, in dips, of the specified text drawn with the
        /// specified font in the specified device context.</summary>
        /// <param name="dc">The device context in which to measure the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The <see cref="Font" /> to apply to the measured text.</param>
        /// <returns>The <see cref="SizeD" />, in dips, of <paramref name="text" /> drawn
        /// in a single line with the specified <paramref name="font" /> in the specified
        /// device context.</returns>
        public static SizeD MeasureText(Graphics dc, ReadOnlySpan<char> text, Font? font)
        {
            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font);
            return dc.MeasureText(text, font);
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
            ReadOnlySpan<char> text,
            Font? font,
            PointD pt,
            Color foreColor,
            TextFormatFlags flags)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor, flags);
            else
                DrawText(dc, text, font, pt, foreColor, Color.Transparent, flags);
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
            ReadOnlySpan<char> text,
            Font? font,
            RectD bounds,
            Color foreColor,
            TextFormatFlags flags)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, bounds, foreColor, flags);
            else
                DrawText(dc, text, font, bounds, foreColor, Color.Transparent, flags);
        }

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
            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font, proposedSize);

            var result = dc.DrawText(
                        text,
                        font,
                        brush: null,
                        new RectD(PointD.Empty, proposedSize),
                        TextFormat.DefaultRecord);
            return result.Size;
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
            DrawText(
                dc,
                text,
                font,
                new RectD(pt, SizeD.Empty),
                foreColor,
                backColor,
                flags);
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

            TextFormat.Record record = AsTextFormat(flags);

            var result = dc.DrawText(
                                text,
                                font,
                                brush: null,
                                new RectD(PointD.Empty, proposedSize),
                                record);
            return result.Size;
        }
    }
}
