using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.UI.Extensions
{
    public static class MauiExtensionsPrivate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD ToAlternet(this SKRect value)
        {
            return new(value.Left, value.Top, value.Width, value.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD ToAlternet(this SKPoint value)
        {
            return new(value.X, value.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Maui.Graphics.IFont ToMaui(this Alternet.Drawing.Font font)
        {
            return (Microsoft.Maui.Graphics.IFont)font.Handler;
        }

        public static SKFont ToSkFont(this Alternet.Drawing.Font font)
        {
            var handler = (MauiFontHandler)font.Handler;
            if (handler.SkiaFont is not null)
                return handler.SkiaFont;

            SKFontStyleWeight skiaWeight = (SKFontStyleWeight)font.Weight;
            SKFontStyleSlant skiaSlant = font.Style.HasFlag(FontStyle.Italic) ?
                SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;

            var typeFace = SKTypeface.FromFamilyName(
                font.Name,
                skiaWeight,
                SKFontStyleWidth.Normal,
                skiaSlant);

            SKFont skiaFont = new(typeFace, (float)font.Size);

            handler.SkiaFont = skiaFont;

            return skiaFont;

            /*using (SKPaint paint = new SKPaint())
            {
                paint.Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                paint.TextSize = 10;*/
        }

        public static MouseButton ToAlternet(this SKMouseButton value)
        {
            switch (value)
            {
                case SKMouseButton.Unknown:
                default:
                    return MouseButton.Unknown;
                case SKMouseButton.Left:
                    return MouseButton.Left;
                case SKMouseButton.Middle:
                    return MouseButton.Middle;
                case SKMouseButton.Right:
                    return MouseButton.Right;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD ToRectD(this Microsoft.Maui.Graphics.RectF value)
        {
            return new(value.X, value.Y, value.Width, value.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKRect ToSkia(this RectD rect)
        {
            SKRect result = SKRect.Create((float)rect.Width, (float)rect.Height);
            result.Offset((float)rect.X, (float)rect.Y);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKPoint ToSkia(this PointD point)
        {
            return new SKPoint((float)point.X, (float)point.Y);
        }

        public static SKStrokeJoin ToSkia(this LineJoin value)
        {
            switch (value)
            {
                case LineJoin.Miter:
                default:
                    return SKStrokeJoin.Miter;
                case LineJoin.Bevel:
                    return SKStrokeJoin.Bevel;
                case LineJoin.Round:
                    return SKStrokeJoin.Round;
            }
        }

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

        public static SKStrokeCap ToSkia(this LineCap value)
        {
            switch (value)
            {
                case LineCap.Flat:
                default:
                    return SKStrokeCap.Butt;
                case LineCap.Square:
                    return SKStrokeCap.Square;
                case LineCap.Round:
                    return SKStrokeCap.Round;
            }
        }
    }
}

