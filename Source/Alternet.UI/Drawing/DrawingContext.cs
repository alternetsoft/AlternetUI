using System;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Describes a drawing surface.
    /// </summary>
    public class DrawingContext : IDisposable
    {
        private Native.DrawingContext dc;
        private bool isDisposed;

        internal DrawingContext(Native.DrawingContext dc)
        {
            this.dc = dc;
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="RectangleF"/> structure.
        /// </summary>
        public void FillRectangle(RectangleF rectangle, Color color)
        {
            dc.FillRectangle(rectangle, color);
        }

        /// <summary>
        /// Draws a rectangle specified by a <see cref="RectangleF"/> structure.
        /// </summary>
        public void DrawRectangle(RectangleF rectangle, Color color)
        {
            dc.DrawRectangle(rectangle, color);
        }

        /// <summary>
        /// Draws the specified text string at the specified location.
        /// </summary>
        public void DrawText(string text, PointF origin, Color color)
        {
            dc.DrawText(text, origin, color);
        }

        /// <summary>
        /// Measures the specified string.
        /// </summary>
        public SizeF MeasureText(string text)
        {
            return dc.MeasureText(text);
        }

        /// <summary>
        /// Pushes the specified <see cref="Transform"/> onto the drawing context.
        /// </summary>
        public void PushTransform(Transform value)
        {
            dc.PushTransform(value.Translation);
        }

        /// <summary>
        /// Pops the last transform operation that was pushed onto the drawing context.
        /// </summary>
        public void Pop()
        {
            dc.Pop();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="DrawingContext"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DrawingContext"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dc.Dispose();
                    dc = null!;
                }

                isDisposed = true;
            }
        }
    }
}