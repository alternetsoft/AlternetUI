using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        /// <inheritdoc/>
        public override void UpdateHatchBrush(HatchBrush brush)
        {
            ((UI.Native.HatchBrush)brush.Handler).Initialize(
                (UI.Native.BrushHatchStyle)brush.HatchStyle,
                brush.Color);
        }

        public override void UpdateTextureBrush(TextureBrush brush)
        {
            ((UI.Native.TextureBrush)brush.Handler).Initialize(
                (UI.Native.Image)brush.Image.NativeObject);
        }

        /// <inheritdoc/>
        public override IBrushHandler CreateTransparentBrushHandler() => new UI.Native.Brush();

        /// <inheritdoc/>
        public override object CreateHatchBrushHandler() => new UI.Native.HatchBrush();

        /// <inheritdoc/>
        public override ILinearGradientBrushHandler CreateLinearGradientBrushHandler()
            => new UI.Native.LinearGradientBrush();

        /// <inheritdoc/>
        public override IRadialGradientBrushHandler CreateRadialGradientBrushHandler()
            => new UI.Native.RadialGradientBrush();

        /// <inheritdoc/>
        public override ISolidBrushHandler CreateSolidBrushHandler() => new UI.Native.SolidBrush();

        /// <inheritdoc/>
        public override object CreateTextureBrushHandler() => new UI.Native.TextureBrush();
    }
}
