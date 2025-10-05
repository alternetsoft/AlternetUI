using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements simple text control which can draw text with new line
    /// characters and wrapping.
    /// </summary>
    [ControlCategory("Tests")]
    public partial class GenericWrappedTextControl : GenericControl
    {
        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        private TextFormat.Record textFormat;
        private IEnumerable<string>? wrappedText;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWrappedTextControl"/> class
        /// with the specified parent.
        /// </summary>
        public GenericWrappedTextControl(AbstractControl? parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWrappedTextControl"/> class.
        /// </summary>
        public GenericWrappedTextControl()
        {
        }

        /// <summary>
        /// Gets text for painting.
        /// </summary>
        [Browsable(false)]
        public virtual string TextForPaint
        {
            get
            {
                return Text;
            }
        }

        /// <summary>
        /// Gets or sets distance between lines of text.
        /// </summary>
        public virtual Coord LineDistance
        {
            get
            {
                return textFormat.Distance;
            }

            set
            {
                if (textFormat.Distance == value)
                    return;
                textFormat.Distance = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.VerticalAlignment"/>.
        public virtual TextVerticalAlignment TextVerticalAlignment
        {
            get => textFormat.VerticalAlignment;
            set
            {
                if (textFormat.VerticalAlignment == value)
                    return;
                textFormat.VerticalAlignment = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.Trimming"/>.
        public virtual TextTrimming TextTrimming
        {
            get => textFormat.Trimming;
            set
            {
                if (textFormat.Trimming == value)
                    return;
                textFormat.Trimming = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.Wrapping"/>.
        public virtual TextWrapping TextWrapping
        {
            get => textFormat.Wrapping;
            set
            {
                if (textFormat.Wrapping == value)
                    return;
                textFormat.Wrapping = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets text alignment.
        /// </summary>
        public virtual TextHorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return textFormat.HorizontalAlignment;
            }

            set
            {
                if (textFormat.HorizontalAlignment == value)
                    return;
                textFormat.HorizontalAlignment = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets text formatting record.
        /// </summary>
        /// <returns></returns>
        public virtual TextFormat.Record GetFormat()
        {
            return textFormat;
        }

        /// <summary>
        /// Sets text formatting record.
        /// </summary>
        public virtual void SetFormat(TextFormat.Record value)
        {
            textFormat = value;
            PerformLayoutAndInvalidate(() =>
            {
                MaxHeight = value.MaxHeight;
                MaxWidth = value.MaxWidth;
                SetSuggestedSize(value.SuggestedWidth, value.SuggestedHeight);
                Padding = value.Padding;
            });
        }

        /// <summary>
        /// Draws text within a specified rectangle on the provided graphics context.
        /// </summary>
        /// <remarks>The text is aligned within the specified rectangle according to the current text
        /// alignment settings.</remarks>
        /// <param name="dc">The graphics context used for drawing the text.</param>
        /// <param name="rect">The rectangle that defines the area where the text will be drawn.</param>
        /// <param name="font">The font used to render the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color behind the text.</param>
        public virtual void DrawText(
            Graphics dc,
            RectD rect,
            Font font,
            Color foreColor,
            Color backColor)
        {
            var size = DrawInternal(dc, rect, font);
            var textRect = rect.WithHeight(size.Height);

            var vertAlignment = AlignUtils.Convert(TextVerticalAlignment);
            var horzAlignment = AlignUtils.Convert(TextHorizontalAlignment);

            var alignedItemRect = AlignUtils.AlignRectInRect(
                textRect,
                rect,
                horzAlignment,
                vertAlignment);

            DrawInternal(dc, alignedItemRect, font, foreColor, backColor);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var dc = e.Graphics;
            var state = VisualState;
            RectD paddedRect = (
                rect.Location + Padding.LeftTop,
                rect.Size - Padding.Size);

            var labelFont = GetLabelFont(state);
            var labelForeColor = GetLabelForeColor(state);
            var labelBackColor = GetLabelBackColor(state);

            DrawText(
                dc,
                paddedRect,
                labelFont,
                labelForeColor,
                labelBackColor);

            DefaultPaintDebug(e);
        }

        /// <summary>
        /// Measures text size.
        /// </summary>
        /// <param name="dc">Graphics context.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="availableSize">Available size.</param>
        public virtual SizeD MeasureText(Graphics dc, Font font, SizeD availableSize)
        {
            var wrappedWidth = availableSize.Width;

            wrappedText = DrawingUtils.WrapTextToList(
                    TextForPaint,
                    wrappedWidth,
                    font,
                    dc);

            var result = DrawInternal(dc, (PointD.Empty, availableSize), font);
            return result;
        }

        /// <summary>
        /// Default method for calculating preferred size.
        /// </summary>
        public virtual SizeD GetDefaultPreferredSize(
            SizeD availableSize,
            bool withPadding,
            Func<SizeD, SizeD> func)
        {
            var suggested = SuggestedSize;

            var isNanSuggestedWidth = suggested.IsNanWidth;
            var isNanSuggestedHeight = suggested.IsNanHeight;

            var containerSize = suggested;

            if (isNanSuggestedWidth)
                containerSize.Width = availableSize.Width;

            if (isNanSuggestedHeight)
                containerSize.Height = availableSize.Height;

            var paddingSize = Padding.Size;

            containerSize -= paddingSize;

            var measured = func(containerSize);

            if (!isNanSuggestedWidth)
                measured.Width = suggested.Width;
            else
                measured.Width += paddingSize.Width;

            if (!isNanSuggestedHeight)
                measured.Height = suggested.Height;
            else
                measured.Height += paddingSize.Height;

            return measured;
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            if (context.AvailableSize.AnyIsEmptyOrNegative)
                return SizeD.Empty;

            var result = GetDefaultPreferredSize(
                        context.AvailableSize,
                        withPadding: true,
                        (size) =>
                        {
                            var measured = MeasureText(
                                MeasureCanvas,
                                GetLabelFont(VisualControlState.Normal),
                                size);
                            return measured;
                        });
            return result;
        }

        /// <inheritdoc/>
        protected override void OnVisualStateChanged(EventArgs e)
        {
            base.OnVisualStateChanged(e);
            UserControl.HandleOnVisualStateChanged(this, RefreshOptions);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            PerformLayoutAndInvalidate();
        }

        [Conditional("DEBUG")]
        private void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClientRectangle);
        }

        private SizeD DrawInternal(
            Graphics dc,
            RectD rect,
            Font font,
            Color? foreColor = null,
            Color? backColor = null)
        {
            return dc.DrawStrings(
                        rect,
                        font,
                        wrappedText,
                        TextHorizontalAlignment,
                        LineDistance,
                        foreColor,
                        backColor);
        }
    }
}
