using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements formatted text container.
    /// </summary>
    public class FormattedText : TextFormat
    {
        private string text = string.Empty;
        private Coord? maxWidth;
        private Coord? maxHeight;
        private Font? font;
        private IEnumerable<Graphics.StyledText>? styledText;
        private List<string>? wrappedText;
        private Coord? scaleFactor;
        private SizeD? textSize;
        private Coord lineDistance;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedText"/> class.
        /// </summary>
        public FormattedText()
        {
        }

        /// <summary>
        /// Gets or sets scale factor.
        /// </summary>
        public virtual Coord? ScaleFactor
        {
            get
            {
                return scaleFactor;
            }

            set
            {
                if (scaleFactor == value)
                    return;
                scaleFactor = value;
                Changed();
            }
        }

        /// <summary>
        /// Gets or sets distance between lines of text.
        /// </summary>
        public virtual Coord LineDistance
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

        /// <summary>
        /// Gets wrapped text. Each line of the wrapped text has width less than <see cref="MaxWidth"/>
        /// and doesn't have new line characters.
        /// </summary>
        public virtual List<string>? WrappedText
        {
            get
            {
                Prepare();
                return wrappedText;
            }
        }

        /// <summary>
        /// Gets styled text items created from <see cref="WrappedText"/>.
        /// </summary>
        public virtual IEnumerable<Graphics.StyledText>? StyledText
        {
            get
            {
                Prepare();
                return styledText;
            }
        }

        /// <summary>
        /// Gets size of the text with applied min and max constraints in device-independent units.
        /// </summary>
        public virtual SizeD RestrictedSize
        {
            get
            {
                var result = RealSize.Shrink(MaxWidth, MaxHeight);
                return result;
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
        /// Gets real size of the text in device-independent units.
        /// <see cref="MaxWidth"/> and <see cref="MaxHeight"/> are not applied to the result.
        /// </summary>
        public virtual SizeD RealSize
        {
            get
            {
                Prepare();
                return textSize ?? 0;
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
        public virtual Font SafeFont => Font ?? Control.DefaultFont;

        /// <inheritdoc/>
        public override void Changed()
        {
            base.Changed();
            styledText = null;
            textSize = null;
            wrappedText = null;
        }

        /// <summary>
        /// Gets aligned bounding recrangle of the text.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <param name="container">Container rectangle.</param>
        /// <returns></returns>
        public virtual RectD GetBlockRect(Coord scaleFactor, RectD container)
        {
            ScaleFactor = ScaleFactor;
            Prepare();

            if (styledText is null)
                return container;

            RectD blockRect = (container.Location, RestrictedSize);
            blockRect = AlignUtils.AlignRectInRect(
                blockRect,
                container,
                BlockHorizontalAlignment,
                BlockVerticalAlignment);

            return blockRect;
        }

        /// <summary>
        /// Draws formatted text.
        /// </summary>
        /// <param name="dc">Canvas where drawing is performed.</param>
        /// <param name="bounds">Bounding rectangle.</param>
        public virtual void Draw(Graphics dc, RectD bounds)
        {
            ScaleFactor = dc.ScaleFactor;
            Prepare();

            if (styledText is null)
                return;

            var blockRect = GetBlockRect(dc.ScaleFactor, bounds);
            var halign = AlignUtils.Convert(HorizontalAlignment);

            dc.DoInsideClipped(
                bounds,
                () =>
                {
                    dc.DrawStyledText(
                        styledText,
                        blockRect.Location,
                        SafeFont,
                        ForegroundColor ?? Color.Black,
                        BackgroundColor ?? Color.Empty,
                        halign,
                        blockRect.Width,
                        LineDistance);
                },
                IsClipped);
        }

        /// <summary>
        /// Prepares object for painting. You should not call this normally.
        /// </summary>
        public virtual void Prepare()
        {
            if (styledText is not null)
                return;

            wrappedText = DrawingUtils.WrapTextToList(
                        text,
                        MaxWidth,
                        SafeFont,
                        ScaleFactor);

            styledText = Graphics.StyledText.CreateCollection(wrappedText);

            textSize = DrawingUtils.MeasureText(wrappedText, SafeFont, ScaleFactor, LineDistance);
        }
    }
}