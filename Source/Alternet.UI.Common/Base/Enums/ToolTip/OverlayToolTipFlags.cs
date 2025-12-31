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
    public enum OverlayToolTipFlags : ulong
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

        /// <summary>
        /// Indicates that overlays should be cleared before displaying new content.
        /// </summary>
        /// <remarks>This flag is typically used to ensure that any existing overlays
        /// are removed prior
        /// to rendering new overlays, preventing overlap or visual clutter.</remarks>
        Clear = 1 << 2,

        /// <summary>
        /// Represents a combination of flags that clears the current state and
        /// dismisses the item after a specified interval.
        /// </summary>
        /// <remarks>This value is a bitwise combination of the <see cref="Clear"/> and
        /// <see cref="DismissAfterInterval"/> flags. It is typically used to perform both
        /// actions in a single operation.</remarks>
        ClearAndDismiss = Clear | DismissAfterInterval,

        /// <summary>
        /// Specifies that the tooltip should be positioned to fit into its container.
        /// </summary>
        FitIntoContainer = 1 << 3,
    }
}
