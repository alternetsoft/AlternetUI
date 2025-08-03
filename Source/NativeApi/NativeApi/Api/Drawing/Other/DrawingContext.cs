#pragma warning disable
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    using Coord = double;

    public class DrawingContext
    {
        public static DrawingContext CreateMemoryDC(double scaleFactor) => default;

        public static DrawingContext CreateMemoryDCFromImage(Image image) => default;

        public bool IsOk { get; }
        public IntPtr WxWidgetDC { get; }
        public IntPtr GetHandle() => default; 

        public void DestroyClippingRegion() { }

        public void SetClippingRegion(RectD rect) { }

        public RectD GetClippingBox() => default;

        /*
        str The text to draw.
        x The x coordinate position to draw the text at.
        y The y coordinate position to draw the text at.
        angle The angle, in radians, relative to the (default) horizontal direction to draw the string.
        backgroundBrush Brush to fill the text with.
        */
        public void DrawText(string text, PointD location, Font font,
            Color foreColor, Brush backColor, double angle, bool useBrush)
        {
        }        

        public SizeI GetDpi() => default;

        protected DrawingContext() { }

        public static void ImageFromDrawingContext(Image image, int width,
            int height, DrawingContext dc) { }

        public static void ImageFromGenericImageDC(Image image, IntPtr source,
            DrawingContext dc) { }

        //text The text string to measure.
        //width Variable to store the total calculated width of the text.
        //height Variable to store the total calculated height of the text.
        //descent Variable to store the dimension from the baseline of the font to the
        //bottom of the descender.
        //externalLeading Any extra vertical space added to the font by the font designer
        //(usually is zero).
        //Gets the dimensions of the string using the currently selected font.
        //This function only works with single-line strings.
        public SizeD GetTextExtentSimple(string text, Font font, IntPtr control) => default;

        public static DrawingContext FromImage(Image image) => throw new Exception();

        public static DrawingContext FromScreen() => throw new Exception();

        public void RoundedRectangle(Pen pen, Brush brush, RectD rectangle,
            Coord cornerRadius) { }
        public void Rectangle(Pen pen, Brush brush, RectD rectangle) { }
        public void Ellipse(Pen pen, Brush brush, RectD rectangle) { }
        public void Path(Pen pen, Brush brush, GraphicsPath path) { }
        public void Pie(Pen pen, Brush brush, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) { }
        public void Circle(Pen pen, Brush brush, PointD center, Coord radius) { }
        public void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode) { }

        public void FillRectangle(Brush brush, RectD rectangle) { }

        public void FillRectangleI(Brush brush, RectI rectangle) { }

        public void DrawRectangle(Pen pen, RectD rectangle) { }

        public void FillEllipse(Brush brush, RectD bounds) { }
        public void DrawEllipse(Pen pen, RectD bounds) { }

        public void DrawPath(Pen pen, GraphicsPath path) { }
        public void FillPath(Brush brush, GraphicsPath path) { }

        public void DrawImageAtPoint(Image image, PointD origin, bool useMask = false)
            => throw new Exception();

        public void DrawImageAtRect(Image image, RectD destinationRect, bool useMask = false)
            => throw new Exception();

        public void SetTransformValues(
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy) => throw new Exception();

        public void DrawLine(Pen pen, PointD a, PointD b) => throw new Exception();

        public void DrawLines(Pen pen, PointD[] points) => throw new Exception();

        public void DrawArc(Pen pen, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) => throw new Exception();

        public void FillPie(Brush brush, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) => throw new Exception();
        
        public void DrawPie(Pen pen, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) => throw new Exception();

        public void DrawBezier(Pen pen, PointD startPoint, PointD controlPoint1,
            PointD controlPoint2, PointD endPoint) => throw new Exception();

        public void DrawBeziers(Pen pen, PointD[] points) => throw new Exception();

        public void DrawPoint(Pen pen, Coord x, Coord y) { }

        public void DrawCircle(Pen pen, PointD center, Coord radius) => throw new Exception();

        public void FillCircle(Brush brush, PointD center, Coord radius) => throw new Exception();

        public void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
            => throw new Exception();

        public void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
            => throw new Exception();

        public void DrawPolygon(Pen pen, PointD[] points) => throw new Exception();

        public void FillPolygon(Brush brush, PointD[] points, FillMode fillMode)
            => throw new Exception();

        public Region? Clip { get; set; }

        public InterpolationMode InterpolationMode { get; set; }
    }
}