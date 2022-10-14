using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext
    {
        protected DrawingContext() => throw new Exception();

        public static DrawingContext FromImage(Image image) => throw new Exception();

        public void FillRectangle(Rect rectangle, Brush brush) => throw new Exception();

        public void DrawRectangle(Rect rectangle, Pen pen) => throw new Exception();

        public void FillEllipse(Rect bounds, Brush brush) => throw new Exception();

        public void DrawEllipse(Rect bounds, Pen pen) => throw new Exception();

        public void FloodFill(Point point, Brush brush) => throw new Exception();

        public void DrawTextAtPoint(
            string text,
            Point origin,
            Font font,
            Brush brush) => throw new Exception();

        public void DrawTextAtRect(
            string text,
            Rect bounds,
            Font font,
            Brush brush,
            TextHorizontalAlignment horizontalAlignment,
            TextVerticalAlignment verticalAlignment,
            TextTrimming trimming) => throw new Exception();

        public void DrawImageAtPoint(Image image, Point origin) => throw new Exception();

        public void DrawImageAtRect(Image image, Rect rect) => throw new Exception();

        public Size MeasureText(string text, Font font, double maximumWidth) => throw new Exception();

        public void PushTransform(Size translation) => throw new Exception();

        public void Pop() => throw new Exception();

        public void DrawLine(Point a, Point b, Pen pen) => throw new Exception();

        public void DrawLines(Point[] points, Pen pen) => throw new Exception();
    }
}