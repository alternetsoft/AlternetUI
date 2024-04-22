using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI.Maui
{
    public class MauiDrawing : NativeDrawing
    {
        private static bool initialized;

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
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateTransparentBrush()
        {
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateSolidBrush()
        {
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateHatchBrush()
        {
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateLinearGradientBrush()
        {
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateRadialGradientBrush()
        {
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateTextureBrush()
        {
            return NotImplemented();
        }

        /// <inheritdoc/>
        public override void UpdatePen(Pen pen)
        {
            NotImplemented();
        }

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

        /// <inheritdoc/>
        public override Color GetColor(SystemSettingsColor index)
        {
            return NotImplemented<Color>();
        }
    }
}
