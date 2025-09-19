using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the graphics backend used for rendering.
    /// </summary>
    public enum GraphicsBackendType
    {
        /// <summary>
        /// Uses the SkiaSharp graphics backend.
        /// </summary>
        SkiaSharp,

        /// <summary>
        /// Uses the WxWidgets graphics backend.
        /// </summary>
        WxWidgets,

        /// <summary>
        /// Represents a null graphics backend which does not perform any rendering.
        /// </summary>
        Null,

        /// <summary>
        /// Other or unknown graphics backend.
        /// </summary>
        Other,
    }
}
