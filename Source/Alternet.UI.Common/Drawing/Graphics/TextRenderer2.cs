using System;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods for rendering text to a <see cref="Graphics"/> object.
    /// </summary>
    internal static class TextRenderer2
    {
        private static Graphics? measure;

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
        /// Draws the specified text string at the specified location using the specified font and foreground color.
        /// </summary>
        /// <param name="dc">The graphics context to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="pt">The location at which to draw the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        public static void DrawText(Graphics dc, string text, Font font, PointD pt, Color foreColor)
        {
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
        public static void DrawText(Graphics dc, string text, Font font, RectD bounds, Color foreColor)
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
        public static void DrawText(Graphics dc, string text, Font font, PointD pt, Color foreColor, Color backColor)
        {
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
        public static void DrawText(Graphics dc, string text, Font font, PointD pt, Color foreColor, TextFormatFlags flags)
        {
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
        public static void DrawText(Graphics dc, string text, Font font, RectD bounds, Color foreColor, Color backColor)
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
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            RectD bounds,
            Color foreColor,
            TextFormatFlags flags)
        {
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
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            PointD pt,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags)
        {
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
        public static void DrawText(
            Graphics dc,
            string text,
            Font font,
            RectD bounds,
            Color foreColor,
            Color backColor,
            TextFormatFlags flags)
        {
            DrawTextInternal(dc, text, font, bounds, foreColor, backColor, flags, false);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        public static SizeD MeasureText(string text, Font font)
        {
            return MeasureTextInternal(Measure, text, font, SizeD.Empty, TextFormatFlags.Default, false);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font.
        /// </summary>
        /// <param name="dc">The graphics context to use for measuring the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        public static SizeD MeasureText(Graphics dc, string text, Font font)
        {
            return MeasureTextInternal(dc, text, font, SizeD.Empty, TextFormatFlags.Default, false);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <returns>The size of the text.</returns>
        public static SizeD MeasureText(string text, Font font, SizeD proposedSize)
        {
            return MeasureTextInternal(Measure, text, font, proposedSize, TextFormatFlags.Default, false);
        }
        
        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="dc">The graphics context to use for measuring the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <returns>The size of the text.</returns>
        public static SizeD MeasureText(Graphics dc, string text, Font font, SizeD proposedSize)
        {
            return MeasureTextInternal(dc, text, font, proposedSize, TextFormatFlags.Default, false);
        }

        /// <summary>
        /// Measures the size of the specified text string when drawn with the specified font and formatting options.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedSize">The maximum size of the text.</param>
        /// <param name="flags">The formatting options to use for measuring the text.</param>
        /// <returns>The size of the text.</returns>
        public static SizeD MeasureText(string text, Font font, SizeD proposedSize, TextFormatFlags flags)
        {
            return MeasureTextInternal(Measure, text, font, proposedSize, flags, false);
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
        public static SizeD MeasureText(Graphics dc, string text, Font font, SizeD proposedSize, TextFormatFlags flags)
        {
            return MeasureTextInternal(dc, text, font, proposedSize, flags, false);
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
            StringFormat sf = FlagsToStringFormat(flags);

            RectD newBounds = PadDrawStringRectangle(bounds, flags);

            dc.DrawString(text, font, foreColor.AsBrush, newBounds, sf);
        }

        internal static SizeD MeasureTextInternal(
            Graphics dc,
            string text,
            Font font,
            SizeD proposedSize,
            TextFormatFlags flags,
            bool useMeasureString)
        {
            StringFormat sf = FlagsToStringFormat(flags);

            SizeD retval;

            float proposedWidth;
            if (proposedSize.Width == 0)
                proposedWidth = Int32.MaxValue;
            else
            {
                proposedWidth = proposedSize.Width;
                if ((flags & TextFormatFlags.NoPadding) == 0)
                    proposedWidth -= 9;
            }

            retval = dc.MeasureString(text, font, proposedWidth, sf);

            if (retval.Width > 0 && !flags.HasFlag(TextFormatFlags.NoPadding))
                retval.Width += 9;

            return retval;
        }

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

        internal static SizeD MeasureTextInternal(
            Graphics dc,
            string text,
            Font font,
            TextFormatFlags flags,
            bool useMeasureString)
        {
            return MeasureTextInternal(dc, text, font, SizeD.Empty, flags, useMeasureString);
        }

        private static StringFormat FlagsToStringFormat(TextFormatFlags flags)
        {
            StringFormat sf = new ();

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

        private static RectD PadRectangle(RectD r, TextFormatFlags flags)
        {
            if (!flags.HasFlag(TextFormatFlags.NoPadding) && !flags.HasFlag(TextFormatFlags.Right)
                && !flags.HasFlag(TextFormatFlags.HorizontalCenter))
            {
                r.X += 3;
                r.Width -= 3;
            }
            if (!flags.HasFlag(TextFormatFlags.NoPadding) && flags.HasFlag(TextFormatFlags.Right))
            {
                r.Width -= 4;
            }
            if (flags.HasFlag(TextFormatFlags.LeftAndRightPadding))
            {
                r.X += 2;
                r.Width -= 2;
            }
            if (flags.HasFlag(TextFormatFlags.WordEllipsis)
                || flags.HasFlag(TextFormatFlags.EndEllipsis)
                || flags.HasFlag(TextFormatFlags.WordBreak))
            {
                r.Width -= 4;
            }
            if (flags.HasFlag(TextFormatFlags.VerticalCenter))
            {
                r.Y += 1;
            }

            return r;
        }

        private static RectD PadDrawStringRectangle(RectD r, TextFormatFlags flags)
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
