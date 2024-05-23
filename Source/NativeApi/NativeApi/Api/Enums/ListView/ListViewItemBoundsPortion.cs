using ApiCommon;

namespace NativeApi.Api
{
    /// <summary>
    /// Specifies a portion of the list view item from which to retrieve the bounding rectangle.
    /// </summary>
    [ManagedExternName("Alternet.UI.ListViewItemBoundsPortion")]
    [ManagedName("Alternet.UI.ListViewItemBoundsPortion")]
    public enum ListViewItemBoundsPortion
    {
        /// <summary>
        /// The bounding rectangle of the entire item, including the icon, the item text should be retrieved.
        /// </summary>
        EntireItem,

        /// <summary>
        /// The bounding rectangle of the icon or small icon should be retrieved.
        /// </summary>
        Icon,

        /// <summary>
        /// The bounding rectangle of the item text should be retrieved.
        /// </summary>
        Label
    }
}