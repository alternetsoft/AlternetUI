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
    public class PaintEventArgs : EventArgs
    {
        internal PaintEventArgs(DrawingContext drawingContext, RectangleF bounds)
        {
            DrawingContext = drawingContext;
            Bounds = bounds;
        }

        /// <summary>
        /// Gets the drawing context used to paint.
        /// </summary>
        /// <value>The <see cref="DrawingContext"/> object used to paint.
        /// The <see cref="DrawingContext"/> object provides methods for drawing objects on the display device.</value>
        public DrawingContext DrawingContext { get; }

        /// <summary>
        /// Gets the rectangle in which to paint.
        /// </summary>
        /// <value>The <see cref="RectangleF"/> in which to paint.</value>
        public RectangleF Bounds { get; }
    }
}