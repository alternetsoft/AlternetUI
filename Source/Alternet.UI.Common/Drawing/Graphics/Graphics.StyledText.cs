using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Draws multiple text strings at the location with the specified
        /// font, horizontal alignment, foreground and background colors.
        /// </summary>
        /// <param name="maxWidth">
        /// Maximal width which is used when alignment is applied.
        /// When -1 is specified, it is calculated.
        /// </param>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="text">Strings to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="lineDistance">Distance between lines of text. Optional. Default is 0.</param>
        public virtual void DrawStyledTextLines(
            IEnumerable text,
            PointD origin,
            Font font,
            Color foreColor,
            Color backColor,
            HorizontalAlignment horz,
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
                if (horz != HorizontalAlignment.Left && maxWidth > 0)
                {
                    RectD rect = (origin, measure);
                    RectD maxRect = (origin.X, origin.Y, maxWidth, measure.Height);
                    var alignedRect = AlignUtils.AlignRectInRect(rect, maxRect, horz, null);
                    alignedOrigin.X = alignedRect.X;
                }

                DrawStyledText(obj, alignedOrigin, font, foreColor, backColor);
                origin.Y += measure.Height + lineDistance;
            }
        }

        /// <summary>
        /// Draws multiple text strings at the specified location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">Strings to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="brush">Default <see cref="Brush"/> that determines the default
        /// color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="lineDistance">Distance between lines of text. Optional. Default is 0.</param>
        public virtual void DrawStyledTextLines(
            IEnumerable text,
            Font font,
            Brush brush,
            PointD origin,
            Coord lineDistance = 0)
        {
            foreach (var obj in text)
            {
                DrawStyledText(obj, font, brush, origin);
                origin.Y += MeasureStyledText(obj, font).Height + lineDistance;
            }
        }

        /// <summary>
        /// Draws multiple text strings at the location with the specified
        /// font, foreground and background colors.
        /// </summary>
        /// <param name="text">Strings to draw.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <param name="foreColor">Default foreground color of the text.</param>
        /// <param name="backColor">Default background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, default background is transparent. </param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="lineDistance">Distance between lines of text. Optional. Default is 0.</param>
        public virtual void DrawStyledTextLines(
            IEnumerable text,
            PointD origin,
            Font font,
            Color foreColor,
            Color backColor,
            Coord lineDistance = 0)
        {
            foreach (var obj in text)
            {
                DrawStyledText(obj, origin, font, foreColor, backColor);
                origin.Y += MeasureStyledText(obj, font).Height + lineDistance;
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
        public virtual void DrawStyledText(object styledText, Font font, Brush brush, PointD origin)
        {
            if (TypeUtils.IsString(styledText))
            {
                var s = styledText.ToString();
                DrawText(s, font, brush, origin);
            }
            else
            {
                var styledObj = styledText as IStyledText;
                styledObj?.Draw(this, origin, font, brush);
            }
        }

        /// <summary>
        /// Measures styled text object.
        /// </summary>
        /// <param name="styledText">Styled text object.</param>
        /// <param name="font">Default <see cref="Font"/> that is used to draw the text.</param>
        /// <returns></returns>
        public virtual SizeD MeasureStyledText(object styledText, Font font)
        {
            if (TypeUtils.IsString(styledText))
            {
                var s = styledText.ToString();
                var measure = MeasureText(s, font);
                return measure;
            }
            else
            {
                var styledObj = styledText as IStyledText;
                var measure = styledObj?.Measure(this, font) ?? SizeD.Empty;
                return measure;
            }
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
        public virtual void DrawStyledText(
            object styledText,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            if (TypeUtils.IsString(styledText))
            {
                var s = styledText.ToString();
                DrawText(s, location, font, foreColor, backColor);
            }
            else
            {
                var styledObj = styledText as IStyledText;
                styledObj?.Draw(this, location, font, foreColor, backColor);
            }
        }

        /// <summary>
        /// Styled text object.
        /// </summary>
        public class StyledText : BaseObject
        {
            /// <summary>
            /// Creates an empty styled text.
            /// </summary>
            /// <returns></returns>
            public static StyledText Create()
            {
                return new SimpleStyledText();
            }

            /// <summary>
            /// Creates styled text without font and color attributes.
            /// </summary>
            /// <param name="s">Text string without line separators.</param>
            /// <returns></returns>
            public static StyledText Create(string s)
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
            public static StyledText Create(string s, FontStyle fontStyle, Brush? brush)
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
            public static StyledText Create(string s, Font? font, Brush? brush)
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
            public static StyledText Create(
                string s,
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
            public static StyledText Create(
                string s,
                FontStyle fontStyle,
                Color? foreColor,
                Color? backColor)
            {
                return new StyledTextWithFontAndColor(s, fontStyle, foreColor, backColor);
            }
        }

        internal class SimpleStyledText : StyledText, IStyledText
        {
            private string text;
            private SizeD? measure;
            private Coord scaleFactor;
            private ObjectUniqueId defaultFont;

            public SimpleStyledText(string text)
            {
                this.text = text;
            }

            public SimpleStyledText()
            {
                text = string.Empty;
            }

            public virtual string Text
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
                    Text,
                    location,
                    SafeFont(font),
                    SafeForeColor(foreColor),
                    SafeBackColor(backColor));
            }

            public virtual void Draw(Graphics dc, PointD location, Font font, Brush foreBrush)
            {
                dc.DrawText(Text, SafeFont(font), SafeForeBrush(foreBrush), location);
            }

            public virtual SizeD Measure(Graphics dc, Font font)
            {
                font = SafeFont(font);
                var fontId = font.UniqueId;
                var newScaleFactor = dc.ScaleFactor;

                if (measure is null || defaultFont != fontId || scaleFactor != newScaleFactor)
                {
                    defaultFont = fontId;
                    measure = dc.MeasureText(Text, font);
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

            public StyledTextWithFontAndColor(string s)
                : base(s)
            {
            }

            public StyledTextWithFontAndColor(string s, FontStyle? fontStyle, Brush? foreBrush)
                : base(s)
            {
                this.fontStyle = fontStyle;
                foregroundBrush = foreBrush;
            }

            public StyledTextWithFontAndColor(
                string s,
                FontStyle? fontStyle,
                Color? foreColor,
                Color? backColor)
                : base(s)
            {
                this.fontStyle = fontStyle;
                foregroundColor = foreColor;
                backgroundColor = backColor;
            }

            public StyledTextWithFontAndColor(string s, Font? font, Brush? foreBrush)
                : base(s)
            {
                this.font = font;
                foregroundBrush = foreBrush;
            }

            public StyledTextWithFontAndColor(string s, Font? font, Color? foreColor, Color? backColor)
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
