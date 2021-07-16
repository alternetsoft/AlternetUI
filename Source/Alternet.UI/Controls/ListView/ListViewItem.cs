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
        /// Initializes a new instance of the <see cref="ListViewItem"/> class with an array of strings representing column cells.
        /// </summary>
        /// <param name="cells">An array of strings that represent the column cells of the new item.</param>
        public ListViewItem(string[] cells)
        {
            foreach (var cell in cells)
                Cells.Add(new ListViewItemCell(cell));
        }

        /// <summary>
        /// Gets or sets the text of the item.
        /// </summary>
        public string Text
        {
            get
            {
                if (Cells.Count == 0)
                    Cells.Add(new ListViewItemCell());
                return Cells[0].Text;
            }

            set
            {
                if (Cells.Count == 0)
                    Cells.Add(new ListViewItemCell(Text));
                
                Cells[0].Text = value;
            }
        }

        /// <summary>
        /// Gets a collection containing all column cells of the item.
        /// </summary>
        /// <remarks>Using the <see cref="Cells"/> property, you can add column cells,
        /// remove column cells, and obtain a count of column cells.</remarks>
        public Collection<ListViewItemCell> Cells { get; } = new Collection<ListViewItemCell> { ThrowOnNullItemAddition = true };
    }
}