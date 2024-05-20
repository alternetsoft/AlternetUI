using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.Drawing
{
    public static class MauiColorUtils
    {
        public static SKColor NullColorSkia = new();

        public static Microsoft.Maui.Graphics.Color NullColorMaui = new();

        public static Microsoft.Maui.Graphics.Color ToMaui(this Color color)
        {
            if (color is null || !color.IsOk)
                return NullColorMaui;
            if (color.NativeObject is Microsoft.Maui.Graphics.Color savedColor)
                return savedColor;

            color.GetArgbValues(out var a, out var r, out var g, out var b);
            var result = new Microsoft.Maui.Graphics.Color(r, g, b, a);
            color.NativeObject = result;
            return result;
        }

        public static SKColor ToSkia(this Color color)
        {
            if (color is null || !color.IsOk)
                return NullColorSkia;
            if (color.NativeObject is SKColor savedColor)
                return savedColor;

            color.GetArgbValues(out var a, out var r, out var g, out var b);
            var skColor = new SKColor(r, g, b, a);
            color.NativeObject = skColor;
            return skColor;
        }
    }
}
