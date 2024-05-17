using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public class SkiaGraphics : NotImplementedGraphics
    {
        private SKCanvas canvas;

        public SkiaGraphics(SKCanvas canvas)
        {
            this.canvas = canvas;
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

        public override SizeD GetTextExtent(string text, Font font)
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

        public override SizeD MeasureText(string text, Font font)
        {
            return SizeD.Empty;
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

        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
        }
    }
}
