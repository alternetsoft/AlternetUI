using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static bool ShowDebugCorners = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTextControl"/> class.
        /// </summary>
        public GenericTextControl()
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

            var conteinerSize = suggested;

            if (isNanSuggestedWidth)
                conteinerSize.Width = availableSize.Width;

            if (isNanSuggestedHeight)
                conteinerSize.Height = availableSize.Height;

            var paddingSize = Padding.Size;

            conteinerSize -= paddingSize;

            var measured = func(conteinerSize);

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
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            if (availableSize.AnyIsEmptyOrNegative)
                return SizeD.Empty;

            var result = GetDefaultPreferredSize(
                        availableSize,
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

        /// <summary>
        /// Measures text size.
        /// </summary>
        /// <param name="dc">Graphics context.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="availableSize">Available size.</param>
        public virtual SizeD MeasureText(Graphics dc, Font font, SizeD availableSize)
        {
            var text = TextForPaint;
            if (string.IsNullOrEmpty(text))
                return SizeD.Empty;
            var result = MeasureCanvas.GetTextExtent(text, font);
            return result;
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
        /// Paints text on the canvas.
        /// </summary>
        /// <param name="dc">Graphics context.</param>
        /// <param name="rect">Bounding rectangle used to draw the text.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        public virtual void DrawText(
            Graphics dc,
            RectD rect,
            Font font,
            Color foreColor,
            Color backColor)
        {
            var labelText = TextForPaint;
            if (labelText == string.Empty)
                return;
            dc.DrawText(labelText, rect.Location, font, foreColor, backColor);
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
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClipRectangle);
        }
    }
}