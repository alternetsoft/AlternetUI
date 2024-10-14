using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        internal interface IStyledText
        {
            void Draw(Graphics dc, PointD location, Font font, Color foreColor, Color backColor);

            void Draw(Graphics dc, PointD location, Font font, Brush brush);

            SizeD Measure(Graphics dc, Font font);
        }

        /// <summary>
        /// Draws multiple styled text items at the location with the specified
        /// font, horizontal alignment, foreground and background colors.
        /// </summary>
        /// <param name="maxWidth">
        /// Maximal width which is used when alignment is applied.
        /// When -1 is specified, it is calculated.
        /// </param>
        /// <param name="alignment">Horizontal alignment.</param>
        /// <param name="text">Strings to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="lineDistance">Distance between lines of text. Optional. Default is 0.</param>
        public virtual void DrawStyledText(
            IEnumerable<StyledText> text,
            PointD origin,
            Font font,
            Color foreColor,
            Color backColor,
            HorizontalAlignment alignment,
            Coord maxWidth = -1,
            Coord lineDistance = 0)
        {
            if (maxWidth < 0)
            {
                maxWidth = 0;

                foreach (var obj in text)
                {
                    var measure = MeasureStyledText(obj, font);
                    maxWidth = Math.Max(maxWidth, measure.Width);
                }
            }

            foreach (var obj in text)
            {
                var measure = MeasureStyledText(obj, font);
                var alignedOrigin = origin;
                if (alignment != HorizontalAlignment.Left && maxWidth > 0)
                {
                    RectD rect = (origin, measure);
                    RectD maxRect = (origin.X, origin.Y, maxWidth, measure.Height);
                    var alignedRect = AlignUtils.AlignRectInRect(rect, maxRect, alignment, null);
                    alignedOrigin.X = alignedRect.X;
                }

                DrawStyledText(obj, alignedOrigin, font, foreColor, backColor);
                origin.Y += measure.Height + lineDistance;
            }
        }

        /// <summary>
        /// Draws multiple styled text items at the location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">Items to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="brush">Default <see cref="Brush"/> that determines the default
        /// color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the text.</param>
        /// <param name="distance">Distance between items. Optional. Default is 0.</param>
        /// <param name="isVertical">Whether to draw items vertically or horizontally.</param>
        public virtual void DrawStyledText(
            IEnumerable<StyledText> text,
            Font font,
            Brush brush,
            PointD origin,
            Coord distance = 0,
            bool isVertical = true)
        {
            foreach (var obj in text)
            {
                DrawStyledText(obj, font, brush, origin);
                var textSize = MeasureStyledText(obj, font).GetSize(isVertical);
                origin.IncLocation(isVertical, textSize + distance);
            }
        }

        /// <summary>
        /// Draws multiple styled text items at the location with the specified
        /// font, foreground and background colors.
        /// </summary>
        /// <param name="text">Items to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="distance">Distance between items. Optional. Default is 0.</param>
        /// <param name="isVertical">Whether to draw items vertically or horizontally.</param>
        public virtual void DrawStyledText(
            IEnumerable<StyledText> text,
            PointD origin,
            Font font,
            Color foreColor,
            Color backColor,
            Coord distance = 0,
            bool isVertical = true)
        {
            foreach (var obj in text)
            {
                DrawStyledText(obj, origin, font, foreColor, backColor);
                var textSize = MeasureStyledText(obj, font).GetSize(isVertical);
                origin.IncLocation(isVertical, textSize + distance);
            }
        }

        /// <summary>
        /// Draws styled text object.
        /// </summary>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="brush">Default <see cref="Brush"/> that determines the default
        /// color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawStyledText(StyledText styledText, Font font, Brush brush, PointD origin)
        {
            ((IStyledText)styledText).Draw(this, origin, font, brush);
        }

        /// <summary>
        /// Measures styled text object.
        /// </summary>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureStyledText(StyledText styledText, Font font)
        {
            var measure = ((IStyledText)styledText).Measure(this, font);
            return measure;
        }

        /// <summary>
        /// Draws styled text object.
        /// </summary>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="location"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawStyledText(
            StyledText styledText,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            ((IStyledText)styledText).Draw(this, location, font, foreColor, backColor);
        }

        /// <summary>
        /// Styled text object.
        /// </summary>
        public class StyledText : BaseObject
        {
            /// <summary>
            /// Conversion from <see cref="string"/> to <see cref="StyledText"/>.
            /// </summary>
            /// <param name="value">Value to convert.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator StyledText(string value)
            {
                return Create(value);
            }

            /// <summary>
            /// Conversion from array of <see cref="string"/> to <see cref="StyledText"/>.
            /// </summary>
            /// <param name="value">Value to convert.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator StyledText(string[] value)
            {
                return Create(value);
            }

            /// <summary>
            /// Creates styled text from the collection of strings.
            /// </summary>
            /// <param name="strings">The collection of strings.</param>
            /// <returns></returns>
            public static IEnumerable<StyledText> CreateCollection(IEnumerable strings)
            {
                List<StyledText> items = new();

                foreach(var s in strings)
                {
                    items.Add(StyledText.Create(s));
                }

                return items;
            }

            /// <summary>
            /// Create styled text container with the strings.
            /// </summary>
            /// <param name="strings">Strings collection.</param>
            /// <param name="distance">Distance between items.</param>
            /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
            /// <returns></returns>
            public static StyledText Create(
                IEnumerable strings,
                Coord distance = 0,
                bool isVertical = true)
            {
                var items = CreateCollection(strings);
                return Create(items, distance, isVertical);
            }

            /// <summary>
            /// Create styled text container wiht the items.
            /// </summary>
            /// <param name="items">Items collection.</param>
            /// <param name="distance">Distance between items.</param>
            /// <param name="isVertical">Whether items are aligned vertically or horizontally.</param>
            /// <returns></returns>
            public static StyledText Create(
                            IEnumerable<StyledText> items,
                            Coord distance = 0,
                            bool isVertical = true)
            {
                return new StyledTextContainer(items, distance, isVertical);
            }

            /// <summary>
            /// Creates an empty styled text.
            /// </summary>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StyledText Create()
            {
                return new SimpleStyledText();
            }

            /// <summary>
            /// Creates styled text without font and color attributes.
            /// </summary>
            /// <param name="s">Text string without line separators.</param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StyledText Create(object s)
            {
                return new SimpleStyledText(s);
            }

            /// <summary>
            /// Creates styled text with font and foreground brush attributes.
            /// </summary>
            /// <param name="s">Text string without line separators.</param>
            /// <param name="fontStyle"><see cref="FontStyle"/> that is used to draw the text.</param>
            /// <param name="brush"><see cref="Brush"/> that determines the
            /// color and texture of the text. If this is null, default brush is used.</param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StyledText Create(object s, FontStyle fontStyle, Brush? brush)
            {
                return new StyledTextWithFontAndColor(s, fontStyle, brush);
            }

            /// <summary>
            /// Creates styled text with font and foreground brush attributes.
            /// </summary>
            /// <param name="s">Text string without line separators.</param>
            /// <param name="font"><see cref="Font"/> that is used to draw the text.</param>
            /// <param name="brush"><see cref="Brush"/> that determines the
            /// color and texture of the text. If this is null, default brush is used.</param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StyledText Create(object s, Font? font, Brush? brush)
            {
                return new StyledTextWithFontAndColor(s, font, brush);
            }

            /// <summary>
            /// Creates styled text with font style, foreground and background color attributes.
            /// </summary>
            /// <param name="s">Text string without line separators.</param>
            /// <param name="font"><see cref="Font"/> that is used to draw the text.</param>
            /// <param name="foreColor">Foreground color of the text.
            /// If this is null, default foreground color is used.
            /// </param>
            /// <param name="backColor">Background color of the text. If parameter is equal
            /// to <see cref="Color.Empty"/>, background is transparent.
            /// If this is null, default background color is used.
            /// </param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StyledText Create(
                object s,
                Font? font,
                Color? foreColor,
                Color? backColor)
            {
                return new StyledTextWithFontAndColor(s, font, foreColor, backColor);
            }

            /// <summary>
            /// Creates styled text with font style, foreground and background color attributes.
            /// </summary>
            /// <param name="s">Text string without line separators.</param>
            /// <param name="fontStyle"><see cref="FontStyle"/> that is used to draw the text.</param>
            /// <param name="foreColor">Foreground color of the text.
            /// If this is null, default foreground color is used.
            /// </param>
            /// <param name="backColor">Background color of the text. If parameter is equal
            /// to <see cref="Color.Empty"/>, background is transparent.
            /// If this is null, default background color is used.
            /// </param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StyledText Create(
                object s,
                FontStyle fontStyle,
                Color? foreColor,
                Color? backColor)
            {
                return new StyledTextWithFontAndColor(s, fontStyle, foreColor, backColor);
            }
        }

        internal class StyledTextContainer : StyledText, IStyledText
        {
            private readonly bool isVertical;
            private readonly IEnumerable<StyledText> items;
            private readonly Coord distance;

            public StyledTextContainer(
                IEnumerable<StyledText> items,
                Coord distance = 0,
                bool isVertical = true)
            {
                this.isVertical = isVertical;
                this.items = items;
                this.distance = distance;
            }

            public void Draw(Graphics dc, PointD location, Font font, Color foreColor, Color backColor)
            {
                dc.DrawStyledText(
                    items,
                    location,
                    font,
                    foreColor,
                    backColor,
                    distance,
                    isVertical);
            }

            public void Draw(Graphics dc, PointD location, Font font, Brush brush)
            {
                dc.DrawStyledText(
                    items,
                    font,
                    brush,
                    location,
                    distance,
                    isVertical);
            }

            public SizeD Measure(Graphics dc, Font font)
            {
                Coord width = 0;
                Coord height = 0;

                if (isVertical)
                {
                    foreach (var s in items)
                    {
                        var size = dc.MeasureStyledText(s, font);
                        width = Math.Max(width, size.Width);
                        height += size.Height + distance;
                    }
                }
                else
                {
                    foreach (var s in items)
                    {
                        var size = dc.MeasureStyledText(s, font);
                        height = Math.Max(height, size.Height);
                        width += size.Width + distance;
                    }
                }

                return (width, height);
            }
        }

        internal class SimpleStyledText : StyledText, IStyledText
        {
            private object text;
            private SizeD? measure;
            private Coord scaleFactor;
            private ObjectUniqueId defaultFont;

            public SimpleStyledText(object text)
            {
                this.text = text;
            }

            public SimpleStyledText()
            {
                text = string.Empty;
            }

            public virtual object Text
            {
                get
                {
                    return text;
                }

                set
                {
                    if (text == value)
                        return;
                    text = value;
                    Changed();
                }
            }

            public virtual void Draw(
                Graphics dc,
                PointD location,
                Font font,
                Color foreColor,
                Color backColor)
            {
                dc.DrawText(
                    Text.ToString(),
                    location,
                    SafeFont(font),
                    SafeForeColor(foreColor),
                    SafeBackColor(backColor));
            }

            public virtual void Draw(Graphics dc, PointD location, Font font, Brush foreBrush)
            {
                dc.DrawText(Text.ToString(), SafeFont(font), SafeForeBrush(foreBrush), location);
            }

            public virtual SizeD Measure(Graphics dc, Font font)
            {
                font = SafeFont(font);
                var fontId = font.UniqueId;
                var newScaleFactor = dc.ScaleFactor;

                if (measure is null || defaultFont != fontId || scaleFactor != newScaleFactor)
                {
                    defaultFont = fontId;
                    measure = dc.MeasureText(Text.ToString(), font);
                    scaleFactor = newScaleFactor;
                }

                return measure.Value;
            }

            public virtual Font SafeFont(Font font)
            {
                return font ?? Control.DefaultFont;
            }

            public virtual Brush SafeForeBrush(Brush brush)
            {
                return brush ?? Brush.Default;
            }

            public virtual Color SafeForeColor(Color foreColor)
            {
                return foreColor ?? Color.Black;
            }

            public virtual Color SafeBackColor(Color backColor)
            {
                return backColor ?? Color.Empty;
            }

            public virtual void Changed()
            {
                measure = null;
            }
        }

        internal class StyledTextWithFontAndColor : SimpleStyledText
        {
            private Font? font;
            private FontStyle? fontStyle;
            private Color? foregroundColor;
            private Color? backgroundColor;
            private Brush? foregroundBrush;

            public StyledTextWithFontAndColor()
                : base()
            {
            }

            public StyledTextWithFontAndColor(object s)
                : base(s)
            {
            }

            public StyledTextWithFontAndColor(object s, FontStyle? fontStyle, Brush? foreBrush)
                : base(s)
            {
                this.fontStyle = fontStyle;
                foregroundBrush = foreBrush;
            }

            public StyledTextWithFontAndColor(
                object s,
                FontStyle? fontStyle,
                Color? foreColor,
                Color? backColor)
                : base(s)
            {
                this.fontStyle = fontStyle;
                foregroundColor = foreColor;
                backgroundColor = backColor;
            }

            public StyledTextWithFontAndColor(object s, Font? font, Brush? foreBrush)
                : base(s)
            {
                this.font = font;
                foregroundBrush = foreBrush;
            }

            public StyledTextWithFontAndColor(object s, Font? font, Color? foreColor, Color? backColor)
                : base(s)
            {
                this.font = font;
                foregroundColor = foreColor;
                backgroundColor = backColor;
            }

            public virtual Color? ForegroundColor
            {
                get
                {
                    return foregroundColor;
                }

                set
                {
                    foregroundColor = value;
                }
            }

            public virtual Color? BackgroundColor
            {
                get
                {
                    return backgroundColor;
                }

                set
                {
                    backgroundColor = value;
                }
            }

            public virtual Brush? ForegroundBrush
            {
                get
                {
                    return foregroundBrush;
                }

                set
                {
                    foregroundBrush = value;
                }
            }

            public virtual Font? Font
            {
                get
                {
                    return font;
                }

                set
                {
                    if (font == value)
                        return;
                    font = value;
                    Changed();
                }
            }

            public virtual FontStyle? FontStyle
            {
                get
                {
                    return fontStyle;
                }

                set
                {
                    if (fontStyle == value)
                        return;
                    fontStyle = value;
                    Changed();
                }
            }

            public override void Changed()
            {
                base.Changed();
            }

            public override Font SafeFont(Font font)
            {
                if (this.font is not null)
                    return this.font;

                var baseFont = base.SafeFont(font);

                if(fontStyle is null)
                    return baseFont;
                return baseFont.WithStyle(fontStyle.Value);
            }

            public override Brush SafeForeBrush(Brush brush)
            {
                return foregroundBrush ?? base.SafeForeBrush(brush);
            }

            public override Color SafeForeColor(Color foreColor)
            {
                return foregroundColor ?? base.SafeForeColor(foreColor);
            }

            public override Color SafeBackColor(Color backColor)
            {
                return backgroundColor ?? base.SafeBackColor(backColor);
            }
        }
    }
}
