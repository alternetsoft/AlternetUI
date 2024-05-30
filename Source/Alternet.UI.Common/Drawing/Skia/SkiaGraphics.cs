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
    public class SkiaGraphics : NotImplementedGraphics
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

        public static SKBitmap ToSkia(Image image)
        {
            return ((SkiaImageHandler)image.Handler).ToSkia();
        }

        public override SizeD GetTextExtent(
            string text,
            Font font,
            out Coord? descent,
            out Coord? externalLeading,
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

            var typeFace = skiaFont.Typeface;

            using SKPaint paint = new(skiaFont);
            SKRect textBounds = default;

            /*
            var count = typeFace.GlyphCount;

            int[] codepoints = new int[count];

            for (int i = 0; i < count; i++)
            {
                codepoints[i] = i;
            }

            ushort[] glyphs = typeFace.GetGlyphs(codepoints);

            ///GetGlyph(codepoint) != 0;
                var glyph = typeFace.GetGlyph(i);
            skiaFont.MeasureText()
            */

            paint.MeasureText(text, ref textBounds);
            var width = textBounds.Width;
            var height = Math.Max(textBounds.Height, 0);

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
        public override void SetPixel(Coord x, Coord y, Color color)
        {
            using SKPaint paint = new();
            paint.Color = color;
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
            paint.Color = foreColor;
            paint.Style = SKPaintStyle.Fill;
            paint.TextAlign = SKTextAlign.Left;

            SKRect textBounds = default;
            paint.MeasureText(text, ref textBounds);
            var width = textBounds.Width;
            var height = Math.Max(textBounds.Height, 0);

            /*Debug.WriteLine($"SizeInPoints: {font.SizeInPoints}, SizeInPixels: {font.SizeInPixels}, Height: {textBounds.Height}");*/

            SKRect textRect = SKRect.Create(width, height);
            textRect.Offset(locationX, locationY);

            if (backColor.IsOk)
            {
                using SKPaint fillPaint = new();
                fillPaint.Color = backColor;
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
            var skiaPoints = points.ToSkia();
            canvas.DrawPoints(SKPointMode.Polygon, skiaPoints, paint);
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            canvas.DrawRect(rectangle, paint);
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            canvas.DrawLine(a, b, paint);
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
            DebugPenAssert(pen);
            using var paint = pen.ToSkia();
            SKRoundRect roundRect = new(rect, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, paint);
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
            DebugBrushAssert(brush);
            using var paint = brush.ToSkia();
            SKRoundRect roundRect = new(rect, (float)cornerRadius);
            canvas.DrawRoundRect(roundRect, paint);
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            using var paint = brush.ToSkia();
            canvas.DrawRect(rectangle, paint);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
            DebugImageAssert(image);
            canvas.DrawBitmap(ToSkia(image), origin);
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            var skiaPoints = points.ToSkia();
            using var paint = pen.ToSkia();
            throw new NotImplementedException();
        }
    }
}