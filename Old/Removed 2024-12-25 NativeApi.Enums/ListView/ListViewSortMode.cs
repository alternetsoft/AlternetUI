using ApiCommon;

namespace NativeApi.Api
{
    /// <summary>
    /// Specifies the sorting behavior of a list view.
    /// </summary>
    /// <remarks>This enumeration is used by the <see cref="ListView"/> class.</remarks>
    [ManagedExternName("Alternet.UI.ListViewSortMode")]
    [ManagedName("Alternet.UI.ListViewSortMode")]
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
        /// The items are sorted using the <see cref="ListView.CustomItemSortComparer"/> property.
        /// </summary>
        Custom
    }
}