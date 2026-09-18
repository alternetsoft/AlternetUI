using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Skia;
using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Specifies the default vertical text direction used when no direction is provided.
        /// </summary>
        public static VerticalTextDirection DefaultVertTextDirection = VerticalTextDirection.TopToBottom;

        /// <summary>
        /// Gets or sets whether debug corners are painted in some of the graphics methods.
        /// </summary>
        public static bool DrawDebugCorners = false;

#if DEBUG
        /// <summary>
        /// Internal use only.
        /// </summary>
        public static ObjectUniqueId? DebugElementId;
#endif

        /// <inheritdoc cref="DrawText(ReadOnlySpan{char}, Font, Brush, RectD)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, RectD layoutRectangle)
        {
            DrawText(s, font, brush, layoutRectangle);
        }

        /// <inheritdoc cref="DrawText(ReadOnlySpan{char}, Font, Brush, PointD)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, PointD point)
        {
            DrawText(s, font, brush, point);
        }

        /// <summary>
        /// Draws the text string at the specified location using the specified font and brush.
        /// </summary>
        /// <param name="s">The text to draw.</param>
        /// <param name="font">The font to use.</param>
        /// <param name="brush">The brush to use.</param>
        /// <param name="x">The x-coordinate of the location.</param>
        /// <param name="y">The y-coordinate of the location.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, float x, float y)
        {
            DrawText(s, font, brush, new PointD(x, y));
        }

        /// <summary>
        /// Returns the size of a single character when drawn with the specified font.
        /// </summary>
        /// <param name="ch">The character to measure.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SizeD"/> structure representing the width and height of the character.
        /// </returns>
        public SizeD CharSize(char ch, Font font)
        {
#pragma warning disable
            Span<char> buffer = stackalloc char[1];
            buffer[0] = ch;
#pragma warning restore
            return GetTextExtent(buffer, font);
        }

        /// <summary>
        /// Returns the size of a pair of identical characters when drawn with the specified font.
        /// </summary>
        /// <param name="ch">The character to measure as a pair.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SizeD"/> structure representing the width and height of the character pair.
        /// </returns>
        public SizeD CharPairSize(char ch, Font font)
        {
#pragma warning disable
            Span<char> buffer = stackalloc char[2];
            buffer[0] = ch;
            buffer[1] = ch;
#pragma warning restore
            return GetTextExtent(buffer, font);
        }

        /// <summary>
        /// Calculates the size of a single character and the spacing between two consecutive characters.
        /// </summary>
        /// <param name="ch">The character to measure.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <param name="charSize">When this method returns, contains the size of the character.</param>
        /// <param name="spacing">When this method returns, contains the spacing between
        /// two consecutive characters.</param>
        public void InterCharSpacing(char ch, Font font, out SizeD charSize, out Coord spacing)
        {
            charSize = CharSize(ch, font);
            spacing = CharPairSize(ch, font).Width - (2 * charSize.Width);
        }

        /// <summary>
        /// Returns the size of a sequence of identical characters when drawn with the specified font.
        /// </summary>
        /// <param name="ch">The character to measure.</param>
        /// <param name="count">The number of times the character is repeated.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SizeD"/> structure representing the width and height of the repeated characters.
        /// </returns>
        public SizeD CharSize(char ch, int count, Font font)
        {
            if (count == 1)
                return CharSize(ch, font);
            if (count <= 0)
                return SizeD.Empty;

            SizeD result = SizeD.Empty;

            SkiaHelper.InvokeWithFilledSpan(
                count,
                ch,
                span =>
                {
                    result = GetTextExtent(span, font);
                });

            return result;
        }

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        public abstract SizeD GetTextExtent(ReadOnlySpan<char> text, Font font);

        /// <summary>
        /// Draws text with the specified angle, font, background and foreground colors.
        /// </summary>
        /// <param name="location">Location used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        /// <param name="angle">The angle, in degrees, relative to the (default) horizontal
        /// direction to draw the string.</param>
        public abstract void DrawTextWithAngle(
            ReadOnlySpan<char> text,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor,
            Coord angle);

        /// <summary>
        /// Draws text string with the specified bounds, <see cref="Brush"/>
        /// and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture
        /// of the drawn text.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that specifies the bounds of
        /// the drawn text.</param>
        public abstract void DrawText(ReadOnlySpan<char> text, Font font, Brush brush, RectD bounds);

        /// <summary>
        /// Draws text string with the specified location, <see cref="Brush"/>
        /// and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the text position on the canvas.</param>
        public abstract void DrawText(ReadOnlySpan<char> text, Font font, Brush brush, PointD origin);

        /// <summary>
        /// Draws text with the specified font, background and foreground colors.
        /// This is the fastest method to draw text.
        /// </summary>
        /// <param name="location">Location used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        public abstract void DrawText(
            ReadOnlySpan<char> text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor);

        /// <summary>
        /// Draws text with <see cref="AbstractControl.DefaultFont"/> and <see cref="Brush.Default"/>.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the text position on the canvas.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(ReadOnlySpan<char> text, PointD origin)
        {
            DrawText(text, Control.DefaultFont, Brush.Default, origin);
        }

        /// <summary>
        /// Draws text inside the bounds with the specified font, background and foreground colors.
        /// </summary>
        /// <param name="rect">Bounding rectangle used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        public virtual void DrawText(
            ReadOnlySpan<char> text,
            RectD rect,
            Font font,
            Color foreColor,
            Color backColor)
        {
            Save();

            try
            {
                ClipRect(rect);
                DrawText(
                    text,
                    rect.Location,
                    font,
                    foreColor,
                    backColor);
            }
            finally
            {
                Restore();
            }
        }

        /// <summary>
        /// Draws the text string at the specified location with
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="rect"><see cref="RectD"/> structure that specifies the bounds
        /// of the text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as alignment and trimming, that are applied to the drawn text.</param>
        /// <remarks>
        /// You can pass 0 as width of the <paramref name="rect"/>. In this case wrapping
        /// will not be performed, only line breaks will be applied.
        /// </remarks>
        /// <remarks>
        /// You can pass 0 as height of the <paramref name="rect"/>.
        /// </remarks>
        public RectD DrawText(
            ReadOnlySpan<char> text,
            Font font,
            Brush brush,
            RectD rect,
            TextFormat format)
        {
            return DrawText(text, font, brush, rect, format.AsRecord);
        }

        /// <summary>
        /// Calculates the size of the text string when drawn with the specified font and formatting attributes.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="rect">The bounds of the text.</param>
        /// <param name="format">The formatting attributes to apply to the text.</param>
        /// <returns>The size of the text.</returns>
        public virtual SizeD MeasureText(
            ReadOnlySpan<char> text,
            Font font,
            RectD rect,
            in TextFormat.Record format)
        {
            var result = DrawText(text, font, null, rect, in format);
            return result.Size;
        }

        /// <summary>
        /// Calculates the size of the text string when drawn with the specified font and formatting attributes.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="proposedWidth">The maximum width of the text.</param>
        /// <param name="format">The formatting attributes to apply to the text.</param>
        /// <returns>The size of the text.</returns>
        public virtual SizeD MeasureText(
            ReadOnlySpan<char> text,
            Font font,
            float proposedWidth,
            in TextFormat.Record format)
        {
            var result = DrawText(text, font, null, new RectD(0, 0, proposedWidth, PointD.HalfOfMaxValue.Y), in format);
            return result.Size;
        }

        /// <summary>
        /// Draws the text string at the specified location with
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="rect"><see cref="RectD"/> structure that specifies the bounds
        /// of the text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as alignment and trimming, that are applied to the drawn text.</param>
        /// <remarks>
        /// You can pass 0 as width of the <paramref name="rect"/>. In this case wrapping
        /// will not be performed, only line breaks will be applied.
        /// </remarks>
        /// <remarks>
        /// You can pass 0 as height of the <paramref name="rect"/>.
        /// </remarks>
        public virtual RectD DrawText(
            ReadOnlySpan<char> text,
            Font font,
            Brush? brush,
            RectD rect,
            in TextFormat.Record format)
        {
            if (text.IsEmpty)
                return rect.WithEmptySize();

            var document = SafeDocument;
            var wrappedText = document.WrappedText;

            if (rect.HasEmptyWidth || rect.Width == int.MaxValue || rect.Size.IsNanWidth)
                rect.Width = MaxCoord;

            if (rect.HasEmptyHeight || rect.Height == int.MaxValue || rect.Size.IsNanHeight)
                rect.Height = MaxCoord;

            wrappedText.SuspendLayout();
            try
            {
                document.Size = rect.Size;
                wrappedText.SetFormat(in format);
                wrappedText.Text = string.Empty;
                wrappedText.Text = text.ToString();
                wrappedText.Font = font;

                if (brush is null)
                {
                    wrappedText.ForegroundColor = null;
                }
                else
                {
                    document.DrawTextColor.SetColors(brush.AsColor);
                    wrappedText.ForegroundColor = document.DrawTextColor;
                }
            }
            finally
            {
                wrappedText.ResumeLayout(true, true);
            }

            if (brush is null)
            {
                var result = wrappedText.MeasureText(wrappedText.MeasureCanvas, font, rect.Size);
                return new(rect.Location, result);
            }
            else
            {
                TemplateUtils.RaisePaintClipped(wrappedText, this, rect.Location, isClipped: true);
                var result = wrappedText.Bounds.WithLocation(rect.Location);
                return result;
            }
        }

        /// <summary>
        /// Draws text with html bold tags.
        /// </summary>
        public virtual SizeD DrawTextWithBoldTags(
            ReadOnlySpan<char> text,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            var convertedText = RegexUtils.GetBoldTagSplitted(text);
            return DrawTextWithFontStyle(
                        convertedText,
                        location,
                        font,
                        foreColor,
                        backColor);
        }

        /// <summary>
        /// Draws multiple lines of text, each with individual font style segments,
        /// at the specified location and
        /// returns the size of each rendered line.
        /// </summary>
        /// <remarks>The method draws each line sequentially, offsetting the Y-coordinate by the height of
        /// the previous line plus the specified line distance. Each text segment
        /// within a line can have its own font
        /// style, but the base font is used when no style is specified.</remarks>
        /// <param name="text">An array of text lines, where each line is represented
        /// as an array of text segments with associated font
        /// styles to be drawn.</param>
        /// <param name="lineDistance">The vertical distance, in device-independent units,
        /// to apply between each line of text.</param>
        /// <param name="location">The starting location, in device-independent coordinates,
        /// where the first line of text will be drawn.</param>
        /// <param name="font">The base font to use for rendering text segments that
        /// do not specify an explicit font style.</param>
        /// <param name="foreColor">The color to use for the text foreground.</param>
        /// <param name="backColor">The background color to use behind the text.
        /// If null, no background is drawn.</param>
        /// <returns>An array of SizeD values representing the width and height
        /// of each rendered text line, in the same order as
        /// the input lines.</returns>
        public virtual SizeD[] DrawTextLinesWithFontStyle(
            TextAndFontStyle[][] text,
            Coord lineDistance,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            var result = new SizeD[text.Length];

            for (var i = 0; i < text.Length; i++)
            {
                var line = text[i];
                var size = DrawTextWithFontStyle(line, location, font, foreColor, backColor);
                result[i] = size;
                location.Y += size.Height + lineDistance;
            }

            return result;
        }

        /// <summary>
        /// Draws an array of text elements with custom font styles and colors.
        /// </summary>
        public virtual SizeD DrawTextWithFontStyle(
            TextAndFontStyle[] text,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            DebugFontAssert(font);

            SizeD result = 0;

            bool visible = foreColor.IsOk && (foreColor != Color.Empty);

            for (int i = 0; i < text.Length; i++)
            {
                var item = text[i];

                var itemFont = font.WithStyle(item.FontStyle);
                var measure = GetTextExtent(item.Text, itemFont);

                if (visible)
                {
                    var myForeColor = item.ForeColor ?? foreColor;
                    var myBackColor = item.BackColor ?? backColor ?? Color.Empty;

                    DrawText(item.Text, location, itemFont, myForeColor, myBackColor);
                }

                text[i].MeasuredBounds = new RectD(location, measure);

                location.X += measure.Width;
                result.Width += measure.Width;
                result.Height = Math.Max(result.Height, measure.Height);
            }

            return result;
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted.</param>
        /// <param name="image">Optional image.</param>
        /// <param name="rect">Rectangle in which drawing is performed.</param>
        /// <param name="alignment">Alignment of the text.</param>
        /// <param name="indexAccel">Index of underlined mnemonic character.</param>
        /// <returns>The bounding rectangle.</returns>
        public virtual RectD DrawLabel(
            ReadOnlySpan<char> text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            HVAlignment? alignment = null,
            int indexAccel = -1)
        {
            DrawLabelParams prm = new(
                text.ToString(),
                font,
                foreColor,
                backColor,
                image,
                rect,
                alignment,
                indexAccel);

            var result = DrawLabel(ref prm);
            return result;
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors, and an array of images.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="foreColor">The foreground color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <param name="images">An array of images to draw alongside the text.</param>
        /// <param name="rect">The rectangle in which to draw the text and images.</param>
        /// <param name="alignment">The alignment of the text within the rectangle.</param>
        /// <param name="indexAccel">The index of the underlined mnemonic character.</param>
        /// <returns>The bounding rectangle of the drawn text and images.</returns>
        public virtual RectD DrawLabelWithImages(
            ReadOnlySpan<char> text,
            Font font,
            Color foreColor,
            Color backColor,
            Image[]? images,
            RectD rect,
            HVAlignment? alignment = null,
            int indexAccel = -1)
        {
            DrawLabelParams prm = new(
                text.ToString(),
                font,
                foreColor,
                backColor,
                null,
                rect,
                alignment,
                indexAccel);

            prm.SetImages(images);

            var result = DrawLabel(ref prm);
            return result;
        }

        /// <summary>
        /// Draws text vertically starting from the bottom of the specified rectangle using the given font and colors.
        /// Text is centered within the rectangle, and the method handles
        /// rotation and translation to achieve the desired orientation.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="foreColor">The foreground color of the text.</param>
        /// <param name="rect">The rectangle in which to draw the text.</param>
        public virtual void DrawVertTextFromBottom(ReadOnlySpan<char> text, Font font, Color foreColor, RectD rect)
        {
            var state = Save();
            RotateTransform(-90);
            TranslateTransform(0, rect.Top + rect.Bottom, MatrixOrder.Append);

            var strSize = MeasureText(text, font);

            PointD p = new(
                rect.Top + Math.Max(0, (rect.Height - strSize.Width) / 2),
                rect.Left + ((rect.Width - strSize.Height) / 2));

            var r = new RectD(p, new SizeD(rect.Height, rect.Width));

            DrawString(text, font, foreColor.AsBrush, r);
            Restore(state);
        }

        /// <summary>
        /// Draws text vertically in the specified rectangle using the given font and colors,
        /// based on the specified vertical text direction.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="foreColor">The foreground color of the text.</param>
        /// <param name="rect">The rectangle in which to draw the text.</param>
        /// <param name="direction">The direction in which to draw the vertical text.</param>
        public virtual void DrawVertText(
            ReadOnlySpan<char> text,
            Font font,
            Color foreColor,
            RectD rect,
            VerticalTextDirection? direction)
        {
            if ((direction ?? DefaultVertTextDirection) == VerticalTextDirection.BottomToTop)
            {
                DrawVertTextFromBottom(text, font, foreColor, rect);
            }
            else
            {
                DrawVertTextFromTop(text, font, foreColor, rect);
            }
        }

        /// <summary>
        ///  Draws the specified text at the specified location with the specified <see cref="Brush"/> and
        ///  <see cref="Font"/> objects using the formatting attributes of the specified <see cref="StringFormat"/>.
        /// </summary>
        /// <param name="s">The text to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of the drawn text.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
        /// <param name="format">
        ///  <see cref="StringFormat"/> that specifies formatting attributes, such as line spacing and alignment,
        ///  that are applied to the drawn text.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <paramref name="brush"/> is <see langword="null"/>. -or- <paramref name="font"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, float x, float y, StringFormat? format) =>
            DrawString(s, font, brush, new RectD(x, y, 0, 0), format);

        /// <summary>
        ///  Draws the specified text at the specified location with the specified <see cref="Brush"/> and
        ///  <see cref="Font"/> objects using the formatting attributes of the specified <see cref="StringFormat"/>.
        /// </summary>
        /// <param name="s">The text to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of the drawn text.</param>
        /// <param name="point"><see cref="PointD"/>structure that specifies the upper-left corner of the drawn text.</param>
        /// <param name="format">
        ///  <see cref="StringFormat"/> that specifies formatting attributes, such as line spacing and alignment,
        ///  that are applied to the drawn text.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <paramref name="brush"/> is <see langword="null"/>. -or- <paramref name="font"/> is <see langword="null"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DrawString(ReadOnlySpan<char> s, Font font, Brush brush, PointD point, StringFormat? format) =>
            DrawString(s, font, brush, new RectD(point.X, point.Y, 0, 0), format);

        /// <summary>
        ///  Draws the specified text in the specified rectangle with the specified <see cref="Brush"/> and
        ///  <see cref="Font"/> objects using the formatting attributes of the specified <see cref="StringFormat"/>.
        /// </summary>
        /// <param name="s">The text to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of the drawn text.</param>
        /// <param name="layoutRectangle"><see cref="RectD"/>structure that specifies the location of the drawn text.</param>
        /// <param name="format">
        ///  <see cref="StringFormat"/> that specifies formatting attributes, such as line spacing and alignment,
        ///  that are applied to the drawn text.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <paramref name="brush"/> is <see langword="null"/>. -or- <paramref name="font"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        ///  <para>
        ///   The text represented by the <paramref name="s"/> parameter is drawn inside the rectangle represented by
        ///   the <paramref name="layoutRectangle"/> parameter. If the text does not fit inside the rectangle, it is
        ///   truncated at the nearest word, unless otherwise specified with the <paramref name="format"/> parameter.
        ///  </para>
        /// </remarks>
        internal void DrawString(
            ReadOnlySpan<char> s,
            Font font,
            Brush brush,
            RectD layoutRectangle,
            StringFormat? format)
        {
            throw new NotImplementedException("DrawString with StringFormat is not implemented in this Graphics class.");
        }

        /// <summary>
        /// Calculates the size of the specified text string when drawn with the specified font and formatting attributes.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="layoutArea">The maximum layout area for the text.</param>
        /// <param name="stringFormat">Formatting information for the text.</param>
        /// <returns>The size of the text string when drawn with the specified font and formatting attributes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal SizeD MeasureString(ReadOnlySpan<char> text, Font font, SizeD layoutArea, StringFormat? stringFormat)
        {
            throw new NotImplementedException("MeasureString with StringFormat is not implemented in this Graphics class.");
        }

        /// <summary>
        /// Calculates the size of the specified text string when drawn with the specified font and formatting attributes.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="font">The font to use for measuring the text.</param>
        /// <param name="width">The maximum width for the text layout.</param>
        /// <param name="format">Formatting information for the text.</param>
        /// <returns>The size of the text string when drawn with the specified font and formatting attributes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal SizeD MeasureString(ReadOnlySpan<char> text, Font font, int width, StringFormat? format) =>
            MeasureString(text, font, new SizeD(width, 999999), format);

        /// <summary>
        /// Draws text vertically starting from the top of the specified rectangle using the given font and colors.
        /// Text is centered within the rectangle, and the method handles
        /// rotation and translation to achieve the desired orientation.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The font to use for drawing the text.</param>
        /// <param name="foreColor">The foreground color of the text.</param>
        /// <param name="rect">The rectangle in which to draw the text.</param>
        public virtual void DrawVertTextFromTop(ReadOnlySpan<char> text, Font font, Color foreColor, RectD rect)
        {
            var state = Save();

            RotateTransform(90);

            TranslateTransform(rect.Right, rect.Top, MatrixOrder.Append);

            var strSize = MeasureText(text, font);

            PointD p = new(
                Math.Max(0, (rect.Height - strSize.Width) / 2),
                ((rect.Width - strSize.Height) / 2));

            var r = new RectD(p, new SizeD(rect.Height, rect.Width));

            DrawString(text, font, foreColor.AsBrush, r);

            Restore(state);
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// </summary>
        /// <param name="prm">Parameters specified using
        /// <see cref="DrawLabelParams"/> structure.</param>
        /// <returns></returns>
        public virtual RectD DrawLabel(ref DrawLabelParams prm)
        {
            RectD result = RectD.Empty;

            if (prm.Rect.SizeIsEmpty || prm.Rect.SizeIsPositiveInfinity)
            {
                result = DrawLabelUnclipped(ref prm);
            }
            else
            {
                try
                {
                    Save();
                    ClipRect(prm.Rect);
                    result = DrawLabelUnclipped(ref prm);
                }
                finally
                {
                    Restore();
                }
            }

            return result;
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// This is unclipped version of <see cref="DrawLabel(ref DrawLabelParams)"/>.
        /// </summary>
        /// <param name="prm">Parameters specified using
        /// <see cref="DrawLabelParams"/> structure.</param>
        /// <returns></returns>
        public virtual RectD DrawLabelUnclipped(ref DrawLabelParams prm)
        {
            var image = prm.Image;

            DrawElementsParams drawParams = new();

            if (image is null)
            {
                if (prm.TextVisible)
                    drawParams.Elements = [DrawElementParams.CreateTextElement(ref prm)];
            }
            else
            {
                var additionalImages = prm.AdditionalImages;

                if (additionalImages is null || additionalImages.Count == 0)
                {
                    var imageElement = DrawElementParams.CreateImageElement(ref prm, ref prm.ImageParams);

                    if (prm.TextVisible)
                    {
                        var textElement = DrawElementParams.CreateTextElement(ref prm);

                        drawParams.Elements = prm.IsImageAfterText
                            ? [textElement, imageElement] : [imageElement, textElement];
                    }
                    else
                    {
                        drawParams.Elements = [imageElement];
                    }
                }
                else
                {
                    List<DrawElementParams> elements = new(additionalImages.Count + 1);
                    var textElement = DrawElementParams.CreateTextElement(ref prm);

                    if (prm.IsImageAfterText)
                    {
                        elements.Add(textElement);
                    }

                    var imageElement = DrawElementParams.CreateImageElement(ref prm, ref prm.ImageParams);
                    elements.Add(imageElement);

                    if (additionalImages is not null)
                    {
                        for (int i = 0; i < additionalImages.Count; i++)
                        {
                            var p = additionalImages[i];
                            imageElement = DrawElementParams.CreateImageElement(ref prm, ref p);
                            elements.Add(imageElement);
                        }
                    }

                    if (!prm.IsImageAfterText)
                    {
                        elements.Add(textElement);
                    }

                    drawParams.Elements = elements.ToArray();
                }
            }

            if (prm.PrefixElements is not null || prm.SuffixElements is not null)
            {
                drawParams.Elements = ArrayUtils.CombineArrays<DrawElementParams>(
                    prm.PrefixElements,
                    drawParams.Elements,
                    prm.SuffixElements);
            }

            drawParams.IsVertical = prm.IsVertical;
            drawParams.Distance = prm.ImageLabelDistance;
            drawParams.Rect = prm.Rect;
            drawParams.Alignment = prm.Alignment;
            drawParams.Visible = prm.Visible;
            drawParams.DrawDebugCorners = prm.DrawDebugCorners;
            drawParams.DebugId = prm.DebugId;

            var result = DrawElements(ref drawParams);

            prm.ResultRects = drawParams.ResultRects;
            prm.ResultSizes = drawParams.ResultSizes;
            prm.ResultBounds = result;
            prm.ImageLabelDistance = drawParams.Distance;

            return result;
        }

        /// <summary>
        /// Draws range of strings.
        /// </summary>
        /// <param name="rect">The bounding rectangle used to specify location
        /// and maximal width of the text. Height of the rectangle is not used. If width
        /// of the rectangle is not specified <paramref name="textHorizontalAlignment"/>
        /// is not used.</param>
        /// <param name="font">The font used to draw the text.</param>
        /// <param name="wrappedText">The text to draw.</param>
        /// <param name="textHorizontalAlignment">The horizontal alignment of the text</param>
        /// <param name="lineDistance">The vertical distance between the lines of text.
        /// Optional. If not specified, 0 is used.</param>
        /// <param name="foreColor">The foreground color of the text. If <c>null</c>, text
        /// will be only measured and no drawing will be performed.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <returns></returns>
        public virtual SizeD DrawStrings(
            RectD rect,
            Font font,
            IEnumerable<string>? wrappedText,
            TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left,
            Coord lineDistance = 0,
            Color? foreColor = null,
            Color? backColor = null)
        {
            var wrappedWidth = 0;

            if (wrappedText is null)
                return SizeD.Empty;

            var origin = rect.Location;
            SizeD totalMeasure = (wrappedWidth, 0);

            Coord? emptyStringMeasure = null;

            foreach (var s in wrappedText)
            {
                SizeD measure;

                var isEmpty = s is null || s.Length == 0;

                if (isEmpty)
                {
                    emptyStringMeasure ??= font.GetHeight(this);
                    measure = (CoordD.Empty, emptyStringMeasure.Value);
                }
                else
                {
                    measure = MeasureText(s!, font).Ceiling();
                }

                if (!isEmpty && foreColor is not null)
                {
                    PointD location;

                    if (rect.SizeIsEmpty)
                    {
                        location = origin;
                    }
                    else
                    {
                        RectD itemRect = (origin, measure);
                        RectD itemContainer = itemRect;
                        itemContainer.Width = rect.Width;

                        var alignment = AlignUtils.Convert(textHorizontalAlignment);

                        var alignedItemRect = AlignUtils.AlignRectInRect(
                            false,
                            itemRect,
                            itemContainer,
                            (CoordAlignment)alignment);
                        location = alignedItemRect.Location;
                    }

                    DrawText(
                        s!,
                        location,
                        font,
                        foreColor,
                        backColor ?? Color.Empty);
                }

                var increment = measure.Height + lineDistance;
                origin.Y += increment;
                totalMeasure.Height += increment;

                totalMeasure.Width = Math.Max(totalMeasure.Width, measure.Width);
            }

            return totalMeasure.ApplyMax(rect.Size);
        }

        /// <summary>
        /// Draws array of elements specified with <see cref="DrawElementsParams"/>.
        /// </summary>
        /// <param name="prm">Method arguments.</param>
        /// <returns></returns>
        public virtual RectD DrawElements(ref DrawElementsParams prm)
        {
            var drawDebugCorners = prm.DrawDebugCorners;
            var visible = prm.Visible;

            var length = prm.Elements.Length;
            if (length == 0)
                return prm.Rect;

            SizeD[] elementSizes = new SizeD[length];

            for (int i = 0; i < length; i++)
            {
                elementSizes[i] = prm.Elements[i].GetRealSize(this);
            }

            prm.ResultSizes = elementSizes;

            var elementDistance = prm.Distance ?? SpeedButton.DefaultImageLabelDistance;
            prm.Distance = elementDistance;
            Coord sumDistance = elementDistance * (length - 1);

            var sumSize = SizeD.Sum(elementSizes);
            var maxSize = SizeD.MaxWidthHeights(elementSizes);

            SizeD size;

            if (prm.IsVertical)
            {
                size = (maxSize.Width, sumSize.Height + sumDistance);
            }
            else
            {
                size = (sumSize.Width + sumDistance, maxSize.Height);
            }

            RectD beforeAlign = (prm.Rect.Location, size);
            RectD afterAlign;

            if (prm.Rect.Size.AnyIsEmptyOrNegative)
            {
                afterAlign = beforeAlign;
            }
            else
            {
                afterAlign = AlignUtils.AlignRectInRect(
                    beforeAlign,
                    prm.Rect,
                    prm.Alignment.Horizontal,
                    prm.Alignment.Vertical,
                    shrinkSize: false);
            }

#if DEBUG
            if (drawDebugCorners)
            {
                BorderSettings.DrawDesignCorners(
                    this,
                    afterAlign,
                    SystemSettings.AppearanceIsDark,
                    BorderSettings.DebugBorder);
            }
#endif

            prm.ResultBounds = afterAlign;

            RectD[] bounds = new RectD[length];
            prm.ResultRects = bounds;

            var rect = afterAlign;

            for (int i = 0; i < length; i++)
            {
                var element = prm.Elements[i];

                var elementSize = elementSizes[i];
                RectD elementBeforeAlign = (rect.Location, elementSize);

#if DEBUG
                AlignUtils.DebugIdentifier = element.DebugIdentifier;
#endif

                if (prm.IsVertical)
                {
                    var elementAfterAlign = AlignUtils.AlignRectInRect(
                        elementBeforeAlign,
                        rect,
                        element.Alignment.Horizontal,
                        VerticalAlignment.Top,
                        shrinkSize: false);
                    bounds[i] = elementAfterAlign;
                    DrawElement(in element, elementAfterAlign);
                    rect.Y = elementAfterAlign.Bottom + elementDistance;
                    rect.Height = rect.Height - elementAfterAlign.Height - elementDistance;
                }
                else
                {
                    var elementAfterAlign = AlignUtils.AlignRectInRect(
                        elementBeforeAlign,
                        rect,
                        element.Alignment.Horizontal,
                        element.Alignment.Vertical,
                        shrinkSize: false);
                    bounds[i] = elementAfterAlign;
                    DrawElement(in element, elementAfterAlign);
                    if (element.Alignment.Horizontal == HorizontalAlignment.Left)
                    {
                        rect.X = elementAfterAlign.Right + elementDistance;
                    }
                    else
                    {
                    }

                    rect.Width = rect.Width - elementAfterAlign.Width - elementDistance;
                }
            }

            void DrawElement(in DrawElementParams element, RectD rect)
            {
                if (!visible)
                    return;
                element.Draw(this, rect);

#if DEBUG
                if (!drawDebugCorners)
                    return;
                BorderSettings.DrawDesignCorners(this, rect, SystemSettings.AppearanceIsDark, BorderSettings.DebugBorder);
#endif
            }

            return afterAlign;
        }
    }
}
