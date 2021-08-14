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
        /// Draws an image at the specified location.
        /// </summary>
        public void DrawImage(Image image, PointF origin)
        {
            dc.DrawImage(image.NativeImage, origin);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointF"/> structure that specifies the upper-left corner of the drawn text.</param>
        public void DrawText(string text, Font font, Color brush, PointF origin)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));

            dc.DrawText(text, origin, font.NativeFont, brush);
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <returns>
        /// This method returns a <see cref="SizeF"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        public SizeF MeasureText(string text, Font font)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));

            return dc.MeasureText(text, font.NativeFont);
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