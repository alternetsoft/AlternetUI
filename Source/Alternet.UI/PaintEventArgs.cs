using System;
using System.Drawing;

namespace Alternet.UI
{
    public class PaintEventArgs : EventArgs
    {
        internal PaintEventArgs(DrawingContext drawingContext, RectangleF bounds)
        {
            DrawingContext = drawingContext;
            Bounds = bounds;
        }

        public DrawingContext DrawingContext { get; }

        public RectangleF Bounds { get; }
    }
}