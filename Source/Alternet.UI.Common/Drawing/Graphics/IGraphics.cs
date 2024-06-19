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

        void DrawRotatedText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle,
            GraphicsUnit unit = GraphicsUnit.Dip);

        SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control);

        bool Blit(
            PointD destPt,
            SizeD sz,
            Graphics source,
            PointD srcPt,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip);

        bool StretchBlit(
            PointD dstPt,
            SizeD dstSize,
            Graphics source,
            PointD srcPt,
            SizeD srcSize,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip);

        void RoundedRectangle(
            Pen pen,
            Brush brush,
            RectD rectangle,
            Coord cornerRadius);

        SizeD GetTextExtent(string text, Font font);

        void Rectangle(Pen pen, Brush brush, RectD rectangle);

        void Ellipse(Pen pen, Brush brush, RectD rectangle);

        void Path(Pen pen, Brush brush, GraphicsPath path);

        void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        void Circle(Pen pen, Brush brush, PointD center, Coord radius);

        void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode);

        void FillRectangle(Brush brush, RectD rectangle);

        void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        void DrawPoint(Pen pen, Coord x, Coord y);

        void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        void DrawBeziers(Pen pen, PointD[] points);

        void DrawCircle(Pen pen, PointD center, Coord radius);

        void FillCircle(Brush brush, PointD center, Coord radius);

        void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius);

        void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius);

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

        void DrawLine(Pen pen, Coord x1, Coord y1, Coord x2, Coord y2);

        void DrawEllipse(Pen pen, RectD bounds);

        void DrawImageUnscaled(Image image, PointD origin);

        void DrawImage(Image image, PointD origin);

        void DrawImage(Image image, RectD destinationRect);

        void DrawImage(Image image, RectD destinationRect, RectD sourceRect);

        void SetPixel(PointD point, Pen pen);

        void SetPixel(Coord x, Coord y, Pen pen);

        void SetPixel(Coord x, Coord y, Color color);

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

        SizeI GetDPI();

        void DestroyClippingRegion();

        void SetClippingRegion(RectD rect);

        RectD GetClippingBox();
    }
}
