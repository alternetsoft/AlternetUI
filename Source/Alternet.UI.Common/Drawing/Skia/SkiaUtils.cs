using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.UI.Extensions;

namespace Alternet.Drawing
{
    public static class SkiaUtils
    {
        public static SKColor NullColorSkia = new();

        public static SKPaint ToSkia(this Pen pen)
        {
            SKPaint result = new();
            result.Color = pen.Color.ToSkColor();
            result.StrokeCap = pen.LineCap.ToSkia();
            result.StrokeJoin = pen.LineJoin.ToSkia();
            result.StrokeWidth = (float)pen.Width;
            result.IsStroke = true;
            return result;
        }

        public static SKPaint ToSkia(this Brush brush)
        {
            SKPaint result = new();
            result.Color = brush.BrushColor.ToSkColor();
            result.Style = SKPaintStyle.Fill;
            return result;
        }

        public static SKFont ToSkFont(this Alternet.Drawing.Font font)
        {
            var skiaFont = font.SkiaFont;
            if (skiaFont is not null)
                return skiaFont;

            SKFontStyleWeight skiaWeight = (SKFontStyleWeight)font.Weight;
            SKFontStyleSlant skiaSlant = font.Style.HasFlag(FontStyle.Italic) ?
                SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

            var typeFace = SKTypeface.FromFamilyName(
                font.Name,
                skiaWeight,
                SKFontStyleWidth.Normal,
                skiaSlant);

            skiaFont = new(typeFace, (float)font.SizeInPixels);

            font.SkiaFont = skiaFont;

            return skiaFont;

            /*using (SKPaint paint = new SKPaint())
            {
                paint.Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                paint.TextSize = 10;*/
        }

        public static SKColor ToSkColor(this Color color)
        {
            if (color is null || !color.IsOk)
                return NullColorSkia;
            if (color.SkiaColor is not null)
                return color.SkiaColor.Value;

            color.GetArgbValues(out var a, out var r, out var g, out var b);
            var skColor = new SKColor(r, g, b, a);
            color.SkiaColor = skColor;
            return skColor;
        }
    }
}
