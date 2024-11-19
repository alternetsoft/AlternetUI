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
    public partial class SkiaGraphics : Graphics
    {
        private SKCanvas canvas;
        private SKBitmap? bitmap;
        private InterpolationMode interpolationMode = InterpolationMode.HighQuality;

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
        /// Gets or sets whether 'DrawImage' methods draw unscaled image.
        /// </summary>
        public bool UseUnscaledDrawImage { get; set; }

        /// <summary>
        /// Gets or sets original scale factor applied to the canvas before the first
        /// drawing is performed.
        /// </summary>
        public float OriginalScaleFactor { get; set; } = 1f;

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
        public override bool IsOk
        {
            get => true;
        }

        /// <inheritdoc/>
        public override InterpolationMode InterpolationMode
        {
            get
            {
                return interpolationMode;
            }

            set
            {
                interpolationMode = value;
            }
        }

        /// <inheritdoc/>
        public override object NativeObject
        {
            get => canvas;
        }

        internal SKPaint? InterpolationModePaint => SkiaUtils.InterpolationModePaints[InterpolationMode];

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
            DebugPenAssert(pen);
            canvas.DrawBeziers(pen, points);
        }

        /// <summary>
        /// Gets <see cref="SKPaint"/> for the specifed brush and pen.
        /// </summary>
        /// <param name="pen">Pen to use.</param>
        /// <param name="brush">Brush to use.</param>
        /// <returns></returns>
        public virtual SKPaint GetFillAndStrokePaint(Pen pen, Brush brush)
        {
            DebugAssert(pen, brush);
            BrushAndPen brushAndPen = new(brush, pen);
            return GraphicsFactory.CreateStrokeAndFillPaint(brushAndPen);
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            SKRoundRect roundRect = new(rectangle, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, paint);
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            canvas.DrawRect(rectangle, paint);
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            canvas.DrawOval(rectangle, paint);
        }

        /// <inheritdoc/>
        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            canvas.DrawCircle(center, (float)radius, paint);
        }

        /// <inheritdoc/>
        public override void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            DebugPenAssert(pen);
            var rect = RectD.GetCircleBoundingBox(center, radius);
            canvas.DrawArc(rect, (float)startAngle, (float)sweepAngle, true, pen);
        }

        /// <inheritdoc/>
        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
            DebugPenAssert(pen);
            canvas.DrawPoint((float)x, (float)y, pen.Color.AsFillPaint);
        }

        /// <inheritdoc/>
        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
            DebugPenAssert(pen);
            canvas.DrawCircle(center, (float)radius, pen);
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
            DebugBrushAssert(brush);
            canvas.DrawCircle(center, (float)radius, brush);
        }

        /// <inheritdoc/>
        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
            foreach (var rect in rects)
                DrawRectangle(pen, rect);
        }

        /// <inheritdoc/>
        public override void FillRectangles(Brush brush, RectD[] rects)
        {
            foreach (var rect in rects)
                FillRectangle(brush, rect);
        }

        /// <inheritdoc/>
        public override void FillEllipse(Brush brush, RectD bounds)
        {
            DebugBrushAssert(brush);
            canvas.DrawOval(bounds, brush);
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            var skiaPoints = points.ToSkia();
            canvas.DrawPoints(SkiaSharp.SKPointMode.Lines, skiaPoints, pen);
        }

        /// <inheritdoc/>
        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            DebugPenAssert(pen);
            canvas.DrawOval(bounds, pen);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin)
        {
            BeforeDrawImage(ref image, ref origin);
            canvas.DrawBitmap((SKBitmap)image, origin, InterpolationModePaint);
            AfterDrawImage();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect)
        {
            var origin = destinationRect.Location;
            if (BeforeDrawImage(ref image, ref origin))
                destinationRect.Location = origin;
            canvas.DrawBitmap((SKBitmap)image, destinationRect, InterpolationModePaint);
            AfterDrawImage();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            var origin = destinationRect.Location;
            if (BeforeDrawImage(ref image, ref origin))
            {
                destinationRect.Location = origin;
                sourceRect.ScaleLocation(OriginalScaleFactor);
            }

            canvas.DrawBitmap((SKBitmap)image, sourceRect, destinationRect, InterpolationModePaint);
            AfterDrawImage();
        }

        /// <inheritdoc/>
        public override void DrawImage(
            Image image,
            RectD destinationRect,
            RectD sourceRect,
            GraphicsUnit unit)
        {
            ToDip(ref destinationRect, unit);
            ToDip(ref sourceRect, unit);
            DrawImage(image, destinationRect, sourceRect);
        }

        /// <inheritdoc/>
        public override void SetPixel(PointD point, Pen pen)
        {
            DebugPenAssert(pen);
            canvas.DrawPoint((float)point.X, (float)point.Y, pen.Color.AsFillPaint);
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
            DebugPenAssert(pen);
            canvas.DrawPoint((float)x, (float)y, pen.Color.AsFillPaint);
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
        public override RectD GetClippingBox()
        {
            return canvas.LocalClipBounds;
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
            ToDip(ref rectangle, unit);
            FillRectangle(brush, rectangle);
        }

        /// <summary>
        /// Called before any draw image operation.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="location"><see cref="PointD"/> structure that represents the
        /// upper-left corner of the drawn image on the destination drawing context.</param>
        protected virtual bool BeforeDrawImage(ref Image image, ref PointD location)
        {
            DebugImageAssert(image);

            if (UseUnscaledDrawImage && OriginalScaleFactor != 1f)
            {
                location.X *= OriginalScaleFactor;
                location.Y *= OriginalScaleFactor;
                canvas.Save();
                canvas.Scale(1 / OriginalScaleFactor);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called after any draw image operation.
        /// </summary>
        protected virtual void AfterDrawImage()
        {
            if (UseUnscaledDrawImage && OriginalScaleFactor != 1f)
            {
                canvas.Restore();
            }
        }

        /// <inheritdoc/>
        protected override void SetHandlerTransform(TransformMatrix matrix)
        {
            var scaleFactor = OriginalScaleFactor;
            SKMatrix native = (SKMatrix)matrix;

            if (OriginalScaleFactor == 1f)
            {
                canvas.SetMatrix(native);
            }
            else
            {
                var scaleMatrix = SKMatrix.CreateScale(scaleFactor, scaleFactor);
                var result = scaleMatrix.PreConcat(native);
                canvas.SetMatrix(result);
            }
        }
    }
}