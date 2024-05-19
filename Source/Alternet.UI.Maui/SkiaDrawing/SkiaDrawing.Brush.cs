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
        public override IBrushHandler CreateTransparentBrushHandler()
        {
            return new MauiTransparentBrushHandler();
        }

        /// <inheritdoc/>
        public override ISolidBrushHandler CreateSolidBrushHandler()
        {
            return new MauiSolidBrushHandler();
        }

        /// <inheritdoc/>
        public override object CreateHatchBrushHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override ILinearGradientBrushHandler CreateLinearGradientBrushHandler()
        {
            return new MauiLinearGradientBrushHandler();
        }

        /// <inheritdoc/>
        public override IRadialGradientBrushHandler CreateRadialGradientBrushHandler()
        {
            return new MauiRadialGradientBrushHandler();
        }

        /// <inheritdoc/>
        public override object CreateTextureBrushHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void UpdateHatchBrush(HatchBrush brush)
        {
            NotImplemented();
        }
    }
}
