using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines possible gray scale methods.
    /// </summary>
    public enum ImageGrayScaleMethod
    {
        /// <summary>
        /// Uses default gray scale method.
        /// </summary>
        Default,

        /// <summary>
        /// Sets RGB of the each pixel to 150.
        /// </summary>
        SetColorRGB150,

        /// <summary>
        /// Fills image with <see cref="Image.DisabledBrush"/>.
        /// </summary>
        FillWithDisabledBrush,
    }
}
