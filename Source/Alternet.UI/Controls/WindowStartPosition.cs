using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the initial position of a window.
    /// </summary>
    public enum WindowStartPosition
    {
        /// <summary>
        /// The window is positioned at the operating system default location.
        /// </summary>
        SystemDefaultLocation,

        /// <summary>
        /// The window is positioned at the operating system default location and has the bounds determined by OS default.
        /// </summary>
        SystemDefaultBounds,

        /// <summary>
        /// The position of the window is determined by the <see cref="Window.Location"/> property.
        /// </summary>
        Manual,

        /// <summary>
        /// The window is centered on the current display, and has the dimensions specified in the windows's size.
        /// </summary>
        CenterScreen,

        /// <summary>
        /// The startup location of a <see cref="Window"/> is the center of the <see cref="Window"/> that owns it, as specified by the <see cref="Window.Owner"/> property.
        /// </summary>
        CenterOwner // todo: add window.owner
    }
}