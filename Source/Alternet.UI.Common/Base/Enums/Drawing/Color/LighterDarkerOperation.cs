using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the available operations for adjusting a color's brightness, such as lightening or darkening.
    /// </summary>
    /// <remarks>This enumeration provides options for modifying color RGB values. Each value represents a
    /// distinct adjustment operation that can be used to alter the visual appearance of colors, enabling effects like
    /// subtle or significant lightening and darkening. These operations are commonly applied in UI rendering, theming,
    /// or image processing scenarios.</remarks>
    public enum ColorAdjustmentOperation
    {
        /// <summary>
        /// Represents a value indicating that no specific color adjustment operation is selected.
        /// </summary>
        None,
        
        /// <summary>
        /// Represents a value indicating that the color should be lightened.
        /// </summary>
        Lighten,

        /// <summary>
        /// Represents a value indicating that the color should be lightened significantly.
        /// </summary>
        LightenHigh,

        /// <summary>
        /// Represents a value indicating that the color should be darkened.
        /// </summary>
        Darken,

        /// <summary>
        /// Represents a value indicating that the color should be darkened significantly.
        /// </summary>
        DarkenHigh,
    }
}
