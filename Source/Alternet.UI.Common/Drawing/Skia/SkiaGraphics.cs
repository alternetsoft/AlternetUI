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
    /// <summary>
    /// Implements <see cref="Graphics"/> over <see cref="SKCanvas"/> or <see cref="SKBitmap"/>.
    /// </summary>
    public class SkiaGraphics : Graphics
    {
        private SKCanvas canvas;
        private SKBitmap? bitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaGraphics"/> class.
        /// </summary>
        /// <param name="bitmap"><see cref="SKBitmap"/> where drawing will be performed.</param>
        public SkiaGraphics(SKBitmap bitmap)
        {
            this.bitmap = bitmap;
            canvas = new SKCanvas(bitmap);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaGraphics"/> class.
        /// </summary>
        /// <param name="canvas"><see cref="SKCanvas"/> where painting will be performed.</param>
        public SkiaGraphics(SKCanvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Gets or sets <see cref="SKBitmap"/> where drawing will be performed.
        /// </summary>
        public SKBitmap? Bitmap
        {
            get => bitmap;

            set
            {
                if (bitmap == value && bitmap is not null)
                    return;
                bitmap = value;
                if (value is null)
                    canvas = SkiaUtils.CreateNullCanvas();
                else
                    canvas = new SKCanvas(bitmap);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="SKCanvas"/>  where drawing will be performed.
        /// </summary>
        public SKCanvas Canvas
        {
            get => canvas;

            set
            {
                if (canvas == value)
                    return;
                bitmap = null;
                if (value is null)
                    canvas = SkiaUtils.CreateNullCanvas();
                else
                    canvas = value;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override bool IsOk
        {
            get => true;
        }

        /// <inheritdoc/>
        public override Region? Clip
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override InterpolationMode InterpolationMode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override object NativeObject
        {
            get => canvas;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
            return false;
        }

        /// <inheritdoc/>
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
            return false;
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillRectangles(Brush brush, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillEllipse(Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FloodFill(Brush brush, PointD point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPath(Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetPixel(PointD point, Pen pen)
        {
            canvas.DrawPoint((float)point.X, (float)point.Y, pen.Color.AsFillPaint);
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
            canvas.DrawPoint((float)x, (float)y, pen.Color.AsFillPaint);
        }

        /// <inheritdoc/>
        public override Color GetPixel(PointD point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(
            Image image,
            RectD destinationRect,
            RectD sourceRect,
            GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override SizeI GetDPI()
        {
            var m = canvas.TotalMatrix;
            if (m.IsIdentity)
                return new(96, 96);
            var dpiX = 96f * m.ScaleX;
            var dpiY = 96f * m.ScaleY;
            return new((int)dpiX, (int)dpiY);
        }

        /// <inheritdoc/>
        public override void DestroyClippingRegion()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetClippingRegion(RectD rect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override RectD GetClippingBox()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }
    }
}