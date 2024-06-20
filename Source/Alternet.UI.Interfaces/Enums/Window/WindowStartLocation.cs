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
        Default,

        /// <summary>
        /// The position of the window is determined by 'Location' property of the window.
        /// </summary>
        Manual,

        /// <summary>
        /// The window is centered on the current display, and has the dimensions specified
        /// in the windows's size.
        /// </summary>
        CenterScreen,

        /// <summary>
        /// The startup location of a window is the center of the
        /// window that owns it,
        /// as specified by the 'Owner' property.
        /// </summary>
        CenterOwner,
    }
}
