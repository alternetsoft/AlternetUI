using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.UI.Maui
{
    public class MauiDrawing : NativeDrawing
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
            NativeDrawing.Default = new MauiDrawing();
            initialized = true;
        }

        /// <inheritdoc/>
        public override object CreatePen()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override object CreateTransparentBrush()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override object CreateSolidBrush()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override object CreateHatchBrush()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override object CreateLinearGradientBrush()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override object CreateRadialGradientBrush()
        {
            return new SKPaint();
        }

        /// <inheritdoc/>
        public override object CreateTextureBrush()
        {
            return new SKPaint();
        }

        /*
         var paint1 = new SKPaint {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = new SKColor(255, 0, 0),
                Style = SKPaintStyle.Fill
            };

            var paint2 = new SKPaint {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = new SKColor(0, 136, 0),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 3
            };

            var paint3 = new SKPaint {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = new SKColor(136, 136, 136),
                TextScaleX = 1.5f
            };

        */

        /// <inheritdoc/>
        public override void UpdateSolidBrush(SolidBrush brush)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void UpdatePen(Pen pen)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void UpdateHatchBrush(HatchBrush brush)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void UpdateLinearGradientBrush(LinearGradientBrush brush)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void UpdateRadialGradientBrush(RadialGradientBrush brush)
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