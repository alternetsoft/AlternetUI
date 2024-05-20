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
        public static PointD ToAlternet(this SKPoint value)
        {
            return new(value.X, value.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFont ToMaui(this Alternet.Drawing.Font font)
        {
            return (IFont)font.Handler;
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
