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

        /// <summary>
        /// Specifies a combination of rendering options that enable both SkiaSharp and OpenGL.
        /// </summary>
        /// <remarks>This value is a bitwise combination of the <see cref="UseSkiaSharp"/> and
        /// <see cref="UseOpenGL"/> flags. It is typically used to configure rendering
        /// pipelines that leverage SkiaSharp for
        /// 2D graphics and OpenGL for hardware-accelerated rendering.</remarks>
        UseSkiaSharpWithOpenGL = UseSkiaSharp | UseOpenGL,
    }
}
