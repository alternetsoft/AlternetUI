using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Drawable text element.
    /// </summary>
    public class DrawableTextElement : DrawableElement, IDrawableElement
    {
        private DrawableElementStyle? style;
        private object? text;

        private SizeD? measuredSize;
        private Coord measuredScaleFactor;
        private ObjectUniqueId measuredFontId;
        private string? measuredText;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableTextElement"/> class.
        /// </summary>
        public DrawableTextElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableTextElement"/> class
        /// with the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public DrawableTextElement(object? text)
        {
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableTextElement"/> class
        /// with the specified text and style.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style of the text.</param>
        public DrawableTextElement(object? text, DrawableElementStyle? style)
        {
            this.text = text;
            this.style = style;
        }

        /// <summary>
        /// Gets real attributes of this object.
        /// </summary>
        [Browsable(false)]
        public virtual DrawableElementStyle RealStyle
        {
            get
            {
                return Style ?? DrawableElementStyle.Default;
            }
        }

        /// <summary>
        /// Gets or sets attributes of the styled text.
        /// </summary>
        public virtual DrawableElementStyle? Style
        {
            get
            {
                return style;
            }

            set
            {
                SetProperty(ref style, value, nameof(Style));
            }
        }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        public virtual object? Text
        {
            get
            {
                return text;
            }

            set
            {
                SetProperty(ref text, value, nameof(Text));
            }
        }

        /// <inheritdoc/>
        public override void Draw(Graphics dc, PointD location)
        {
            var realText = Text?.ToString();
            if (realText is null || realText.Length == 0)
                return;

            var realStyle = RealStyle;
            var realForeColor = realStyle.RealForeColor;
            Font realFont = realStyle.RealFont;

            if (realForeColor is null)
            {
                var realForeBrush = realStyle.RealForeBrush
                    ?? DrawableElementStyle.DefaultForegroundColor.AsBrush;
                dc.DrawText(realText, realFont, realForeBrush, location);
            }
            else
            {
                var realBackColor = realStyle.RealBackColor;
                dc.DrawText(realText, location, realFont, realForeColor, realBackColor ?? Color.Empty);
            }
        }

        /// <inheritdoc/>
        public override SizeD Measure(Graphics dc)
        {
            var realText = Text?.ToString();
            if (realText is null || realText.Length == 0)
                return SizeD.Empty;

            var realStyle = RealStyle;
            var realFont = realStyle.RealFont;
            var realFontId = realFont.UniqueId;
            var newScaleFactor = dc.ScaleFactor;

            var valid =
                measuredSize is not null
                && measuredScaleFactor == newScaleFactor
                && measuredFontId == realFontId
                && measuredText == realText;

            if (valid)
                return measuredSize!.Value;

            measuredFontId = realFontId;
            measuredScaleFactor = newScaleFactor;
            measuredSize = dc.MeasureText(realText, realFont);
            measuredText = realText;
            return measuredSize.Value;
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            measuredSize = null;
        }
    }
}
