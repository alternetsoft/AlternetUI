#pragma warning disable
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    using Coord = float;

    public class DrawingContext
    {
        public static DrawingContext CreateMemoryDC(Coord scaleFactor) => default;

        public static DrawingContext CreateMemoryDCFromImage(Image image) => default;

        public bool IsOk { get; }
        public IntPtr WxWidgetDC { get; }
        public IntPtr GetHandle() => default; 

        public void DestroyClippingRegion() { }

        public void Save() { }

        public void Restore() { }

        public void SetClippingRect(RectD rect) { }

        public void SetClippingRegion(Region region) { }

        public RectD GetClippingBox() => default;

        /*
        str The text to draw.
        x The x coordinate position to draw the text at.
        y The y coordinate position to draw the text at.
        angle The angle, in radians, relative to the (default) horizontal direction to draw the string.
        backgroundBrush Brush to fill the text with.
        */
        public unsafe void DrawText(IntPtr text, int charLength, PointD location, Font font,
            Color foreColor, Brush backColor, Coord angle, bool useBrush)
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
        public unsafe SizeD GetTextExtentSimple(IntPtr text, int charLength, Font font, IntPtr control) => default;

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
        public unsafe void Polygon(Pen pen, Brush brush,
            PointD* points, int pointsLength,  FillMode fillMode) { }

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
            Coord m11,
            Coord m12,
            Coord m21,
            Coord m22,
            Coord dx,
            Coord dy) => throw new Exception();

        public void DrawLine(Pen pen, PointD a, PointD b) => throw new Exception();

        public unsafe void DrawLines(Pen pen, PointD* points, int pointsLength) => throw new Exception();

        public void DrawArc(Pen pen, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) => throw new Exception();

        public void FillPie(Brush brush, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) => throw new Exception();
        
        public void DrawPie(Pen pen, PointD center, Coord radius, Coord startAngle,
            Coord sweepAngle) => throw new Exception();

        public void DrawBezier(Pen pen, PointD startPoint, PointD controlPoint1,
            PointD controlPoint2, PointD endPoint) => throw new Exception();

        public unsafe void DrawBeziers(Pen pen, PointD* points, int pointsLength) => throw new Exception();

        public void DrawPoint(Pen pen, Coord x, Coord y) { }

        public void DrawCircle(Pen pen, PointD center, Coord radius) => throw new Exception();

        public void FillCircle(Brush brush, PointD center, Coord radius) => throw new Exception();

        public void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
            => throw new Exception();

        public void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
            => throw new Exception();

        public unsafe void DrawPolygon(Pen pen, PointD* points, int pointsLength) => throw new Exception();

        public unsafe void FillPolygon(Brush brush, PointD* points, int pointsLength, FillMode fillMode)
            => throw new Exception();

        public Region? Clip { get; set; }

        public InterpolationMode InterpolationMode { get; set; }
    }
}