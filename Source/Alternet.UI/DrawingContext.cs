using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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

        public void PushTransform(Matrix value)
        {
            dc.PushTransform(new SizeF(value.OffsetX, value.OffsetY));
        }

        public void Pop()
        {
            dc.Pop();
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