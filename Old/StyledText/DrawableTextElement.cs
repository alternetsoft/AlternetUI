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
        private object? text;

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
            : base(style)
        {
            this.text = text;
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
        public override void Draw(Graphics dc, RectD container)
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
                dc.DrawText(realText, realFont, realForeBrush, container.Location);
            }
            else
            {
                var realBackColor = realStyle.RealBackColor;
                dc.DrawText(
                    realText,
                    container.Location,
                    realFont,
                    realForeColor,
                    realBackColor ?? Color.Empty);
            }
        }

        /// <inheritdoc/>
        public override SizeD Measure(Graphics dc, SizeD availableSize)
        {
            var realText = Text?.ToString();
            if (realText is null || realText.Length == 0)
                return SizeD.Empty;

            var realStyle = RealStyle;
            var realFont = realStyle.RealFont;

            var measuredSize = dc.MeasureText(realText, realFont);
            return measuredSize;
        }
    }
}
