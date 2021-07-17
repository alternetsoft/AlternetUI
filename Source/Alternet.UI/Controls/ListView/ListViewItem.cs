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
        /// Initializes a new instance of the <see cref="ListViewItem"/> class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">The zero-based index of the image within the <see cref="ImageList"/> associated with the <see cref="ListView"/> that contains the item.</param>
        public ListViewItem(string text, int imageIndex)
        {
            Text = text;
            ImageIndex = imageIndex;
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
        /// Initializes a new instance of the <see cref="ListViewItem"/> class with an array of strings representing column cells and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="cells">An array of strings that represent the column cells of the new item.</param>
        /// <param name="imageIndex">The zero-based index of the image within the <see cref="ImageList"/> associated with the <see cref="ListView"/> that contains the item.</param>
        public ListViewItem(string[] cells, int imageIndex) : this(cells)
        {
            ImageIndex = imageIndex;
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
        /// Gets or sets the index of the image that is displayed for the item.
        /// </summary>
        /// <value>The zero-based index of the image in the <see cref="ImageList"/> that is displayed for the item. The default is <c>null</c>.</value>
        /// <remarks>
        /// The value of this property depends on the value of the <see cref="ListView.SmallImageList"/> and <see cref="ListView.LargeImageList"/> properties.
        /// Depending on the current value of the View property of the <see cref="ListView"/> control associated
        /// with the item, the <see cref="ImageList"/> used by the item could be one specified in the <see cref="ListView.LargeImageList"/>
        /// property or the <see cref="ListView.SmallImageList"/> property of the <see cref="ListView"/> control. If the <see cref="ListView.View"/> property is set
        /// to <see cref="ListViewView.LargeIcon"/>, the <see cref="ImageList"/> specified in the <see cref="ListView.LargeImageList"/> property is used;
        /// otherwise, the <see cref="ImageList"/> specified in the <see cref="ListView.SmallImageList"/> property is used. The images defined in the <see cref="ImageList"/>
        /// specified in the <see cref="ListView.SmallImageList"/> property should have the same index positions as the images in
        /// the <see cref="ImageList"/> specified in the <see cref="ListView.LargeImageList"/> property. If the index positions are the same for both
        /// <see cref="ImageList"/> instances, you can set a single index value for the <see cref="ImageIndex"/> property and the appropriate
        /// image will be displayed regardless of the value of the <see cref="ListView.View"/> property of the <see cref="ListView"/> control.
        /// </remarks>
        public int? ImageIndex { get; set; }

        /// <summary>
        /// Gets a collection containing all column cells of the item.
        /// </summary>
        /// <remarks>Using the <see cref="Cells"/> property, you can add column cells,
        /// remove column cells, and obtain a count of column cells.</remarks>
        public Collection<ListViewItemCell> Cells { get; } = new Collection<ListViewItemCell> { ThrowOnNullItemAddition = true };
    }
}