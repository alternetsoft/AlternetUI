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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override ISolidBrushHandler CreateSolidBrush()
        {
            return new PlessSolidBrushHandler();
        }

        /// <inheritdoc/>
        public override object CreateHatchBrush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateLinearGradientBrush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateRadialGradientBrush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateTextureBrush()
        {
            throw new NotImplementedException();
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
