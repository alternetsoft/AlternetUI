using System;
using System.Drawing;

namespace Alternet.UI
{
    public class DrawingContext : IDisposable
    {
        private Native.DrawingContext dc;
        private bool isDisposed;

        internal DrawingContext(Native.DrawingContext dc)
        {
            this.dc = dc;
        }

        public void FillRectangle(RectangleF rectangle, Color color)
        {
            dc.FillRectangle(rectangle, color);
        }

        public void DrawRectangle(RectangleF rectangle, Color color)
        {
            dc.DrawRectangle(rectangle, color);
        }

        public void DrawText(string text, PointF origin, Color color)
        {
            dc.DrawText(text, origin, color);
        }

        public SizeF MeasureText(string text)
        {
            return dc.MeasureText(text);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

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