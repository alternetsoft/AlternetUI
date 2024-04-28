using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing : NativeDrawing
    {
        public static SKColor NullColor = new();

        private static bool initialized;

        public static SKColor Convert(Color color)
        {
            if (color is null || !color.IsOk)
                return NullColor;

            if (color.SkiaColor is not null)
                return (SKColor)color.SkiaColor;

            color.GetArgbValues(out var a, out var r, out var g, out var b);
            var skColor = new SKColor(r, g, b, a);
            color.SkiaColor = skColor;
            return skColor;
        }

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new SkiaDrawing();
            initialized = true;
        }

        /// <inheritdoc/>
        public override object CreatePen()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override void UpdatePen(Pen pen)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override Color GetColor(SystemSettingsColor index)
        {
            return NotImplemented<Color>();
        }
   }
}