using System;
using System.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext
    {
        private DrawingContext() => throw new Exception();

        public void FillRectangle(RectangleF rectangle, Color color) => throw new Exception();

        public void DrawRectangle(RectangleF rectangle, Color color) => throw new Exception();

        public void DrawText(string text, PointF origin, Color color) => throw new Exception();

        public SizeF MeasureText(string text) => throw new Exception();
    }
}