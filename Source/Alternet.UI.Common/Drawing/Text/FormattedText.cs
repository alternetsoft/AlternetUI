using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements formatted text container.
    /// </summary>
    public class FormattedText : TextFormat, IDrawableElement
    {
        private string text = string.Empty;
        private Coord? maxWidth;
        private Coord? maxHeight;
        private Font? font;
        private Coord lineDistance;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedText"/> class.
        /// </summary>
        public FormattedText()
        {
        }

        /// <summary>
        /// Gets or sets distance between lines of text.
        /// </summary>
        public virtual Coord Distance
        {
            get
            {
                return lineDistance;
            }

            set
            {
                if (lineDistance == value)
                    return;
                lineDistance = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets font.
        /// </summary>
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

        /// <summary>
        /// Gets or sets maximal width.
        /// </summary>
        public virtual Coord? MaxWidth
        {
            get
            {
                return maxWidth;
            }

            set
            {
                if (maxWidth == value)
                    return;
                maxWidth = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets maximal height.
        /// </summary>
        public virtual Coord? MaxHeight
        {
            get
            {
                return maxHeight;
            }

            set
            {
                if (maxHeight == value)
                    return;
                maxHeight = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
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
                text = value ?? string.Empty;
                Changed();
            }
        }

        /// <inheritdoc/>
        public override TextVerticalAlignment VerticalAlignment
        {
            get
            {
                switch (BlockVerticalAlignment)
                {
                    case UI.VerticalAlignment.Center:
                        return TextVerticalAlignment.Center;
                    case UI.VerticalAlignment.Bottom:
                        return TextVerticalAlignment.Bottom;
                    default:
                        return TextVerticalAlignment.Top;
                }
            }

            set
            {
                switch (value)
                {
                    default:
                        BlockVerticalAlignment = UI.VerticalAlignment.Top;
                        break;
                    case TextVerticalAlignment.Center:
                        BlockVerticalAlignment = UI.VerticalAlignment.Center;
                        break;
                    case TextVerticalAlignment.Bottom:
                        BlockVerticalAlignment = UI.VerticalAlignment.Bottom;
                        break;
                }
            }
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
        /// Gets or sets foreground color. If not specified, black color is used.
        /// </summary>
        public virtual Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets background color. If not specified, background is not filled.
        /// </summary>
        public virtual Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether to set clip rectangle when object is painted.
        /// Default is <c>true</c>.
        /// </summary>
        public virtual bool IsClipped { get; set; } = true;

        /// <summary>
        /// Gets font in a safe way. If <see cref="Font"/> is not specified, returns default font.
        /// </summary>
        public virtual Font SafeFont => Font ?? AbstractControl.DefaultFont;

        /// <inheritdoc/>
        public override void Changed()
        {
            base.Changed();
        }

        /// <summary>
        /// Draws formatted text.
        /// </summary>
        /// <param name="dc">Canvas where drawing is performed.</param>
        /// <param name="bounds">Bounding rectangle.</param>
        public virtual void Draw(Graphics dc, RectD bounds)
        {
            var drawable = CreateElement(bounds.Size);
            drawable.Draw(dc, bounds);
        }

        /// <inheritdoc/>
        public SizeD Measure(Graphics dc, SizeD availableSize)
        {
            var drawable = CreateElement(availableSize);
            var result = drawable.Measure(dc, availableSize);
            return result;
        }

        private DrawableWrappedTextElement CreateElement(SizeD availableSize)
        {
            availableSize.Width = MathUtils.ApplyMinMax(availableSize.Width, 0, MaxWidth);
            availableSize.Height = MathUtils.ApplyMinMax(availableSize.Height, 0, MaxHeight);

            var drawable = new DrawableWrappedTextElement(text);
            drawable.Alignment = (CoordAlignment)AlignUtils.Convert(HorizontalAlignment);
            drawable.Distance = Distance;
            drawable.IsVertical = true;
            drawable.BlockHorizontalAlignment = BlockHorizontalAlignment;
            drawable.BlockVerticalAlignment = BlockVerticalAlignment;
            drawable.Style = new DrawableElementStyle();
            drawable.Style.Font = SafeFont;
            drawable.Style.BackgroundColor = BackgroundColor;
            drawable.Style.ForegroundColor = ForegroundColor;
            return drawable;
        }
    }
}