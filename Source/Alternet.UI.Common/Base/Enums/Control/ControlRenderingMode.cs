using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the rendering mode for the control.
    /// </summary>
    public enum ControlRenderingMode
    {
        /// <summary>
        /// Control is rendered using software mode with clipping.
        /// </summary>
        SoftwareClipped,

        /// <summary>
        /// Control is rendered using software mode with double buffering.
        /// </summary>
        SoftwareDoubleBuffered,

        /// <summary>
        /// Control is rendered using SkiaSharp.
        /// </summary>
        SkiaSharp,

        /// <summary>
        /// Control is rendered using SkiaSharp with OpenGL acceleration.
        /// </summary>
        SkiaSharpWithOpenGL,
    }
}
