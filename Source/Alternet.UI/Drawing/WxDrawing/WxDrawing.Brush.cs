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
            ((UI.Native.HatchBrush)brush.NativeObject).Initialize(
                (UI.Native.BrushHatchStyle)brush.HatchStyle,
                brush.Color);
        }

        /// <inheritdoc/>
        public override void UpdateLinearGradientBrush(LinearGradientBrush brush)
        {
            ((UI.Native.LinearGradientBrush)brush.NativeObject).Initialize(
                brush.StartPoint,
                brush.EndPoint,
                brush.GradientStops.Select(x => x.Color).ToArray(),
                brush.GradientStops.Select(x => x.Offset).ToArray());
        }

        /// <inheritdoc/>
        public override void UpdateRadialGradientBrush(RadialGradientBrush brush)
        {
            ((UI.Native.RadialGradientBrush)brush.NativeObject).Initialize(
                brush.Center,
                brush.Radius,
                brush.GradientOrigin,
                brush.GradientStops.Select(x => x.Color).ToArray(),
                brush.GradientStops.Select(x => x.Offset).ToArray());
        }

        /// <inheritdoc/>
        public override void UpdateSolidBrush(SolidBrush brush)
        {
            ((UI.Native.SolidBrush)brush.NativeObject).Initialize(brush.Color);
        }

        public override void UpdateTextureBrush(TextureBrush brush)
        {
            ((UI.Native.TextureBrush)brush.NativeObject).Initialize(
                (UI.Native.Image)brush.Image.NativeObject);
        }

        /// <inheritdoc/>
        public override object CreateTransparentBrush() => new UI.Native.Brush();

        /// <inheritdoc/>
        public override object CreateHatchBrush() => new UI.Native.HatchBrush();

        /// <inheritdoc/>
        public override object CreateLinearGradientBrush() => new UI.Native.LinearGradientBrush();

        /// <inheritdoc/>
        public override object CreateRadialGradientBrush() => new UI.Native.RadialGradientBrush();

        /// <inheritdoc/>
        public override object CreateSolidBrush() => new UI.Native.SolidBrush();

        /// <inheritdoc/>
        public override object CreateTextureBrush() => new UI.Native.TextureBrush();
    }
}
