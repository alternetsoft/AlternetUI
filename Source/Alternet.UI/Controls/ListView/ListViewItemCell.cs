namespace Alternet.UI
{
    /// <summary>
    /// Represents a column cell of a <see cref="ListViewItem"/>.
    /// </summary>
    public class ListViewItemCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemCell"/> class with default values.
        /// </summary>
        public ListViewItemCell()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemCell"/> class with the specified cell text.
        /// </summary>
        /// <param name="text">The text to display for the cell.</param>
        public ListViewItemCell(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets the text of the cell.
        /// </summary>
        public string Text { get; set; } = "";
    }
}