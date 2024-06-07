using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    public class SkiaGraphics : Graphics
    {
        private SKCanvas canvas;

        public SkiaGraphics(SKBitmap bitmap)
        {
            canvas = new SKCanvas(bitmap);
        }

        public SkiaGraphics(SKCanvas canvas)
        {
            this.canvas = canvas;
        }

        public SKCanvas Canvas
        {
            get => canvas;
            set => canvas = value;
        }

        public override TransformMatrix Transform
        {
            get
            {
                var m = canvas.TotalMatrix;
                var result = (TransformMatrix)m;
                return result;
            }

            set
            {
                SKMatrix matrix = (SKMatrix)value;
                canvas.SetMatrix(matrix);
            }
        }

        public override bool IsOk
        {
            get => true;
        }
        
        public override Region? Clip
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override InterpolationMode InterpolationMode
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override object NativeObject
        {
            get => canvas;
        }

        public override SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control)
        {
            return GetTextExtent(text, font);
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(string text, Font font)
        {
            return canvas.GetTextExtent(text, font);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin)
        {
            canvas.DrawText(
                text,
                origin,
                font,
                brush.AsColor,
                Color.Empty);
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Color color)
        {
            canvas.DrawPoint((float)x, (float)y, color.AsFillPaint);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            canvas.DrawText(
                text,
                location,
                font,
                foreColor,
                backColor);
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            var skiaPoints = points.ToSkia();
            canvas.DrawPoints(SKPointMode.Polygon, skiaPoints, pen);
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            DebugPenAssert(pen);
            canvas.DrawRect(rectangle, pen);
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            DebugPenAssert(pen);
            canvas.DrawLine(a, b, pen);
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
            DebugPenAssert(pen);
            SKRoundRect roundRect = new(rect, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, pen);
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
            DebugBrushAssert(brush);
            SKRoundRect roundRect = new(rect, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, brush);
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            canvas.DrawRect(rectangle, brush);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin)
        {
            DebugImageAssert(image);
            canvas.DrawBitmap((SKBitmap)image, origin);
        }

        /// <inheritdoc/>
        public override void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            canvas.DrawBezier(pen, startPoint, controlPoint1, controlPoint2, endPoint);
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            canvas.DrawBeziers(pen, points);
        }

        public override void DrawRotatedText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            throw new NotImplementedException();
        }

        public override bool Blit(
            PointD destPt,
            SizeD sz,
            Graphics source,
            PointD srcPt,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            throw new NotImplementedException();
        }

        public override bool StretchBlit(
            PointD dstPt,
            SizeD dstSize,
            Graphics source,
            PointD srcPt,
            SizeD srcSize,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            throw new NotImplementedException();
        }

        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
            throw new NotImplementedException();
        }

        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public override void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            throw new NotImplementedException();
        }

        public override void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
            throw new NotImplementedException();
        }

        public override void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        public override void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate)
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        public override void FillRectangles(Brush brush, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        public override void FillEllipse(Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        public override void FloodFill(Brush brush, PointD point)
        {
            throw new NotImplementedException();
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public override void FillPath(Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public override void DrawLines(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(Image image, RectD destinationRect)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(PointD point, Pen pen)
        {
            canvas.DrawPoint((float)point.X, (float)point.Y, pen.Color.AsFillPaint);
        }

        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
            canvas.DrawPoint((float)x, (float)y, pen.Color.AsFillPaint);
        }

        public override Color GetPixel(PointD point)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(
            Image image,
            RectD destinationRect,
            RectD sourceRect,
            GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        public override RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            GenericAlignment alignment = GenericAlignment.Left,
            int indexAccel = -1)
        {
            throw new NotImplementedException();
        }

        public override SizeI GetDPI()
        {
            return 96;
        }

        public override void DestroyClippingRegion()
        {
            throw new NotImplementedException();
        }

        public override void SetClippingRegion(RectD rect)
        {
            throw new NotImplementedException();
        }

        public override RectD GetClippingBox()
        {
            throw new NotImplementedException();
        }

        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }
    }
}