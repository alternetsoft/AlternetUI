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
            out double descent,
            out double externalLeading,
            IControl? control = null)
        {
            descent = 0;
            externalLeading = 0;
            return SizeD.Empty;
        }

        public override SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control)
        {
            return SizeD.Empty;
        }

        public override void DrawText(string text, Font font, Brush brush, PointD origin)
        {
            DrawText(text, font, brush, origin, TextFormat.Default);
        }

        public override void DrawText(string text, PointD origin)
        {
            DrawText(text, Font.Default, Brush.Default, origin);
        }

        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin,
            TextFormat format)
        {
        }

        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DrawText(text, font, brush, bounds, TextFormat.Default);
        }

        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format)
        {
        }

        public override SizeD MeasureText(string text, Font font, double maximumWidth)
        {
            return SizeD.Empty;
        }

        public override SizeD MeasureText(
            string text,
            Font font,
            double maximumWidth,
            TextFormat format)
        {
            return SizeD.Empty;
        }

        /// <inheritdoc/>
        // Used in editor
        public override SizeD GetTextExtent(string text, Font font)
        {
            return SizeD.Empty;
        }

        /// <inheritdoc/>
        // Used in editor
        public override void SetPixel(double x, double y, Color color)
        {
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
