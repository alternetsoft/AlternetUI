namespace Alternet.UI
{
    /// <summary>
    /// Specifies the grid line display mode for a list view.
    /// </summary>
    public enum ListViewGridLinesDisplayMode
    {
        /// <summary>
        /// No grid lines are visible. Not implemented on some platforms.
        /// If not implemented, vertical lines are shown.
        /// </summary>
        None,

        /// <summary>
        /// Vertical grid lines are visible.
        /// </summary>
        Vertical,

        /// <summary>
        /// Horizontal grid lines are visible.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Both vertical and horizontal grid lines are visible.
        /// </summary>
        VerticalAndHorizontal,
    }
}