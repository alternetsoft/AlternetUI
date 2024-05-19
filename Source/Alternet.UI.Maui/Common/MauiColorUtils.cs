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
        public static SKColor NullColor = new();

        public static SKColor Convert(Color color)
        {
            if (color is null || !color.IsOk)
                return NullColor;

            if (color.NativeObject is not null)
                return (SKColor)color.NativeObject;

            color.GetArgbValues(out var a, out var r, out var g, out var b);
            var skColor = new SKColor(r, g, b, a);
            color.NativeObject = skColor;
            return skColor;
        }
    }
}
