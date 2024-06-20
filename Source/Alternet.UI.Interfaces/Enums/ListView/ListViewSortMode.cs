namespace Alternet.UI
{
    /// <summary>
    /// Specifies the sorting behavior of the control.
    /// </summary>
    public enum ListViewSortMode
    {
        /// <summary>
        /// No sorting
        /// </summary>
        None,

        /// <summary>
        /// The items are sorted in ascending order.
        /// </summary>
        Ascending,

        /// <summary>
        /// The items are sorted in descending order.
        /// </summary>
        Descending,

        /// <summary>
        /// The items are sorted using the custom sort method.
        /// </summary>
        Custom,
    }
}