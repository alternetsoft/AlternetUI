using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the unit of measurement for coordinates.
    /// </summary>
    public enum CoordUnit
    {
        /// <summary>
        /// The coordinate is measured in physical pixels.
        /// </summary>
        Pixel,

        /// <summary>
        /// The coordinate is measured in device-independent pixels.
        /// </summary>
        Dip,

        /// <summary>
        /// The coordinate is specified as a percentage.
        /// </summary>
        Percent,
    }
}
