using System;
using System.Collections.Generic;
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
        // Used in editor
        public override SizeD GetTextExtent(string text, Font font)
        {
            return SizeD.Empty;
        }

        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin)
        {
        }

        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void SetPixel(double x, double y, Color color)
        {
            using SKPaint paint = new();
            paint.Color = color.ToSkia();
            canvas.DrawPoint((float)x, (float)y, paint);
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            var locationX = (float)location.X;
            var locationY = (float)location.Y;

            var skiaFont = font.ToSkia();

            using SKPaint paint = new(skiaFont);
            paint.Color = foreColor.ToSkia();
            paint.Style = SKPaintStyle.Fill;
            paint.TextAlign = SKTextAlign.Left;

            SKRect textBounds = default;
            paint.MeasureText(text, ref textBounds);
            textBounds.Offset(locationX, locationY);

            if (backColor.IsOk)
            {
                var skiaBackColor = backColor.ToSkia();
                using SKPaint fillPaint = new();
                fillPaint.Color = skiaBackColor;
                fillPaint.Style = SKPaintStyle.Fill;

                canvas.DrawRect(textBounds, fillPaint);
            }

            canvas.DrawText(text, locationX, locationY, paint);

            if (font.Style.HasFlag(FontStyle.Underline))
            {
            }

            if (font.Style.HasFlag(FontStyle.Strikeout))
            {
                float y = textBounds.Top + (textBounds.Height / 2);
                SKPoint point1 = new(textBounds.Left, y);
                SKPoint point2 = new(textBounds.Left + textBounds.Width, y);

                var thickness = paint.FontMetrics.StrikeoutThickness;
                thickness ??= Math.Min(textBounds.Height / 5, 2);
                paint.StrokeWidth = thickness.Value;
                canvas.DrawLine(point1, point2, paint);
            }
        }

        /// <inheritdoc/>
        // Used in editor
        public override void PushTransform(TransformMatrix transform)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void Pop()
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
        }

        /// <inheritdoc/>
        // Used in editor
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
        }
    }
}
