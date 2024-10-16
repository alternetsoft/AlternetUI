using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Drawable text element with text wrapping.
    /// </summary>
    public class DrawableWrappedTextElement : DrawableStackElement
    {
        private object? text;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableWrappedTextElement"/> class.
        /// </summary>
        public DrawableWrappedTextElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableWrappedTextElement"/> class
        /// with the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public DrawableWrappedTextElement(object? text)
        {
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableWrappedTextElement"/> class
        /// with the specified text and style.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style of the text.</param>
        public DrawableWrappedTextElement(object? text, DrawableElementStyle? style)
        {
            this.text = text;
            Style = style;
        }

        /// <summary>
        /// Gets of sets horizontal alignment of the whole text block.
        /// </summary>
        public virtual HorizontalAlignment BlockHorizontalAlignment { get; set; }
            = UI.HorizontalAlignment.Stretch;

        /// <summary>
        /// Gets of sets vertical alignment of the whole text block.
        /// </summary>
        public virtual VerticalAlignment BlockVerticalAlignment { get; set; }
            = UI.VerticalAlignment.Stretch;

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
            if (!Prepare(dc, container.Size))
                return;

            var size = Measure(dc, container.Size);
            RectD rect = (container.Location, size);

            var blockRect = AlignUtils.AlignRectInRect(
                rect,
                container,
                BlockHorizontalAlignment,
                BlockVerticalAlignment);

            dc.DoInsideClipped(
                container,
                () =>
                {
                    base.Draw(dc, blockRect);
                },
                IsClipped);
        }

        /// <inheritdoc/>
        public override SizeD Measure(Graphics dc, SizeD availableSize)
        {
            if (!Prepare(dc, availableSize))
                return SizeD.Empty;
            return base.Measure(dc, availableSize);
        }

        private bool Prepare(Graphics dc, SizeD availableSize)
        {
            Items = null;
            var realText = Text?.ToString();
            if (realText is null || realText.Length == 0)
                return false;

            var realStyle = RealStyle;
            Font realFont = realStyle.RealFont;

            var wrappedText = DrawingUtils.WrapTextToList(
                        realText,
                        availableSize.Width,
                        realFont,
                        dc);
            Items = CreateCollection(wrappedText, this);
            return true;
        }
    }
}