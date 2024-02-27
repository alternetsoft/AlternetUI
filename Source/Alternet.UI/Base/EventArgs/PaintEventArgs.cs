using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Control.Paint"/> event.
    /// </summary>
    /// <remarks>
    /// The <see cref="Control.Paint"/> event occurs when a control is redrawn.
    /// A <see cref="PaintEventArgs"/> specifies the <see cref="DrawingContext"/> to use
    /// to paint the control and the <see cref="Bounds"/> in which to paint.
    /// </remarks>
    public class PaintEventArgs : BaseEventArgs
    {
        internal PaintEventArgs(Graphics drawingContext, RectD bounds)
        {
            DrawingContext = drawingContext;
            Bounds = bounds;
        }

        /// <summary>
        /// Gets the drawing context used to paint.
        /// </summary>
        /// <value>The <see cref="Graphics"/> object used to paint.
        /// The <see cref="Graphics"/> object provides methods for drawing objects
        /// on the display or other device.</value>
        /// <remarks>
        /// This is the same as <see cref="Graphics"/> property.
        /// </remarks>
        public Graphics DrawingContext { get; internal set; }

        /// <summary>
        /// Gets the rectangle in which to paint.
        /// </summary>
        /// <value>The <see cref="RectD"/> in which to paint.</value>
        /// <remarks>
        /// This is the same as <see cref="ClipRectangle"/> property.
        /// </remarks>
        public RectD Bounds { get; internal set; }

        /// <summary>Gets the rectangle in which to paint.</summary>
        /// <returns>The <see cref="RectD" /> in which to paint.</returns>
        /// <remarks>
        /// This is the same as <see cref="Bounds"/> property.
        /// </remarks>
        public RectD ClipRectangle => Bounds;

        /// <summary>
        /// Gets the graphics used to paint.
        /// </summary>
        /// <returns>
        /// The <see cref="Graphics" /> object used to paint.
        /// The <see cref="Graphics" /> object provides methods for drawing objects
        /// on the display or other device.
        /// </returns>
        /// <remarks>
        /// This is the same as <see cref="DrawingContext"/> property.
        /// </remarks>
        public Graphics Graphics => DrawingContext;
    }
}
