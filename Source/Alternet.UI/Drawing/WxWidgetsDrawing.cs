using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class WxWidgetsDrawing : NativeDrawing
    {
        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            NativeDrawing.Default = new WxWidgetsDrawing();
            initialized = true;
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
