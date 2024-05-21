using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.UI
{
    public static class MauiUtils
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
        public static IFont ToMaui(this Alternet.Drawing.Font font)
        {
            return (IFont)font.Handler;
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

        public static MouseButton Convert(SKMouseButton value)
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
    }
}
