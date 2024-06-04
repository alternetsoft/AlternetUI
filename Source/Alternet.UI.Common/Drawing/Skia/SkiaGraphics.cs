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
        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
            DebugImageAssert(image);
            canvas.DrawBitmap((SKBitmap)image, origin);
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            /*var skiaPoints = points.ToSkia();*/
            throw new NotImplementedException();
        }
    }
}