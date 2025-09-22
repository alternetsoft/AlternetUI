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
        private bool ignoreSetHandlerTransform;
        private SKCanvas canvas;
        private SKBitmap? bitmap;
        private InterpolationMode interpolationMode = InterpolationMode.HighQuality;
        private SKMatrix? initialMatrix;

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
        /// Gets a value indicating whether the scale factor is 1f.
        /// </summary>
        public bool IsUnscaled => OriginalScaleFactor == 1f;

        /// <summary>
        /// Gets a value indicating whether the object is scaled.
        /// </summary>
        public bool IsScaled => OriginalScaleFactor != 1f;

        /// <inheritdoc/>
        public override GraphicsBackendType BackendType => GraphicsBackendType.SkiaSharp;

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
        /// Gets or sets the initial transformation matrix applied to the canvas
        /// before the first drawing is performed.
        /// </summary>
        public SKMatrix? InitialMatrix
        {
            get => initialMatrix;

            set
            {
                initialMatrix = value;
            }
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

        internal SKPaint? InterpolationModePaint
            => SkiaUtils.InterpolationModePaints[InterpolationMode];

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
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DoInsideClipped(bounds, () =>
            {
                canvas.DrawText(
                    text,
                    bounds.Location,
                    font,
                    brush.AsColor,
                    Color.Empty);
            });
        }

        /// <summary>
        /// Sets pixel at the specified coordinates to the specified color.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        /// <param name="y">The Y coordinate of the point.</param>
        /// <param name="color">The color used to set the pixel.</param>
        public void SetPixel(Coord x, Coord y, Color color)
        {
            canvas.DrawPoint(x, y, color.AsFillPaint);
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
        public override void DrawTextWithAngle(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle)
        {
            canvas.Save();
            canvas.Translate(location.X, location.Y);
            canvas.RotateDegrees(angle);

            canvas.DrawText(
                text,
                location,
                font,
                foreColor,
                backColor);

            canvas.Restore();
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            canvas.DrawPoints(SKPointMode.Polygon, PointD.ToSkiaArray(points), pen);
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

        /// <inheritdoc/>
        public override void ClipRect(RectD rect, bool antialiasing = true)
        {
            canvas.ClipRect(
                rect,
                SKClipOperation.Intersect,
                antialiasing);
        }

        /// <summary>
        /// Gets <see cref="SKPaint"/> for the specified brush and pen.
        /// </summary>
        /// <param name="pen">Pen to use.</param>
        /// <param name="brush">Brush to use.</param>
        /// <returns></returns>
        public virtual (SKPaint? Fill, SKPaint? Stroke) GetFillAndStrokePaint(Pen? pen, Brush? brush)
        {
            return (brush?.SkiaPaint, pen?.SkiaPaint);
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
            var (fill, stroke) = GetFillAndStrokePaint(pen, brush);
            SKRoundRect roundRect = new(rectangle, (float)cornerRadius);
            if(fill is not null)
                canvas.DrawRoundRect(roundRect, fill);
            if (stroke is not null)
                canvas.DrawRoundRect(roundRect, stroke);
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            var (fill, stroke) = GetFillAndStrokePaint(pen, brush);
            SKRect rect = rectangle;
            if (fill is not null)
                canvas.DrawRect(rect, fill);
            if (stroke is not null)
                canvas.DrawRect(rect, stroke);
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            var (fill, stroke) = GetFillAndStrokePaint(pen, brush);
            SKRect rect = rectangle;
            if (fill is not null)
                canvas.DrawOval(rect, fill);
            if (stroke is not null)
                canvas.DrawOval(rect, stroke);
        }

        /// <inheritdoc/>
        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
            var (fill, stroke) = GetFillAndStrokePaint(pen, brush);
            var radiusF = (float)radius;
            SKPoint centerF = center;
            if (fill is not null)
                canvas.DrawCircle(centerF, radiusF, fill);
            if (stroke is not null)
                canvas.DrawCircle(centerF, radiusF, stroke);
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
        public override void FillEllipse(Brush brush, RectD bounds)
        {
            DebugBrushAssert(brush);
            canvas.DrawOval(bounds, brush);
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            canvas.DrawPoints(SKPointMode.Lines, PointD.ToSkiaArray(points), pen);
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
            BeforeDrawImage(ref image, ref destinationRect);
            canvas.DrawBitmap((SKBitmap)image, destinationRect, InterpolationModePaint);
            AfterDrawImage();
        }

        /// <summary>
        /// Sets pixel at the specified coordinates to the color of the specified pen.
        /// </summary>
        /// <param name="point">The coordinates of the point.</param>
        /// <param name="pen">The pen which color is used to set the pixel.</param>
        public void SetPixel(PointD point, Pen pen)
        {
            DebugPenAssert(pen);
            canvas.DrawPoint((float)point.X, (float)point.Y, pen.Color.AsFillPaint);
        }

        /// <summary>
        /// Sets pixel at the specified coordinates to the specified color.
        /// </summary>
        /// <param name="x">The X coordinate of the point.</param>
        /// <param name="y">The Y coordinate of the point.</param>
        /// <param name="pen">The pen which color is used to set the pixel.</param>
        public void SetPixel(Coord x, Coord y, Pen pen)
        {
            DebugPenAssert(pen);
            canvas.DrawPoint((float)x, (float)y, pen.Color.AsFillPaint);
        }

        /// <summary>
        /// Creates an <see cref="SKPath"/> representing a pie segment.
        /// </summary>
        /// <param name="center">The center point of the circular pie.</param>
        /// <param name="radius">The radius of the pie segment.</param>
        /// <param name="startAngle">
        /// The angle (in degrees) at which the pie segment starts, measured clockwise from the X-axis.
        /// </param>
        /// <param name="sweepAngle">
        /// The angle (in degrees) the segment sweeps clockwise from the start angle.
        /// </param>
        /// <returns>
        /// An <see cref="SKPath"/> that describes the pie segment as an
        /// arc closed by a line to the center.
        /// </returns>
        public virtual SKPath CreatePiePath(
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            var path = new SKPath();

            SKRect oval = new(
                (float)(center.X - radius),
                (float)(center.Y - radius),
                (float)(center.X + radius),
                (float)(center.Y + radius));

            path.AddArc(oval, (float)startAngle, (float)sweepAngle);
            path.LineTo(center);
            path.Close();
            return path;
        }

        /// <inheritdoc/>
        public override void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            DebugBrushAssert(brush);
            var path = CreatePiePath(center, radius, startAngle, sweepAngle);
            canvas.DrawPath(path, brush);
        }

        /// <inheritdoc/>
        public override void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            DebugPenAssert(pen);
            var path = CreatePiePath(center, radius, startAngle, sweepAngle);
            canvas.DrawPath(path, pen);
        }

        /// <inheritdoc/>
        public override void Save()
        {
            base.Save();
            canvas.Save();
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            canvas.Restore();
            ignoreSetHandlerTransform = true;
            base.Restore();
            ignoreSetHandlerTransform = false;
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
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            var path = CreatePiePath(center, radius, startAngle, sweepAngle);
            if (brush is not null)
                canvas.DrawPath(path, brush);
            if(pen is not null)
                canvas.DrawPath(path, pen);
        }

        /// <summary>
        /// Gets <see cref="SKPath"/> from array of points and fill mode.
        /// </summary>
        /// <param name="points">The array of points.</param>
        /// <param name="fillMode">The fill mode.</param>
        /// <returns></returns>
        public virtual SKPath? GetPathFromPoints(PointD[] points, FillMode fillMode)
        {
            if (points == null || points.Length < 3)
                return null;

            var path = new SKPath
            {
                FillType = fillMode.ToSkia(),
            };

            // Move to the first point
            path.MoveTo(points[0].X, points[0].Y);

            // Draw lines between the points
            for (int i = 1; i < points.Length; i++)
            {
                path.LineTo(points[i].X, points[i].Y);
            }

            // Ensures the polygon is closed
            path.Close();

            return path;
        }

        /// <inheritdoc/>
        public override void Polygon(Pen? pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            using var path = GetPathFromPoints(points, fillMode);
            if (path is null)
                return;

            var (fill, stroke) = GetFillAndStrokePaint(pen, brush);

            if (fill is not null)
                canvas.DrawPath(path, fill);
            if (stroke is not null)
                canvas.DrawPath(path, stroke);
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            FillPath(brush, path);
            DrawPath(pen, path);
        }

        /// <inheritdoc/>
        public override void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate)
        {
            DebugBrushAssert(brush);

            var path = GetPathFromPoints(points, fillMode);
            if (path is null)
                return;

            canvas.DrawPath(path, brush);
        }

        /// <inheritdoc/>
        public override SizeI GetDPI()
        {
            if(IsUnscaled)
                return SizeI.NinetySix;
            var dpiX = 96f * OriginalScaleFactor;
            var dpiY = 96f * OriginalScaleFactor;
            return new((int)dpiX, (int)dpiY);

            /*
            var m = canvas.TotalMatrix;
            if (m.IsIdentity)
                return new(96, 96);
            var dpiX = 96f * m.ScaleX;
            var dpiY = 96f * m.ScaleY;
            return new((int)dpiX, (int)dpiY);
            */
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

            if (UseUnscaledDrawImage && IsScaled)
            {
                var sf = OriginalScaleFactor;
                location.X *= sf;
                location.Y *= sf;
                canvas.Save();
                canvas.Scale(1 / sf);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called before any draw image operation.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="rect"><see cref="RectD"/> structure that represents the
        /// bounds of the image on the destination drawing context.</param>
        protected virtual bool BeforeDrawImage(ref Image image, ref RectD rect)
        {
            DebugImageAssert(image);

            if (UseUnscaledDrawImage && IsScaled)
            {
                var sf = OriginalScaleFactor;
                rect.X *= sf;
                rect.Y *= sf;
                rect.Width *= sf;
                rect.Height *= sf;
                canvas.Save();
                canvas.Scale(1 / sf);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called after any draw image operation.
        /// </summary>
        protected virtual void AfterDrawImage()
        {
            if (UseUnscaledDrawImage && IsScaled)
            {
                canvas.Restore();
            }
        }

        /// <inheritdoc/>
        protected override void SetHandlerTransform(TransformMatrix matrix)
        {
            if (ignoreSetHandlerTransform)
                return;

            SKMatrix native = (SKMatrix)matrix;

            SKMatrix combined;

            if (IsScaled)
            {
                var scaleFactor = OriginalScaleFactor;
                var scaleMatrix = SKMatrix.CreateScale(scaleFactor, scaleFactor);

                combined = SKMatrix.Concat(scaleMatrix, native);
            }
            else
            {
                combined = native;
            }

            if (InitialMatrix is not null)
            {
                combined = SKMatrix.Concat(InitialMatrix.Value, combined);
            }

            canvas.SetMatrix(combined);
        }
    }
}