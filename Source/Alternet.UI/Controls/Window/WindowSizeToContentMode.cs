namespace Alternet.UI
{
    /// <summary>
    /// Specifies how a window will size itself to fit the size of its content.
    /// </summary>
    public enum WindowSizeToContentMode
    {
        /// <summary>
        /// No size to content behavior is performed.
        /// </summary>
        None,

        /// <summary>
        /// Specifies that a window will set its width to fit the width of its content, but not the height.
        /// </summary>
        Width,

        /// <summary>
        /// Specifies that a window will set its height to fit the height of its content, but not the width.
        /// </summary>
        Height,

        /// <summary>
        /// Specifies that a window will set both its width and height to fit the width and height of its content.
        /// </summary>
        WidthAndHeight,
    }
}