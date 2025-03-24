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
        /// Specifies that a window will SET its width to fit the width of its content,
        /// but not the height.
        /// </summary>
        Width,

        /// <summary>
        /// Specifies that a window will SET its height to fit the height of its content,
        /// but not the width.
        /// </summary>
        Height,

        /// <summary>
        /// Specifies that a window will SET both its width and height to fit the width
        /// and height of its content.
        /// </summary>
        WidthAndHeight,

        /// <summary>
        /// Specifies that a window will GROW its width to fit the width of its content,
        /// but not the height.
        /// </summary>
        GrowWidth,

        /// <summary>
        /// Specifies that a window will GROW its height to fit the height of its content,
        /// but not the width.
        /// </summary>
        GrowHeight,

        /// <summary>
        /// Specifies that a window will GROW both its width and height to fit the width
        /// and height of its content.
        /// </summary>
        GrowWidthAndHeight,
    }
}