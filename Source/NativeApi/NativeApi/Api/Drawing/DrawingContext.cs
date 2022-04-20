using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext
    {
        private DrawingContext() => throw new Exception();

        public void FillRectangle(Rect rectangle, Brush brush) => throw new Exception();

        public void DrawRectangle(Rect rectangle, Pen pen) => throw new Exception();

        public void FillEllipse(Rect bounds, Brush brush) => throw new Exception();

        public void DrawEllipse(Rect bounds, Pen pen) => throw new Exception();

        public void DrawText(string text, Point origin, Font font, Brush brush) => throw new Exception();

        public void DrawImage(Image image, Point origin) => throw new Exception();

        public Size MeasureText(string text, Font font) => throw new Exception();

        public void PushTransform(Size translation) => throw new Exception();

        public void Pop() => throw new Exception();

        public void DrawLine(Point a, Point b, Pen pen) => throw new Exception();

        public void DrawLines(Point[] points, Pen pen) => throw new Exception();
    }
}