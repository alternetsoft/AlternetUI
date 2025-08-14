using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control overlay that displays text at a specified location
    /// with customizable font and colors.
    /// </summary>
    public class ControlOverlayWithText : ControlOverlay
    {
        /// <summary>
        /// Gets or sets the text to display in the overlay.
        /// </summary>
        public virtual string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location where the text is drawn.
        /// </summary>
        public virtual PointD Location { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// If <c>null</c>, the control's foreground color is used.
        /// </summary>
        public virtual Color? TextColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the text.
        /// If <c>null</c>, the background is transparent.
        /// </summary>
        public virtual Color? BackColor { get; set; }

        /// <summary>
        /// Gets or sets the font used to draw the text.
        /// If <c>null</c>, the control's font is used.
        /// </summary>
        public virtual Font? Font { get; set; }

        /// <summary>
        /// Gets the font to use for drawing the text.
        /// </summary>
        /// <param name="control">The control to use as a fallback for the font.</param>
        /// <returns>The font to use for drawing the text.</returns>
        public Font GetFont(AbstractControl control)
        {
            return Font ?? control.RealFont;
        }

        /// <summary>
        /// Gets the color to use for drawing the text.
        /// </summary>
        /// <param name="control">The control to use as a fallback for the text color.</param>
        /// <returns>The color to use for drawing the text.</returns>
        public virtual Color GetTextColor(AbstractControl control)
        {
            if (TextColor is not null)
                return TextColor;
            return control.RealForegroundColor;
        }

        /// <summary>
        /// Gets the background color to use for drawing the text.
        /// </summary>
        /// <param name="control">The control to use as a fallback for the background color.</param>
        /// <returns>The background color to use for drawing the text.</returns>
        public virtual Color GetBackColor(AbstractControl control)
        {
            if (BackColor is not null)
                return BackColor;
            return Color.Transparent;
        }

        /// <inheritdoc/>
        public override void OnPaint(AbstractControl control, PaintEventArgs e)
        {
            e.Graphics.DrawText(
                Text,
                Location,
                GetFont(control),
                GetTextColor(control),
                GetBackColor(control));
        }
    }
}
