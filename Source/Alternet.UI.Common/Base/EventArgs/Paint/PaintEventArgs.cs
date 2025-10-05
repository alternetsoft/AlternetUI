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
        private RectD clipRectangle;
        private RectD clientRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvas">The <see cref="Graphics"/> object representing the drawing surface.</param>
        /// <param name="rect">The <see cref="RectD"/> structure
        /// that defines both the clipping and client area for the paint operation.</param>
        public PaintEventArgs(Graphics canvas, RectD rect)
        {
            this.canvas = canvas;
            canvasFunc = () => canvas;
            this.clipRectangle = rect;
            this.clientRectangle = rect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvasFunc">The function which returns
        /// <see cref="Graphics"/> used to paint.</param>
        /// <param name="rect">The <see cref="RectD"/> structure
        /// that defines both the clipping and client area for the paint operation.</param>
        public PaintEventArgs(Func<Graphics> canvasFunc, RectD rect)
        {
            this.canvasFunc = canvasFunc;
            this.clipRectangle = rect;
            this.clientRectangle = rect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> used to paint.</param>
        /// <param name="clipRect">The <see cref="RectD" /> that represents
        /// the invalidated rectangle. See <see cref="ClipRectangle"/> property description.</param>
        /// <param name="clientRect">The <see cref="RectD" /> that represents
        /// the client area of the object.</param>
        public PaintEventArgs(Graphics canvas, RectD clipRect, RectD clientRect)
        {
            this.canvas = canvas;
            canvasFunc = () => canvas;
            this.clipRectangle = clipRect;
            this.clientRectangle = clientRect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="canvasFunc">The function which returns
        /// <see cref="Graphics"/> used to paint.</param>
        /// <param name="clipRect">The <see cref="RectD" /> that represents
        /// the invalidated rectangle. See <see cref="ClipRectangle"/> property description.</param>
        /// <param name="clientRect">The <see cref="RectD" /> that represents
        /// the client area of the object.</param>
        public PaintEventArgs(Func<Graphics> canvasFunc, RectD clipRect, RectD clientRect)
        {
            this.canvasFunc = canvasFunc;
            this.clipRectangle = clipRect;
            this.clientRectangle = clientRect;
        }

        /// <summary>
        /// Gets whether canvas was allocated.
        /// </summary>
        public bool GraphicsAllocated => canvas is not null;

        /// <summary>
        /// Gets or sets the client area of the object, represented as a rectangle.
        /// </summary>
        public RectD ClientRectangle
        {
            get => clientRectangle;
            set => clientRectangle = value;
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
        /// Gets the rectangle that describes the update/invalidated region
        /// that should be repainted. What it means:
        /// <list type="bullet">
        ///   <item><description>
        ///   It is in control client coordinates (origin = top-left of the client area).
        ///   </description></item>
        ///   <item><description>
        ///   It is the bounding rectangle of the update region (the union of invalidated rectangles).
        ///   The OS may give you the minimal rectangle that covers the dirty region.
        ///   </description></item>
        ///   <item><description>
        ///   It is expressed in dips (i.e., logical coordinates for the control surface).
        ///   </description></item>
        /// </list>
        /// How it is produced:
        /// <list type="bullet">
        ///   <item><description>
        ///   By calls such as Invalidate(), Invalidate(Rectangle), Invalidate(Rectangle, bool)
        ///   or by the OS when exposed regions change.
        ///   </description></item>
        ///   <item><description>
        ///   If you call Refresh() or Update() you get a paint for the invalidated area
        ///   (which may be the whole client if you invalidated the whole control).
        ///   </description></item>
        /// </list>
        /// </summary>
        /// <returns>The <see cref="RectD" /> in which to paint.</returns>
        public RectD ClipRectangle
        {
            get
            {
                return clipRectangle;
            }

            set
            {
                clipRectangle = value;
            }
        }

        /// <summary>
        /// Returns a new instance of <c>PaintEventArgs</c> with the same canvas and
        /// the specified rectangles. If the specified rectangles match the current
        /// rectangles, the current instance is returned.
        /// </summary>
        /// <param name="clipRect">The <see cref="RectD" /> that represents
        /// the invalidated rectangle. See <see cref="ClipRectangle"/> property description.</param>
        /// <param name="clientRect"></param>
        /// <returns>A new instance of <c>PaintEventArgs</c> with the same canvas
        /// and the given rectangle.</returns>
        public virtual PaintEventArgs WithRects(RectD clipRect, RectD clientRect)
        {
            if (clipRect == ClipRectangle && clientRect == ClientRectangle)
                return this;
            return new PaintEventArgs(() => Graphics, clipRect, clientRect);
        }

        /// <summary>
        /// Returns a new <see cref="PaintEventArgs"/> instance with the specified
        /// clipping and client rectangles. If the specified rectangle matches both
        /// <see cref="ClipRectangle"/> and <see cref="ClientRectangle"/>, the current
        /// instance is returned.
        /// </summary>
        /// <param name="rect">The rectangle to use as both the clipping
        /// and client rectangles for the new <see cref="PaintEventArgs"/>
        /// instance.</param>
        /// <returns>A new <see cref="PaintEventArgs"/> instance
        /// with the specified rectangle as both the clipping and client
        /// rectangles, or the current instance if the
        /// specified rectangle matches both the <see cref="ClipRectangle"/>
        /// and <see cref="ClientRectangle"/>.</returns>
        public virtual PaintEventArgs WithRect(RectD rect)
        {
            if (rect == ClipRectangle && rect == ClientRectangle)
                return this;
            return new PaintEventArgs(() => Graphics, rect, rect);
        }
    }
}
