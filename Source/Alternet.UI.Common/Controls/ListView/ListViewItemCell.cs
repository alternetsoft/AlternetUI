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
    public class ListViewItemCell : BaseControlItem
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
        public virtual string Text
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
        public virtual int? ImageIndex
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
        public virtual int? ColumnIndex
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
        public virtual ListViewItem? Item
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
        public virtual ListViewItemCell Clone()
        {
            var result = new ListViewItemCell();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns properties from another <see cref="ListViewItemCell"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public virtual void Assign(ListViewItemCell item)
        {
            var newText = item.Text;
            var newImageIndex = item.ImageIndex;

            bool applyText = text != newText;
            bool applyImage = imageIndex != newImageIndex;

            text = newText;
            imageIndex = newImageIndex;

            ApplyAll(applyText, applyImage);
        }

        private void ApplyAll(bool applyText = true, bool applyImage = true)
        {
            var item = Item;
            if (item is null)
                return;

            var listView = item.ListView;
            if (listView is null)
                return;

            var itemIndex = item.Index;
            if (itemIndex is null)
                return;

            if (ColumnIndex is null)
                return;

            var column = ColumnIndex.Value;
            var index = itemIndex.Value;

            if (applyText)
                listView.Handler.SetItemText(index, column, text);
            if (applyImage)
                listView.Handler.SetItemImageIndex(index, column, imageIndex);
        }

        private void ApplyText() => ApplyAll(true, false);

        private void ApplyImage() => ApplyAll(false, true);
    }
}