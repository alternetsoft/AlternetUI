namespace Alternet.UI
{
    /// <summary>
    /// Represents a column cell of a <see cref="ListViewItem"/>.
    /// </summary>
    /// <remarks>
    /// A <see cref="ListView"/> control displays a list of items that are defined by the <see cref="ListViewItem"/> class.
    /// Each <see cref="ListViewItem"/> can store cell objects that are defined by the <see cref="ListViewItemCell"/> class.
    /// Cells are displayed when the <see cref="ListView.View"/> property of the <see cref="ListView"/> control is set to <see cref="ListViewView.Details"/>.
    /// </remarks>
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
        /// <value>The text to display for the cell.</value>
        public string Text { get; set; } = "";
    }
}