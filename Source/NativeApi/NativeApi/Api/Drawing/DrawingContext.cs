#pragma warning disable
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class DrawingContext
    {
        protected DrawingContext() { }

        // Fills the widths array with the widths from the beginning of text
        // to the corresponding character of text.
        // The generic version simply builds a running total of the widths of each character
        // using GetTextExtent(), however if the various platforms have a native API function
        // that is faster or more accurate than the generic implementation then it should be used instead.
        public void GetPartialTextExtents(string text, double[]	widths, Font font, IntPtr control) { }

        //X wxDouble* width,
        //Y wxDouble* height,
        //W wxDouble* descent,
        //H wxDouble* externalLeading
        //text The text string to measure.
        //width Variable to store the total calculated width of the text.
        //height Variable to store the total calculated height of the text.
        //descent Variable to store the dimension from the baseline of the font to the bottom of the descender.
        //externalLeading Any extra vertical space added to the font by the font designer (usually is zero).
        //Gets the dimensions of the string using the currently selected font.
        //This function only works with single-line strings.
        public Rect GetTextExtent(string text, Font font, IntPtr control) => default;

        public Size MeasureText(
            string text,
            Font font,
            double maximumWidth,
            TextWrapping textWrapping) => throw new Exception();

        public static DrawingContext FromImage(Image image) => throw new Exception();

        public void RoundedRectangle(Pen pen, Brush brush, Rect rectangle, double cornerRadius) { }
        public void Rectangle(Pen pen, Brush brush, Rect rectangle) { }
        public void Ellipse(Pen pen, Brush brush, Rect rectangle) { }
        public void Path(Pen pen, Brush brush, GraphicsPath path) { }
        public void Pie(Pen pen, Brush brush, Point center, double radius, double startAngle,
            double sweepAngle) { }
        public void Circle(Pen pen, Brush brush, Point center, double radius) { }
        public void Polygon(Pen pen, Brush brush, Point[] points, FillMode fillMode) { }

        public void FillRectangle(Brush brush, Rect rectangle) { }
        public void DrawRectangle(Pen pen, Rect rectangle) { }

        public void FillEllipse(Brush brush, Rect bounds) { }
        public void DrawEllipse(Pen pen, Rect bounds) { }

        public void FloodFill(Brush brush, Point point) { }

        public void DrawPath(Pen pen, GraphicsPath path) { }
        public void FillPath(Brush brush, GraphicsPath path) { }

        public void DrawTextAtPoint(
            string text,
            Point origin,
            Font font,
            Brush brush) {}

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

        public void DrawImageAtRect(Image image, Rect destinationRect) => throw new Exception();

        public void DrawImagePortionAtRect(Image image, Rect destinationRect, Rect sourceRect) { }

        public void Push() => throw new Exception();

        public void Pop() => throw new Exception();

        public TransformMatrix Transform { get; set; }

        public void DrawLine(Pen pen, Point a, Point b) => throw new Exception();

        public void DrawLines(Pen pen, Point[] points) => throw new Exception();

        public void DrawArc(Pen pen, Point center, double radius, double startAngle,
            double sweepAngle) => throw new Exception();

        public void FillPie(Brush brush, Point center, double radius, double startAngle,
            double sweepAngle) => throw new Exception();
        
        public void DrawPie(Pen pen, Point center, double radius, double startAngle,
            double sweepAngle) => throw new Exception();

        public void DrawBezier(Pen pen, Point startPoint, Point controlPoint1,
            Point controlPoint2, Point endPoint) => throw new Exception();

        public void DrawBeziers(Pen pen, Point[] points) => throw new Exception();

        public void DrawPoint(Pen pen, double x, double y) { }

        public void DrawCircle(Pen pen, Point center, double radius) => throw new Exception();

        public void FillCircle(Brush brush, Point center, double radius) => throw new Exception();

        public void DrawRoundedRectangle(Pen pen, Rect rect, double cornerRadius)
            => throw new Exception();

        public void FillRoundedRectangle(Brush brush, Rect rect, double cornerRadius)
            => throw new Exception();

        public void DrawPolygon(Pen pen, Point[] points) => throw new Exception();

        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
            => throw new Exception();

        public void DrawRectangles(Pen pen, Rect[] rects) => throw new Exception();

        public void FillRectangles(Brush brush, Rect[] rects) => throw new Exception();

        /*public Color GetPixel(Point p) => throw new Exception();*/

        public Region? Clip { get; set; }

        public InterpolationMode InterpolationMode { get; set; }
    }
}