using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the smoothing mode for rendering.
    /// </summary>
    public enum SmoothingMode
    {
        /// <summary>
        /// Specifies an invalid smoothing mode.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// Specifies no antialiasing smoothing mode.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Specifies no antialiasing smoothing mode.
        /// </summary>
        HighSpeed = 1,

        /// <summary>
        /// Specifies antialiased smoothing mode.
        /// </summary>
        HighQuality = 2,

        /// <summary>
        /// Specifies no antialiasing smoothing mode.
        /// </summary>
        None = 3,

        /// <summary>
        /// Specifies antialiased smoothing mode.
        /// </summary>
        AntiAlias = 4
    }
}
