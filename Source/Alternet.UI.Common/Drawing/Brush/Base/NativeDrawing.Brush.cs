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
        /// Creates transparent brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract IBrushHandler CreateTransparentBrushHandler();

        /// <summary>
        /// Creates hatch brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateHatchBrushHandler();

        /// <summary>
        /// Creates linear gradient brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract ILinearGradientBrushHandler CreateLinearGradientBrushHandler();

        /// <summary>
        /// Creates radial gradient brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract IRadialGradientBrushHandler CreateRadialGradientBrushHandler();

        /// <summary>
        /// Creates solid brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract ISolidBrushHandler CreateSolidBrushHandler();

        /// <summary>
        /// Creates texture brush handler.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTextureBrushHandler();

        /// <summary>
        /// Updates native hatch brush properties.
        /// </summary>
        /// <returns></returns>
        public abstract void UpdateHatchBrush(HatchBrush brush);
    }
}
