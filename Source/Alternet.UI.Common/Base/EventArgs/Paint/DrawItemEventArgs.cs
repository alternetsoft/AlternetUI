using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the draw item events.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DrawItemEventArgs" /> that contains the event data.</param>
    public delegate void DrawItemEventHandler(object? sender, DrawItemEventArgs e);

    /// <summary>
    /// Provides data for the draw item events.
    /// </summary>
    public class DrawItemEventArgs : BaseEventArgs
    {
        private Graphics graphics;
        private int index;
        private RectD rect;
        private DrawItemState state;
        private Color backColor;
        private Color foreColor;
        private Font font;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawItemEventArgs" /> class
        /// for the specified control with the specified font, state, surface to draw on,
        /// and the bounds to draw within.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="font">The <see cref="Font" /> to use, usually
        /// the parent control's <see cref="Font" /> property.</param>
        /// <param name="rect">The <see cref="RectD" /> bounds to draw within.</param>
        /// <param name="index">The index value of the item that is being drawn.</param>
        /// <param name="state">The control's <see cref="DrawItemState" /> information.</param>
#pragma warning disable
        public DrawItemEventArgs(
            Graphics graphics,
            Font? font = null,
            RectD? rect = null,
            int index = 0,
            DrawItemState state = 0)
#pragma warning restore
        {
            Assign(graphics, font, rect, index, state);
        }

        /// <summary>Initializes a new instance of the <see cref="DrawItemEventArgs" />
        /// class for the specified control with the specified font, state,
        /// foreground color, background color, surface to draw on, and
        /// the bounds to draw within.</summary>
        /// <param name="graphics">The <see cref="Graphics" /> surface on which to draw.</param>
        /// <param name="font">The <see cref="Font" /> to use, usually the parent
        /// control's <see cref="Font" /> property.</param>
        /// <param name="rect">The <see cref="RectD" /> bounds to draw within.</param>
        /// <param name="index">The index value of the item that is being drawn.</param>
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
        public virtual Color BackColor
        {
            get
            {
                if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    return SystemColors.Highlight;
                }

                return backColor;
            }

            set
            {
                backColor = value;
            }
        }

        /// <summary>
        /// Gets the rectangle that represents the bounds of the item that is being drawn.
        /// </summary>
        /// <returns>The <see cref="RectD" /> that represents the bounds of the item that
        /// is being drawn.</returns>
        public virtual RectD Bounds
        {
            get
            {
                return rect;
            }

            set
            {
                rect = value;
            }
        }

        /// <summary>Gets the font that is assigned to the item being drawn.</summary>
        /// <returns>The <see cref="Font" /> that is assigned to the item being drawn.</returns>
        public virtual Font Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
            }
        }

        /// <summary>Gets the foreground color of the of the item being drawn.</summary>
        /// <returns>The foreground <see cref="Color" /> of the item being drawn.</returns>
        public virtual Color ForeColor
        {
            get
            {
                if ((state & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    return SystemColors.HighlightText;
                }

                return foreColor;
            }

            set
            {
                foreColor = value;
            }
        }

        /// <summary>Gets the graphics surface to draw the item on.</summary>
        /// <returns>The <see cref="Graphics" /> surface to draw the item on.</returns>
        public virtual Graphics Graphics
        {
            get
            {
                return graphics;
            }

            set
            {
                graphics = value;
            }
        }

        /// <summary>Gets the index value of the item that is being drawn.</summary>
        public virtual int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        /// <summary>Gets the state of the item being drawn.</summary>
        /// <returns>The <see cref="DrawItemState" /> that represents the state of the
        /// item being drawn.</returns>
        public virtual DrawItemState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        /// <summary>Draws the background within the bounds specified
        /// in the constructor and with the appropriate color.</summary>
        public virtual void DrawBackground()
        {
            Brush brush = new SolidBrush(BackColor);
            Graphics.FillRectangle(brush, rect);
            brush.Dispose();
        }

        /// <summary>
        /// Assigns the drawing context with the specified graphics, font, drawing bounds, item index, and item state.
        /// </summary>
        /// <param name="graphics">The graphics surface on which drawing operations will be performed. Cannot be null.</param>
        /// <param name="font">The font to use for text rendering. If null, the default control font is used.</param>
        /// <param name="rect">The bounding rectangle that defines the area for drawing. If null, an empty rectangle is used.</param>
        /// <param name="index">The zero-based index of the item to be drawn.</param>
        /// <param name="state">The state flags that describe the visual state of the item being drawn.</param>
        public void Assign(Graphics graphics,
            Font? font = null,
            RectD? rect = null,
            int index = 0,
            DrawItemState state = 0)
        {
            this.graphics = graphics;
            this.font = font ?? Control.DefaultFont;
            this.rect = rect ?? RectD.Empty;
            this.index = index;
            this.state = state;
            foreColor = SystemColors.WindowText;
            backColor = SystemColors.Window;
        }
    }
}
