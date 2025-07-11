using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="AbstractControl.Paint"/> or other paint events.
    /// </summary>
    /// <remarks>
    /// A <see cref="PaintEventArgs"/> specifies the <see cref="Graphics"/> to use
    /// to paint the control or object and the <see cref="ClipRectangle"/> in which to paint.
    /// </remarks>
    public class PaintEventArgs : BaseEventArgs
    {
        private readonly Func<Graphics> canvasFunc;
        private Graphics? canvas;
        private RectD rect;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> used to paint.</param>
        /// <param name="clipRect">The <see cref="RectD" /> that represents
        /// the rectangle in which to paint.</param>
        public PaintEventArgs(Graphics canvas, RectD clipRect)
        {
            this.canvas = canvas;
            canvasFunc = () => canvas;
            this.rect = clipRect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvasFunc">The function which returns
        /// <see cref="Graphics"/> used to paint.</param>
        /// <param name="clipRect">The <see cref="RectD" /> that represents
        /// the rectangle in which to paint.</param>
        public PaintEventArgs(Func<Graphics> canvasFunc, RectD clipRect)
        {
            this.canvasFunc = canvasFunc;
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
            get => Graphics;
            set => Graphics = value;
        }

        /// <summary>
        /// Gets whether canvas was allocated.
        /// </summary>
        public bool GraphicsAllocated => canvas is not null;

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
            get => ClipRectangle;
            set => ClipRectangle = value;
        }

        /// <summary>Gets the rectangle in which to paint.</summary>
        /// <returns>The <see cref="RectD" /> in which to paint.</returns>
        public virtual RectD ClipRectangle
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
        public virtual Graphics Graphics
        {
            get => canvas ??= canvasFunc();
            set => canvas = value;
        }

        /// <summary>
        /// Creates a new instance of PaintEventArgs with the same canvas and
        /// the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to use for painting.</param>
        /// <returns>A new instance of PaintEventArgs with the same canvas
        /// and the given rectangle.</returns>
        public virtual PaintEventArgs WithRect(RectD rect)
        {
            if (rect == ClipRectangle)
                return this;
            return new PaintEventArgs(() => Graphics, rect);
        }
    }
}
