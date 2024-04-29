using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        public abstract void UpdateTextureBrush(TextureBrush brush);

        /// <summary>
        /// Creates native transparent brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTransparentBrush();

        /// <summary>
        /// Creates native hatch brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateHatchBrush();

        /// <summary>
        /// Creates native linear gradient brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateLinearGradientBrush();

        /// <summary>
        /// Creates native radial gradient brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateRadialGradientBrush();

        /// <summary>
        /// Creates native solid brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateSolidBrush();

        /// <summary>
        /// Creates native texture brush.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTextureBrush();

        /// <summary>
        /// Updates native solid brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateSolidBrush(SolidBrush brush);

        /// <summary>
        /// Updates native hatch brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateHatchBrush(HatchBrush brush);

        /// <summary>
        /// Updates native linear gradient brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateLinearGradientBrush(LinearGradientBrush brush);

        /// <summary>
        /// Updates native radial gradient brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateRadialGradientBrush(RadialGradientBrush brush);
    }
}
