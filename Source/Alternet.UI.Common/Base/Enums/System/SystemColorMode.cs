using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known system color modes.
    /// </summary>
    public enum SystemColorMode
    {
        /// <summary>
        /// Dark mode is or should be disabled.
        /// </summary>
        Classic = 0,

        /// <summary>
        /// The setting for the current system color mode is inherited from the operating system.
        /// </summary>
        System = 1,

        /// <summary>
        /// Dark mode is enabled.
        /// </summary>
        Dark = 2,
    }
}
