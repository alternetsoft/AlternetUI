namespace Alternet.UI
{
    /// <summary>
    /// Specifies the grid line display mode for a list view.
    /// </summary>
    public enum ListViewGridLinesDisplayMode
    {
        /// <summary>
        /// No grid lines visible. Not implemented on some platforms.
        /// If not implemented, vertical lines are shown.
        /// </summary>
        None,

        /// <summary>
        /// Vertical grid lines visible.
        /// </summary>
        Vertical,

        /// <summary>
        /// Horizontal grid lines visible.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical and horizontal grid lines visible.
        /// </summary>
        VerticalAndHorizontal,
    }
}