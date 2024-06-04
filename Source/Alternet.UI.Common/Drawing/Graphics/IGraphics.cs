using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IGraphics
    {
        bool IsOk { get; }

        string? Name { get; set; }

        TransformMatrix Transform { get; set; }

        Region? Clip { get; set; }

        InterpolationMode InterpolationMode { get; set; }

        object NativeObject { get; }

        void DrawRotatedTextI(
            string text,
            PointI location,
            Font font,
            Color foreColor,
            Color backColor,
            double angle);

        SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control);

        bool BlitI(
            PointI destPt,
            SizeI sz,
            Graphics source,
            PointI srcPt,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointI? srcPtMask = null);

        bool StretchBlitI(
            PointI dstPt,
            SizeI dstSize,
            Graphics source,
            PointI srcPt,
            SizeI srcSize,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointI? srcPtMask = null);

        void RoundedRectangle(
            Pen pen,
            Brush brush,
            RectD rectangle,
            double cornerRadius);

        SizeD GetTextExtent(string text, Font font);

        void Rectangle(Pen pen, Brush brush, RectD rectangle);

        void Ellipse(Pen pen, Brush brush, RectD rectangle);

        void Path(Pen pen, Brush brush, GraphicsPath path);

        void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        void Circle(Pen pen, Brush brush, PointD center, double radius);

        void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode);

        void FillRectangle(Brush brush, RectD rectangle);

        void FillRectangleI(Brush brush, RectI rectangle);

        void DrawArc(
            Pen pen,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        void DrawPoint(Pen pen, double x, double y);

        void FillPie(
            Brush brush,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        void DrawPie(
            Pen pen,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        void DrawBeziers(Pen pen, PointD[] points);

        void DrawCircle(Pen pen, PointD center, double radius);

        void FillCircle(Brush brush, PointD center, double radius);

        void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius);

        void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius);

        void DrawPolygon(Pen pen, PointD[] points);

        void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate);

        void DrawRectangles(Pen pen, RectD[] rects);

        void FillRectangles(Brush brush, RectD[] rects);

        void FillEllipse(Brush brush, RectD bounds);

        void FloodFill(Brush brush, PointD point);

        void DrawRectangle(Pen pen, RectD rectangle);

        void DrawPath(Pen pen, GraphicsPath path);

        void FillPath(Brush brush, GraphicsPath path);

        void DrawLine(Pen pen, PointD a, PointD b);

        void DrawLine(Pen pen, double x1, double y1, double x2, double y2);

        void DrawEllipse(Pen pen, RectD bounds);

        void DrawImageUnscaled(Image image, PointD origin);

        void DrawImage(Image image, PointD origin, bool useMask = false);

        void DrawImage(Image image, RectD destinationRect, bool useMask = false);

        void DrawImage(Image image, RectD destinationRect, RectD sourceRect);

        void DrawImageI(Image image, RectI destinationRect, RectI sourceRect);

        void SetPixel(PointD point, Pen pen);

        void SetPixel(double x, double y, Pen pen);

        void SetPixel(double x, double y, Color color);

        Color GetPixel(PointD point);

        void DrawImage(
            Image image,
            RectD destinationRect,
            RectD sourceRect,
            GraphicsUnit unit);

        void DrawText(string text, Font font, Brush brush, PointD origin);

        void DrawText(string[] text, Font font, Brush brush, PointD origin);

        void DrawText(string text, PointD origin);

        void DrawText(string text, Font font, Brush brush, RectD bounds);

        void DrawWave(RectD rect, Color color);
 
        SizeD MeasureText(string text, Font font);

        void Pop();

        void PushTransform(TransformMatrix transform);

        void Push();

        void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor);

        RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            GenericAlignment alignment = GenericAlignment.TopLeft,
            int indexAccel = -1);

        SizeD GetDPI();

        void DestroyClippingRegion();

        void SetClippingRegion(RectD rect);

        RectD GetClippingBox();
    }
}
