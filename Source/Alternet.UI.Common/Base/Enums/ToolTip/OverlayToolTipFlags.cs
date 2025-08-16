using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a set of flags that specify options or behaviors for the tooltip overlay.
    /// </summary>
    /// <remarks>This enumeration supports bitwise combination of its member
    /// values using the bitwise OR operator. Use the <see cref="None"/> value
    /// to represent the absence of any options.</remarks>
    [Flags]
    public enum OverlayToolTipFlags
    {
        /// <summary>
        /// Represents the absence of any specific value or option.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the overlay should be automatically dismissed
        /// after a predefined time interval.
        /// </summary>
        DismissAfterInterval = 1 << 0,

        /// <summary>
        /// Specifies that the system colors should be used for rendering the overlay.
        /// </summary>
        UseSystemColors = 1 << 1,
    }
}
