using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.Drawing
{
    public class SkiaGraphics : NotImplementedGraphics
    {
        private SKCanvas canvas;
        private SKPaintSurfaceEventArgs args;
        private SkiaContainer container;

        public SkiaGraphics(SkiaContainer container, SKPaintSurfaceEventArgs args)
        {
            this.args = args;
            canvas = args.Surface.Canvas;
            this.container = container;
        }

        public SkiaContainer Container
        {
            get => container;
            set => container = value;
        }

        public SKPaintSurfaceEventArgs Args
        {
            get => args;

            set
            {
                args = value;
                canvas = args.Surface.Canvas;
            }
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
                var result = canvas.TotalMatrix; // !!!

                throw new NotImplementedException();
            }

            set
            {
                SKMatrix matrix = SKMatrix.CreateIdentity(); // !!!
                canvas.SetMatrix(matrix);

                throw new NotImplementedException();
            }
        }

        public override SizeD GetTextExtent(
            string text,
            Font font,
            out double? descent,
            out double? externalLeading,
            IControl? control = null)
        {
            descent = null;
            externalLeading = null;
            return GetTextExtent(text, font, control);
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
            var skiaFont = font.ToSkFont();
            using SKPaint paint = new(skiaFont);
            SKRect textBounds = default;
            paint.MeasureText(text, ref textBounds);
            var width = textBounds.Width;
            var height = textBounds.Height;

            if (font.Style.HasFlag(FontStyle.Underline))
            {
            }

            return (width, height);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin)
        {
            DrawText(
                text,
                origin,
                font,
                brush.BrushColor,
                Color.Empty);
        }

        /// <inheritdoc/>
        public override void SetPixel(double x, double y, Color color)
        {
            using SKPaint paint = new();
            paint.Color = color.ToSkColor();
            canvas.DrawPoint((float)x, (float)y, paint);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            var locationX = (float)location.X;
            var locationY = (float)location.Y;

            var skiaFont = font.ToSkFont();

            using SKPaint paint = new(skiaFont);
            paint.Color = foreColor.ToSkColor();
            paint.Style = SKPaintStyle.Fill;
            paint.TextAlign = SKTextAlign.Left;

            SKRect textBounds = default;
            paint.MeasureText(text, ref textBounds);
            var width = textBounds.Width;
            var height = textBounds.Height;

            Debug.WriteLine($"SizeInPoints: {font.SizeInPoints}, SizeInPixels: {font.SizeInPixels}, Height: {textBounds.Height}");

            SKRect textRect = SKRect.Create(width, height);
            textRect.Offset(locationX, locationY);

            if (backColor.IsOk)
            {
                var skiaBackColor = backColor.ToSkColor();
                using SKPaint fillPaint = new();
                fillPaint.Color = skiaBackColor;
                fillPaint.Style = SKPaintStyle.Fill;

                canvas.DrawRect(textRect, fillPaint);
            }

            canvas.DrawText(text, locationX - textBounds.Left, locationY - textBounds.Top, paint);

            if (font.Style.HasFlag(FontStyle.Underline))
            {
                // !!!
            }

            if (font.Style.HasFlag(FontStyle.Strikeout))
            {
                float y = textRect.Top + (height / 2);
                SKPoint point1 = new(textRect.Left, y);
                SKPoint point2 = new(textRect.Left + width, y);

                var thickness = paint.FontMetrics.StrikeoutThickness;
                thickness ??= Math.Min(height / 5, 2);
                paint.StrokeWidth = thickness.Value;
                canvas.DrawLine(point1, point2, paint);
            }
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            var skiaPoints = Convert(points);
            canvas.DrawPoints(SKPointMode.Polygon, skiaPoints, paint);
        }

        public SKPoint[] Convert(PointD[] points)
        {
            var length = points.Length;
            SKPoint[] result = new SKPoint[length];
            for (int i = 0; i < length; i++)
                result[i] = points[i].ToSkia();
            return result;
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            canvas.DrawRect(rectangle.ToSkia(), paint);
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            canvas.DrawLine(a.ToSkia(), b.ToSkia(), paint);
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            var skiaRect = rect.ToSkia();
            SKRoundRect roundRect = new(skiaRect, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, paint);
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius)
        {
            DebugBrushAssert(brush);
            using var paint = brush.ToSkia();
            var skiaRect = rect.ToSkia();
            SKRoundRect roundRect = new(skiaRect, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, paint);
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            using var paint = brush.ToSkia();
            var skiaRect = rectangle.ToSkia();
            canvas.DrawRect(skiaRect, paint);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            var skiaPoints = Convert(points);
            using var paint = pen.ToSkia();
        }
    }
}