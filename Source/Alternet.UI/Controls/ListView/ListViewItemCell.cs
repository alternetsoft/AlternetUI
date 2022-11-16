using Alternet.Drawing;

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
        public ListViewItemCell(string text) : this (text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemCell"/> class with the specified item text and
        /// the image index position of the cell's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">The zero-based index of the image within the <see cref="ImageList"/> associated with the <see cref="ListView"/> that contains the item.</param>
        public ListViewItemCell(string text, int? imageIndex)
        {
            Text = text;
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets the text of the cell.
        /// </summary>
        /// <value>The text to display for the cell.</value>
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the cell.
        /// </summary>
        /// <value>The zero-based index of the image in the <see cref="ImageList"/> that is displayed for the cell. The default is <c>null</c>.</value>
        public int? ImageIndex { get; set; }

        /// <summary>
        /// Gets the index the column associated to this cell. If the value is null, the cell is not associated with any item or column.
        /// </summary>
        public int? ColumnIndex { get; internal set; }
    }
}