using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.UI
{
    public static class MauiUtils
    {
        public static PointD Convert(SKPoint value)
        {
            return new(value.X, value.Y);
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
