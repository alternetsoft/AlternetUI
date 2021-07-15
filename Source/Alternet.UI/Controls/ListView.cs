
namespace Alternet.UI
{
    /// <summary>
    /// Represents a list view control, which displays a collection of items that can be displayed using one of several different views.
    /// </summary>
    public class ListView : Control
    {
        /// <summary>
        /// Gets a collection containing all items in the control.
        /// </summary>
        public Collection<ListViewItem> Items { get; } = new Collection<ListViewItem> { ThrowOnNullItemAddition = true };
    }
}