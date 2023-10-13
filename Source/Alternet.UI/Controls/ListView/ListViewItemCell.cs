using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a column cell of a <see cref="ListViewItem"/>.
    /// </summary>
    /// <remarks>
    /// A <see cref="ListView"/> control displays a list of items that are defined by the
    /// <see cref="ListViewItem"/> class.
    /// Each <see cref="ListViewItem"/> can store cell objects that are defined by the
    /// <see cref="ListViewItemCell"/> class.
    /// Cells are displayed when the <see cref="ListView.View"/> property of the
    /// <see cref="ListView"/> control is set to <see cref="ListViewView.Details"/>.
    /// </remarks>
    public class ListViewItemCell
    {
        private string text = string.Empty;
        private int? imageIndex;
        private ListViewItem? item;
        private int? columnIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemCell"/> class with
        /// default values.
        /// </summary>
        public ListViewItemCell()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemCell"/> class with
        /// the specified item text and
        /// the image index position of the cell's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">The zero-based index of the image within
        /// the <see cref="ImageList"/> associated with the <see cref="ListView"/> that
        /// contains the item.</param>
        public ListViewItemCell(string text, int? imageIndex = null)
        {
            this.text = text;
            this.imageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets the text of the cell.
        /// </summary>
        /// <value>The text to display for the cell.</value>
        public string Text
        {
            get => text;
            set
            {
                if (text == value)
                    return;
                text = value;
                ApplyText();
            }
        }

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the cell.
        /// </summary>
        /// <value>The zero-based index of the image in the <see cref="ImageList"/> that
        /// is displayed for the cell. The default is <c>null</c>.</value>
        public int? ImageIndex
        {
            get => imageIndex;
            set
            {
                if (imageIndex == value)
                    return;
                imageIndex = value;
                ApplyImage();
            }
        }

        /// <summary>
        /// Gets the index the column associated to this cell. If the value is null, the
        /// cell is not associated with any column.
        /// </summary>
        [Browsable(false)]
        public int? ColumnIndex
        {
            get => columnIndex;

            internal set
            {
                columnIndex = value;
                ApplyAll();
            }
        }

        /// <summary>
        /// Gets the list view item associated to this cell. If the value is null, the
        /// cell is not associated with any item.
        /// </summary>
        [Browsable(false)]
        public ListViewItem? Item
        {
            get => item;

            internal set
            {
                item = value;
                ApplyAll();
            }
        }

        /// <inheritdoc cref="ListControlItem.ToString"/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(ListViewItemCell);
            else
                return Text;
        }

        /// <summary>
        /// Creates copy of this <see cref="ListViewItemCell"/>.
        /// </summary>
        public ListViewItemCell Clone()
        {
            var result = new ListViewItemCell();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns properties from another <see cref="ListViewItemCell"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public void Assign(ListViewItemCell item)
        {
            bool applyText = text != item.Text;
            bool applyImage = imageIndex != item.ImageIndex;

            text = item.Text;
            imageIndex = item.ImageIndex;

            if (applyText) ApplyText();
            if (applyImage) ApplyImage();
        }

        private void ApplyAll()
        {
            ApplyText();
            ApplyImage();
        }

        private void ApplyText()
        {
            if (TryGetItemIndex(out var listView, out var itemIndex, out var columnIndex))
                listView.Handler.SetItemText(itemIndex.Value, columnIndex.Value, text);
        }

        private void ApplyImage()
        {
            if (TryGetItemIndex(out var listView, out var itemIndex, out var columnIndex))
                listView.Handler.SetItemImageIndex(itemIndex.Value, columnIndex.Value, imageIndex);
        }

        private bool TryGetItemIndex(
            [NotNullWhen(true)] out ListView? listView,
            [NotNullWhen(true)] out long? itemIndex,
            [NotNullWhen(true)] out long? columnIndex)
        {
            var item = Item;
            listView = item?.ListView;
            itemIndex = item?.Index;
            columnIndex = this.columnIndex;

            return listView != null && itemIndex != null && columnIndex != null;
        }
    }
}