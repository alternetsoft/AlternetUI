using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the initial location of a window.
    /// </summary>
    public enum WindowStartLocation
    {
        /// <summary>
        /// The window is positioned at the operating system default location.
        /// </summary>
        SystemDefault,

        /// <summary>
        /// The position of the window is determined by the <see cref="Window.Location"/> property.
        /// </summary>
        Manual,

        /// <summary>
        /// The window is centered on the current display, and has the dimensions specified in the windows's size.
        /// </summary>
        CenterScreen,

        /// <summary>
        /// The startup location of a <see cref="Window"/> is the center of the <see cref="Window"/> that owns it.
        /// </summary>
        CenterOwner // todo: add window.owner, add xmldoc: "as specified by the <see cref="Window.Owner"/> property."
    }
}
