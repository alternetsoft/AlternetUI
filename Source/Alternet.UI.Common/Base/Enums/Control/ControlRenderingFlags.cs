using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies rendering options for a control.
    /// </summary>
    [Flags]
    public enum ControlRenderingFlags
    {
        /// <summary>
        /// No rendering options are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Use SkiaSharp for rendering.
        /// </summary>
        UseSkiaSharp = 1 << 0,

        /// <summary>
        /// Use OpenGL for rendering.
        /// </summary>
        UseOpenGL = 1 << 1,
    }
}
