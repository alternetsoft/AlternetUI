using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing
    {
        /// <inheritdoc/>
        public override void UpdateTextureBrush(TextureBrush brush)
        {
            throw new NotImplementedException();
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
    }
}
