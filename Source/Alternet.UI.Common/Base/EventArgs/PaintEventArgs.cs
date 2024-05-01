using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="Control.Paint"/> or other paint events.
    /// </summary>
    /// <remarks>
    /// A <see cref="PaintEventArgs"/> specifies the <see cref="Graphics"/> to use
    /// to paint the control or object and the <see cref="ClipRectangle"/> in which to paint.
    /// </remarks>
    public class PaintEventArgs : BaseEventArgs
    {
        private Graphics canvas;
        private RectD rect;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvas">The <see cref="Graphics"/> used to paint.</param>
        /// <param name="clipRect">The <see cref="RectD" /> that represents
        /// the rectangle in which to paint.</param>
        public PaintEventArgs(Graphics canvas, RectD clipRect)
        {
            this.canvas = canvas;
            this.rect = clipRect;
        }

        /// <summary>
        /// Gets the drawing context used to paint.
        /// </summary>
        /// <value>The <see cref="Graphics"/> object used to paint.
        /// The <see cref="Graphics"/> object provides methods for drawing objects
        /// on the display or other device.</value>
        /// <remarks>
        /// Please use <see cref="Graphics"/> instead of this property.
        /// </remarks>
        [Obsolete("'DrawingContext' is deprecated, please use 'Graphics' instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Graphics DrawingContext
        {
            get => canvas;
            set => canvas = value;
        }

        /// <summary>
        /// Gets the rectangle in which to paint.
        /// </summary>
        /// <value>The <see cref="RectD"/> in which to paint.</value>
        /// <remarks>
        /// Please use <see cref="ClipRectangle"/> instead of this property.
        /// </remarks>
        [Obsolete("'Bounds' is deprecated, please use 'ClipRectangle' instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public RectD Bounds
        {
            get => rect;
            set => rect = value;
        }

        /// <summary>Gets the rectangle in which to paint.</summary>
        /// <returns>The <see cref="RectD" /> in which to paint.</returns>
        public RectD ClipRectangle
        {
            get => rect;
            set => rect = value;
        }

        /// <summary>
        /// Gets the <see cref="Graphics"/> used to paint.
        /// </summary>
        /// <returns>
        /// The <see cref="Graphics" /> object used to paint.
        /// The <see cref="Graphics" /> object provides methods for drawing objects
        /// on the display or other device.
        /// </returns>
        public Graphics Graphics
        {
            get => canvas;
            set => canvas = value;
        }
    }
}
