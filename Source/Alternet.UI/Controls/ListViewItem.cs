namespace Alternet.UI
{
    /// <summary>
    /// Represents an item in a <see cref="ListView"/> control.
    /// </summary>
    public class ListViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/> class with default values.
        /// </summary>
        public ListViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/> class with the specified item text.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        public ListViewItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets the text of the item.
        /// </summary>
        public string Text { get; set; } = "";
    }
}