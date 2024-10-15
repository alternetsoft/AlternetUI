using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements simple text control which can paint text without new line
    /// characters and wrapping.
    /// </summary>
    public class GenericTextControl : GenericControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTextControl"/> class.
        /// </summary>
        public GenericTextControl()
        {
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var specifiedWidth = SuggestedWidth;
            var specifiedHeight = SuggestedHeight;

            SizeD result = 0;

            var text = Text;
            if (!string.IsNullOrEmpty(text))
            {
                result = MeasureCanvas.GetTextExtent(
                    text,
                    GetLabelFont(VisualControlState.Normal),
                    this);
            }

            if (!Coord.IsNaN(specifiedWidth))
                result.Width = Math.Max(result.Width, specifiedWidth);

            if (!Coord.IsNaN(specifiedHeight))
                result.Height = Math.Max(result.Height, specifiedHeight);

            return result + Padding.Size;
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            PerformLayoutAndInvalidate();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var dc = e.Graphics;
            var state = VisualState;
            RectD paddedRect = (
                rect.Location + Padding.LeftTop,
                rect.Size - Padding.Size);

            var labelText = Text;
            if (labelText == string.Empty)
                return;

            var labelFont = GetLabelFont(state);
            var labelForeColor = GetLabelForeColor(state);
            var labelBackColor = GetLabelBackColor(state);

            dc.DoInsideClipped(
                paddedRect,
                () =>
                {
                    dc.DrawText(
                        labelText,
                        paddedRect.Location,
                        labelFont,
                        labelForeColor,
                        labelBackColor);
                },
                IsClipped);
        }
    }
}