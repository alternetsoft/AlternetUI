using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>Provides data for the <see langword="DrawItem" /> event.</summary>
    public class DrawItemEventArgs : BaseEventArgs
    {
        private readonly Graphics graphics;
        private readonly int index;
        private readonly RectD rect;
        private readonly DrawItemState state;
        private readonly Color backColor;
        private readonly Color foreColor;
        private readonly Font font;

        /// <summary>Initializes a new instance of the <see cref="DrawItemEventArgs" /> class
        /// for the specified control with the specified font, state, surface to draw on,
        /// and the bounds to draw within.</summary>
        /// <param name="graphics">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="font">The <see cref="Font" /> to use, usually
        /// the parent control's <see cref="Font" /> property.</param>
        /// <param name="rect">The <see cref="RectD" /> bounds to draw within.</param>
        /// <param name="index">The <see cref="Control.ControlCollection" /> index value
        /// of the item that is being drawn.</param>
        /// <param name="state">The control's <see cref="T:DrawItemState" /> information.</param>
        public DrawItemEventArgs(
            Graphics graphics,
            Font font,
            RectD rect,
            int index,
            DrawItemState state)
        {
            this.graphics = graphics;
            this.font = font;
            this.rect = rect;
            this.index = index;
            this.state = state;
            foreColor = SystemColors.WindowText;
            backColor = SystemColors.Window;
        }

        /// <summary>Initializes a new instance of the <see cref="DrawItemEventArgs" />
        /// class for the specified control with the specified font, state,
        /// foreground color, background color, surface to draw on, and
        /// the bounds to draw within.</summary>
        /// <param name="graphics">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="font">The <see cref="Font" /> to use, usually the parent
        /// control's <see cref="Font" /> property.</param>
        /// <param name="rect">The <see cref="RectD" /> bounds to draw within.</param>
        /// <param name="index">The <see cref="Control.ControlCollection" /> index
        /// value of the item that is being drawn.</param>
        /// <param name="state">The control's <see cref="DrawItemState" /> information.</param>
        /// <param name="foreColor">The foreground <see cref="Color" /> to
        /// draw the control with.</param>
        /// <param name="backColor">The background <see cref="Color" /> to
        /// draw the control with.</param>
        public DrawItemEventArgs(
            Graphics graphics,
            Font font,
            RectD rect,
            int index,
            DrawItemState state,
            Color foreColor,
            Color backColor)
        {
            this.graphics = graphics;
            this.font = font;
            this.rect = rect;
            this.index = index;
            this.state = state;
            this.foreColor = foreColor;
            this.backColor = backColor;
        }

        /// <summary>Gets the background color of the item that is being drawn.</summary>
        /// <returns>The background <see cref="Color" /> of the item that is being drawn.</returns>
        public Color BackColor
        {
            get
            {
                if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    return SystemColors.Highlight;
                }

                return backColor;
            }
        }

        /// <summary>
        /// Gets the rectangle that represents the bounds of the item that is being drawn.
        /// </summary>
        /// <returns>The <see cref="RectD" /> that represents the bounds of the item that
        /// is being drawn.</returns>
        public RectD Bounds => rect;

        /// <summary>Gets the font that is assigned to the item being drawn.</summary>
        /// <returns>The <see cref="Font" /> that is assigned to the item being drawn.</returns>
        public Font Font => font;

        /// <summary>Gets the foreground color of the of the item being drawn.</summary>
        /// <returns>The foreground <see cref="Color" /> of the item being drawn.</returns>
        public Color ForeColor
        {
            get
            {
                if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    return SystemColors.HighlightText;
                }

                return foreColor;
            }
        }

        /// <summary>Gets the graphics surface to draw the item on.</summary>
        /// <returns>The <see cref="Graphics" /> surface to draw the item on.</returns>
        public Graphics Graphics => graphics;

        /// <summary>Gets the index value of the item that is being drawn.</summary>
        public int Index => index;

        /// <summary>Gets the state of the item being drawn.</summary>
        /// <returns>The <see cref="DrawItemState" /> that represents the state of the
        /// item being drawn.</returns>
        public DrawItemState State => state;

        /// <summary>Draws the background within the bounds specified
        /// in the constructor and with the appropriate color.</summary>
        public virtual void DrawBackground()
        {
            Brush brush = new SolidBrush(BackColor);
            Graphics.FillRectangle(brush, rect);
            brush.Dispose();
        }
    }
}
