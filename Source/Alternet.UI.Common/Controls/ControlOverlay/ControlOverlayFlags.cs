using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies flags that define the behavior or appearance of a control overlay.
    /// </summary>
    /// <remarks>This enumeration is marked with the <see cref="FlagsAttribute"/>,
    /// allowing  bitwise combination of its member values to represent
    /// multiple overlay options.</remarks>
    [Flags]
    public enum ControlOverlayFlags
    {
        /// <summary>
        /// Represents the absence of any value or state.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the overlay should be removed when the 'Escape' key is pressed.
        /// </summary>
        RemoveOnEscape = 1 << 0,

        /// <summary>
        /// Indicates that the overlay should be removed when the 'Enter' key is pressed.
        /// </summary>
        RemoveOnEnter = 1 << 1,
    }
}
