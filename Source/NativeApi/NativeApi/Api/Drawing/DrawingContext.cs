using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext
    {
        protected DrawingContext() => throw new Exception();

        public static DrawingContext FromImage(Image image) => throw new Exception();

        public void FillRectangle(Brush brush, Rect rectangle) => throw new Exception();

        public void DrawRectangle(Pen pen, Rect rectangle) => throw new Exception();

        public void FillEllipse(Brush brush, Rect bounds) => throw new Exception();

        public void DrawEllipse(Pen pen, Rect bounds) => throw new Exception();

        public void FloodFill(Brush brush, Point point) => throw new Exception();

        public void DrawPath(Pen pen, GraphicsPath path) => throw new Exception();
        public void FillPath(Brush brush, GraphicsPath path) => throw new Exception();

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
            TextTrimming trimming,
            TextWrapping wrapping) => throw new Exception();

        public void DrawImageAtPoint(Image image, Point origin) => throw new Exception();

        public void DrawImageAtRect(Image image, Rect rect) => throw new Exception();

        public Size MeasureText(string text, Font font, double maximumWidth, TextWrapping textWrapping) => throw new Exception();

        public void Push() => throw new Exception();

        public void Pop() => throw new Exception();

        public TransformMatrix Transform { get; set; }

        public void DrawLine(Pen pen, Point a, Point b) => throw new Exception();

        public void DrawLines(Pen pen, Point[] points) => throw new Exception();

        public void DrawArc(Pen pen, Point center, double radius, double startAngle, double sweepAngle) => throw new Exception();

        public void FillPie(Brush brush, Point center, double radius, double startAngle, double sweepAngle) => throw new Exception();
        
        public void DrawPie(Pen pen, Point center, double radius, double startAngle, double sweepAngle) => throw new Exception();

        public void DrawBezier(Pen pen, Point startPoint, Point controlPoint1, Point controlPoint2, Point endPoint) => throw new Exception();

        public void DrawBeziers(Pen pen, Point[] points) => throw new Exception();

        public void DrawCircle(Pen pen, Point center, double radius) => throw new Exception();

        public void FillCircle(Brush brush, Point center, double radius) => throw new Exception();

        public void DrawRoundedRectangle(Pen pen, Rect rect, double cornerRadius) => throw new Exception();

        public void FillRoundedRectangle(Brush brush, Rect rect, double cornerRadius) => throw new Exception();

        public void DrawPolygon(Pen pen, Point[] points) => throw new Exception();

        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode) => throw new Exception();

        public void DrawRectangles(Pen pen, Rect[] rects) => throw new Exception();

        public void FillRectangles(Brush brush, Rect[] rects) => throw new Exception();
    }
}