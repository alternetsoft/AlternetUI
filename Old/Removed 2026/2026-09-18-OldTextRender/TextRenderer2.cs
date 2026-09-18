using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods for rendering text to a <see cref="Graphics"/> object.
    /// </summary>
    public static class TextRenderer
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void BeforeDrawText(Graphics graphics, [NotNull] ref Font? font)
        {
            font ??= Control.DefaultFont;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void BeforeMeasureText(Graphics graphics, [NotNull] ref Font? font)
        {
            font ??= Control.DefaultFont;
        }

        /// <summary>
        /// Draws the specified text string at the specified location using the specified font and foreground color.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="pt">The location at which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(Graphics dc, string text, Font? font, PointD pt, Color foreColor)
        {
            BeforeDrawText(dc, ref font);

            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor);
            else
                DrawTextInternal(dc, text, font, pt, foreColor, Color.Transparent, TextFormatFlags.Default, false);
        }

        /// <summary>
        /// Draws the specified text string within the specified rectangle using the specified font and foreground color.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="bounds">The rectangle in which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(Graphics dc, string text, Font? font, RectD bounds, Color foreColor)
        {
            BeforeDrawText(dc, ref font);

            if (handler != null)
                handler.DrawText(dc, text, font, bounds, foreColor);
            else
            {
                DrawTextInternal(
                dc,
                text,
                font,
                bounds,
                foreColor,
                Color.Transparent,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                false);
            }
        }

        /// <summary>
        /// Draws the specified text string at the specified location using the specified font,
        /// foreground color, and background color.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="pt">The location at which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(Graphics dc, string text, Font? font, PointD pt, Color foreColor, Color backColor)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor, backColor);
            else
                DrawTextInternal(dc, text, font, pt, foreColor, backColor, TextFormatFlags.Default, false);
        }

        /// <summary>
        /// Draws the specified text string at the specified location using the specified font and foreground color.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="pt">The location at which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="flags">The formatting options for the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(Graphics dc, string text, Font? font, PointD pt, Color foreColor, TextFormatFlags flags)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, pt, foreColor, flags);
            else
                DrawTextInternal(dc, text, font, pt, foreColor, Color.Transparent, flags, false);
        }

        /// <summary>
        /// Draws the specified text string within the specified rectangle using the specified font,
        /// foreground color, and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="bounds">The rectangle in which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(Graphics dc, string text, Font? font, RectD bounds, Color foreColor, Color backColor)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, bounds, foreColor, backColor);
            else
            {
                DrawTextInternal(
                    dc,
                    text,
                    font,
                    bounds,
                    foreColor,
                    backColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    false);
            }
        }

        /// <summary>
        /// Draws the specified text string within the specified rectangle using the specified font,
        /// foreground color, and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="bounds">The rectangle in which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="flags">The formatting options for the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(
            Graphics dc,
            string text,
            Font? font,
            RectD bounds,
            Color foreColor,
            TextFormatFlags flags)
        {
            BeforeDrawText(dc, ref font);
            if (handler != null)
                handler.DrawText(dc, text, font, bounds, foreColor, flags);
            else
                DrawTextInternal(dc, text, font, bounds, foreColor, Color.Transparent, flags, false);
        }

        /// <summary>
        /// Draws the specified text string within the specified rectangle using the specified font,
        /// foreground color, background color, and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="pt">The point at which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <param name="flags">The formatting options for the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(
            Graphics dc,
            string text,
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
                DrawTextInternal(dc, text, font, pt, foreColor, backColor, flags, false);
        }

        /// <summary>
        /// Draws the specified text string within the specified rectangle using the specified font,
        /// foreground color, background color, and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="bounds">The rectangle in which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <param name="flags">The formatting options for the text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawText(
            Graphics dc,
            string text,
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
                DrawTextInternal(dc, text, font, bounds, foreColor, backColor, flags, false);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD MeasureText(string text, Font? font)
        {
            BeforeMeasureText(Measure, ref font);
            if (handler != null)
                return handler.MeasureText(Measure, text, font);
            else
                return MeasureTextInternal(Measure, text, font, SizeD.Empty, TextFormatFlags.Default);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font.
        /// </summary>
        /// <param name="dc">The graphics context to use for measuring the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD MeasureText(Graphics dc, string text, Font? font)
        {
            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font);
            else
                return MeasureTextInternal(dc, text, font, SizeD.Empty, TextFormatFlags.Default);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <returns>The size of the text.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD MeasureText(string text, Font? font, SizeD proposedSize)
        {
            BeforeMeasureText(Measure, ref font);
            if (handler != null)
                return handler.MeasureText(Measure, text, font, proposedSize);
            else
                return MeasureTextInternal(Measure, text, font, proposedSize, TextFormatFlags.Default);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to use for measuring the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <returns>The size of the text.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD MeasureText(Graphics dc, string text, Font? font, SizeD proposedSize)
        {
            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font, proposedSize);
            else
                return MeasureTextInternal(dc, text, font, proposedSize, TextFormatFlags.Default);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <param name="flags">The formatting options to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD MeasureText(string text, Font? font, SizeD proposedSize, TextFormatFlags flags)
        {
            BeforeMeasureText(Measure, ref font);
            if (handler != null)
                return handler.MeasureText(Measure, text, font, proposedSize, flags);
            else
                return MeasureTextInternal(Measure, text, font, proposedSize, flags);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to use for measuring the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <param name="flags">The formatting options to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD MeasureText(Graphics dc, string text, Font? font, SizeD proposedSize, TextFormatFlags flags)
        {
            BeforeMeasureText(dc, ref font);
            if (handler != null)
                return handler.MeasureText(dc, text, font, proposedSize, flags);
            else
                return MeasureTextInternal(dc, text, font, proposedSize, flags);
        }

        internal static void DrawTextInternal(
            Graphics dc,
            string text,
            Font font,
            RectD bounds,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags,
            bool useDrawString)
        {
            if (dc == null)
                throw new ArgumentNullException(nameof(dc));

            if (text == null || text.Length == 0)
                return;

            RectD newBounds = PadDrawStringRectangle(bounds, flags);

            TextFormat.Record sf = FlagsToTextFormat(flags);
            dc.DrawText(text, font, foreColor.AsBrush, newBounds, in sf);

            /*
            StringFormat sf = FlagsToStringFormat(flags);
            dc.DrawString(text, font, foreColor.AsBrush, newBounds, sf);
            */
        }

        internal static SizeD MeasureTextInternal(
            Graphics dc,
            string text,
            Font font,
            SizeD proposedSize,
            TextFormatFlags flags)
        {
            SizeD retval;

            float proposedWidth;
            if (proposedSize.Width == 0)
                proposedWidth = Int32.MaxValue;
            else
            {
                proposedWidth = proposedSize.Width;
                if (!flags.HasFlag(TextFormatFlags.NoPadding))
                    proposedWidth -= 9;
            }

            TextFormat.Record sf = FlagsToTextFormat(flags);
            retval = dc.MeasureText(text, font, proposedWidth, in sf);

            /*
            StringFormat sf = FlagsToStringFormat(flags);
            retval = dc.MeasureString(text, font, proposedWidth, sf);
            */

            if (retval.Width > 0 && !flags.HasFlag(TextFormatFlags.NoPadding))
                retval.Width += 9;

            return retval;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DrawTextInternal(
            Graphics dc,
            string text,
            Font font,
            PointD pt,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags,
            bool useDrawString)
        {
            SizeD sz = MeasureTextInternal(dc, text, font, flags, useDrawString);
            DrawTextInternal(dc, text, font, new RectD(pt, sz), foreColor, backColor, flags, useDrawString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static SizeD MeasureTextInternal(
            Graphics dc,
            string text,
            Font font,
            TextFormatFlags flags,
            bool useMeasureString)
        {
            return MeasureTextInternal(dc, text, font, SizeD.Empty, flags);
        }

        /// <summary>
        /// Converts <see cref="TextFormatFlags"/> to <see cref="StringFormat"/>.
        /// </summary>
        /// <param name="flags">The text format flags to convert.</param>
        /// <returns>A <see cref="StringFormat"/> representing the specified text format flags.</returns>
        public static StringFormat FlagsToStringFormat(TextFormatFlags flags)
        {
            StringFormat sf = new();

            // Translation table: http://msdn.microsoft.com/msdnmag/issues/06/03/TextRendering/default.aspx?fig=true#fig4

            if (flags.HasFlag(TextFormatFlags.HorizontalCenter))
                sf.Alignment = StringAlignment.Center;
            else if (flags.HasFlag(TextFormatFlags.Right))
                sf.Alignment = StringAlignment.Far;
            else
                sf.Alignment = StringAlignment.Near;

            if (flags.HasFlag(TextFormatFlags.Bottom))
                sf.LineAlignment = StringAlignment.Far;
            else if (flags.HasFlag(TextFormatFlags.VerticalCenter))
                sf.LineAlignment = StringAlignment.Center;
            else
                sf.LineAlignment = StringAlignment.Near;

            if (flags.HasFlag(TextFormatFlags.EndEllipsis))
                sf.Trimming = StringTrimming.EllipsisCharacter;
            else if (flags.HasFlag(TextFormatFlags.PathEllipsis))
                sf.Trimming = StringTrimming.EllipsisPath;
            else if (flags.HasFlag(TextFormatFlags.WordEllipsis))
                sf.Trimming = StringTrimming.EllipsisWord;
            else
                sf.Trimming = StringTrimming.Character;

            if (flags.HasFlag(TextFormatFlags.NoPrefix))
                sf.HotkeyPrefix = HotkeyPrefix.None;
            else if (flags.HasFlag(TextFormatFlags.HidePrefix))
                sf.HotkeyPrefix = HotkeyPrefix.Hide;
            else
                sf.HotkeyPrefix = HotkeyPrefix.Show;

            if (flags.HasFlag(TextFormatFlags.NoPadding))
                sf.FormatFlags |= StringFormatFlags.FitBlackBox;

            if (flags.HasFlag(TextFormatFlags.SingleLine))
                sf.FormatFlags |= StringFormatFlags.NoWrap;
            else if (flags.HasFlag(TextFormatFlags.TextBoxControl))
                sf.FormatFlags |= StringFormatFlags.LineLimit;

            if (flags.HasFlag(TextFormatFlags.NoClipping))
                sf.FormatFlags |= StringFormatFlags.NoClip;

            return sf;
        }

        private static TextFormat.Record FlagsToTextFormat(TextFormatFlags flags)
        {
            TextFormat.Record sf = new();

            if (flags.HasFlag(TextFormatFlags.HorizontalCenter))
                sf.HorizontalAlignment = TextHorizontalAlignment.Center;
            else if (flags.HasFlag(TextFormatFlags.Right))
                sf.HorizontalAlignment = TextHorizontalAlignment.Right;
            else
                sf.HorizontalAlignment = TextHorizontalAlignment.Left;

            if (flags.HasFlag(TextFormatFlags.Bottom))
                sf.VerticalAlignment = TextVerticalAlignment.Bottom;
            else if (flags.HasFlag(TextFormatFlags.VerticalCenter))
                sf.VerticalAlignment = TextVerticalAlignment.Center;
            else
                sf.VerticalAlignment = TextVerticalAlignment.Top;

            /*
                        if (flags.HasFlag(TextFormatFlags.EndEllipsis))
                            sf.Trimming = TextTrimming.Char;
                        else if (flags.HasFlag(TextFormatFlags.PathEllipsis))
                            sf.Trimming = TextTrimming.EllipsisPath;
                        else if (flags.HasFlag(TextFormatFlags.WordEllipsis))
                            sf.Trimming = TextTrimming.EllipsisWord;
                        else
                            sf.Trimming = TextTrimming.Char;
            */

            /*
                        if (flags.HasFlag(TextFormatFlags.NoPrefix))
                            sf.HotkeyPrefix = HotkeyPrefix.None;
                        else if (flags.HasFlag(TextFormatFlags.HidePrefix))
                            sf.HotkeyPrefix = HotkeyPrefix.Hide;
                        else
                            sf.HotkeyPrefix = HotkeyPrefix.Show;
            */

            /*
                        if (flags.HasFlag(TextFormatFlags.NoPadding))
                            sf.FormatFlags |= StringFormatFlags.FitBlackBox;
            */

            if (flags.HasFlag(TextFormatFlags.WordBreak) || flags.HasFlag(TextFormatFlags.WordEllipsis))
                sf.Wrapping = TextWrapping.Word;
            else
                sf.Wrapping = TextWrapping.None;

            if (flags.HasFlag(TextFormatFlags.SingleLine))
                sf.Wrapping = TextWrapping.None;

            /*
                        else
                        if (flags.HasFlag(TextFormatFlags.TextBoxControl))
                            sf.FormatFlags |= StringFormatFlags.LineLimit;
            */
            /*
            if (flags.HasFlag(TextFormatFlags.NoClipping))
                sf.FormatFlags |= StringFormatFlags.NoClip;
            */

            return sf;
        }

        /// <summary>
        /// Pads the specified rectangle based on the text format flags.
        /// </summary>
        /// <param name="r">The rectangle to pad.</param>
        /// <param name="flags">The text format flags.</param>
        /// <returns>The padded rectangle.</returns>
        public static RectD PadDrawStringRectangle(RectD r, TextFormatFlags flags)
        {
            if (!flags.HasFlag(TextFormatFlags.NoPadding) && !flags.HasFlag(TextFormatFlags.Right)
                && !flags.HasFlag(TextFormatFlags.HorizontalCenter))
            {
                r.X += 1;
                r.Width -= 1;
            }
            if (!flags.HasFlag(TextFormatFlags.NoPadding) && flags.HasFlag(TextFormatFlags.Right))
            {
                r.Width -= 4;
            }
            if (flags.HasFlag(TextFormatFlags.NoPadding))
            {
                r.X -= 2;
            }
            if (!flags.HasFlag(TextFormatFlags.NoPadding) && flags.HasFlag(TextFormatFlags.Bottom))
            {
                r.Y += 1;
            }
            if (flags.HasFlag(TextFormatFlags.LeftAndRightPadding))
            {
                r.X += 2;
                r.Width -= 2;
            }

            return r;
        }
    }
}
