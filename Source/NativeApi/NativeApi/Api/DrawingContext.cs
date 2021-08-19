using System;
using System.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext
    {
        private DrawingContext() => throw new Exception();

        public void FillRectangle(RectangleF rectangle, Brush brush) => throw new Exception();

        public void DrawRectangle(RectangleF rectangle, Pen pen) => throw new Exception();

        public void FillEllipse(RectangleF bounds, Brush brush) => throw new Exception();

        public void DrawEllipse(RectangleF bounds, Pen pen) => throw new Exception();

        public void DrawText(string text, PointF origin, Font font, Brush brush) => throw new Exception();

        public void DrawImage(Image image, PointF origin) => throw new Exception();

        public SizeF MeasureText(string text, Font font) => throw new Exception();

        public void PushTransform(SizeF translation) => throw new Exception();

        public void Pop() => throw new Exception();
    }
}