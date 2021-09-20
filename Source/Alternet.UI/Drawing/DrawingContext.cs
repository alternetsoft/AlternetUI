using System;
using Alternet.Drawing;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes a drawing surface.
    /// </summary>
    public class DrawingContext : IDisposable
    {
        private UI.Native.DrawingContext dc;
        private bool isDisposed;

        internal DrawingContext(UI.Native.DrawingContext dc)
        {
            this.dc = dc;
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of the fill.</param>
        /// <param name="rectangle"><see cref="RectangleF"/> structure that represents the rectangle to fill.</param>
        /// <remarks>
        /// This method fills the interior of the rectangle defined by the <c>rect</c> parameter,
        /// including the specified upper-left corner and up to the calculated lower and bottom edges.
        /// </remarks>
        public void FillRectangle(Brush brush, RectangleF rectangle)
        {
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

            dc.FillRectangle(rectangle, brush.NativeBrush);
        }

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by a <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of the fill.</param>
        /// <param name="bounds"><see cref="RectangleF"/> structure that represents the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a <see cref="Brush"/>.
        /// The ellipse is defined by the bounding rectangle represented by the <c>bounds</c> parameter.
        /// </remarks>
        public void FillEllipse(Brush brush, RectangleF bounds)
        {
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

            dc.FillEllipse(bounds, brush.NativeBrush);
        }

        /// <summary>
        /// Draws a rectangle specified by a <see cref="RectangleF"/> structure.
        /// </summary>
        /// <param name="pen">A <see cref="Pen"/> that determines the color, width, and style of the rectangle.</param>
        /// <param name="rectangle">A <see cref="RectangleF"/> structure that represents the rectangle to draw.</param>
        public void DrawRectangle(Pen pen, RectangleF rectangle)
        {
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

            dc.DrawRectangle(rectangle, pen.NativePen);
        }

        /// <summary>
        /// Draws an ellipse defined by a bounding <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style of the ellipse.</param>
        /// <param name="bounds"><see cref="RectangleF"/> structure that defines the boundaries of the ellipse.</param>
        public void DrawEllipse(Pen pen, RectangleF bounds)
        {
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

            dc.DrawEllipse(bounds, pen.NativePen);
        }

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="PointF"/> structure that represents the upper-left corner of the drawn image.</param>
        public void DrawImage(Image image, PointF origin)
        {
            if (image is null)
                throw new ArgumentNullException(nameof(image));

            dc.DrawImage(image.NativeImage, origin);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of the drawn text.</param>
        /// <param name="origin"><see cref="PointF"/> structure that specifies the upper-left corner of the drawn text.</param>
        public void DrawText(string text, Font font, Brush brush, PointF origin)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));

            dc.DrawText(text, origin, font.NativeFont, brush.NativeBrush);
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