using System;

namespace Alternet.UI
{
    /// <summary>
    /// TODO
    /// </summary>
    public enum GridUnitType
    {
        /// <summary>
        /// The value indicates that content should be calculated without constraints. 
        /// </summary>
        Auto = 0,
        /// <summary>
        /// The value is expressed as a pixel.
        /// </summary>
        Pixel,
        /// <summary>
        /// The value is expressed as a weighted proportion of available space.
        /// </summary>
        Star,
    }
}