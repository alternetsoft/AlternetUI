using System;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item in a <see cref="ListView"/> control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ListViewItem"/> class defines the appearance, behavior,
    /// and data associated with an item
    /// that is displayed in the <see cref="ListView"/> control.
    /// <see cref="ListViewItem"/> objects can be displayed in the
    /// <see cref="ListView"/> control in one of several different views
    /// defined by <see cref="ListViewView"/> enumeration.
    /// </para>
    /// <para>
    /// Most of the properties of the <see cref="ListViewItem"/> class provide
    /// ways to change the display of the item
    /// in the <see cref="ListView"/> control it is associated with. The
    /// <see cref="Text"/> property allows you to specify the text displayed
    /// in the item.
    /// </para>
    /// <para>
    /// The <see cref="ImageIndex"/> property allows you to specify the image
    /// to load from the <see cref="ImageList"/> that is assigned
    /// to the <see cref="ListView"/> control (by setting the
    /// <see cref="ListView.LargeImageList"/> or
    /// <see cref="ListView.SmallImageList"/> properties of the
    /// <see cref="ListView"/>).
    /// </para>
    /// <para>
    /// Items can display any number of cells when the
    /// <see cref="ListView.View"/> property of the associated
    /// <see cref="ListView"/> control
    /// is set to <see cref="ListViewView.Details"/> and columns are
    /// defined by the <see cref="ListView.Columns"/> property
    /// of the <see cref="ListView"/> control. You can add cells to an
    /// item by using <see cref="Cells"/> property of the <see cref="ListViewItem"/>.
    /// </para>
    /// </remarks>
    public class ListViewItem : BaseControlItem
    {
        private BaseCollection<ListViewItemCell>? cells;
        private ListView? listView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/>
        /// class with default values.
        /// </summary>
        public ListViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/> class
        /// with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">The zero-based index of the image within
        /// the <see cref="ImageList"/> associated with the <see cref="ListView"/>
        /// that contains the item. Optional.</param>
        public ListViewItem(string text, int? imageIndex = null)
            : this()
        {
            Text = text;
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/> class
        /// with an array of strings representing column cells and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="cellText">An array of strings that represent the column
        /// cells of the new item.</param>
        /// <param name="imageIndex">The zero-based index of the image within
        /// the <see cref="ImageList"/> associated with the <see cref="ListView"/> that
        /// contains the item.</param>
        public ListViewItem(string[] cellText, int? imageIndex = null)
            : this()
        {
            foreach (var cell in cellText)
                Cells.Add(new ListViewItemCell(cell));

            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets the owner <see cref="ListView"/> that the tree item is assigned to.
        /// </summary>
        /// <value>
        /// A <see cref="ListView"/> that represents the parent list view that the
        /// item is assigned to,
        /// or <c>null</c> if the item has not been assigned to a tree view.
        /// </value>
        [Browsable(false)]
        public virtual ListView? ListView
        {
            get => listView;
            internal set
            {
                listView = value;
                ApplyColumns();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the item is in the selected state.
        /// </summary>
        /// <value><see langword="true"/> if the item is in the selected
        /// state; otherwise, <see langword="false"/>.</value>
        [Browsable(false)]
        public virtual bool IsSelected
            => RequiredListView.SelectedIndices.Contains(RequiredIndex);

        /// <summary>
        /// Gets or sets a value indicating whether the item has focus within
        /// the <see cref="ListView"/> control.
        /// </summary>
        /// <value><see langword="true"/> if the item has focus; otherwise,
        /// <see langword="false"/>.</value>
        [Browsable(false)]
        public virtual bool IsFocused
        {
            get => RequiredListView.Handler.FocusedItemIndex == RequiredIndex;
            set => RequiredListView.Handler.FocusedItemIndex = RequiredIndex;
        }

        /// <summary>
        /// Gets the zero-based index of the item within the
        /// <see cref="ListView"/> control,
        /// or <see langword="null"/> if the item is not associated with a
        /// <see cref="ListView"/> control.
        /// </summary>
        [Browsable(false)]
        public virtual long? Index { get; internal set; }

        /// <summary>
        /// Gets or sets the text of the item.
        /// </summary>
        /// <value>The text to display for the item.</value>
        /// <remarks>
        /// When the <see cref="ListView.View"/> property of the associated
        /// <see cref="ListView"/> control
        /// is set to <see cref="ListViewView.Details"/>, this property refers
        /// to the first cell text in the <see cref="Cells"/> collection.
        /// In other views, the property specifies the text displayed together
        /// with an optional image to form a visual representation of the item.
        /// </remarks>
        public virtual string Text
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
                {
                    Cells.Add(new ListViewItemCell(value, null));
                    return;
                }

                Cells[0].Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the item.
        /// </summary>
        /// <value>The zero-based index of the image in the
        /// <see cref="ImageList"/> that is displayed for the item.
        /// The default is <c>null</c>.</value>
        /// <remarks>
        /// The effect of setting this property depends on the value of the
        /// <see cref="ListView.SmallImageList"/> and
        /// <see cref="ListView.LargeImageList"/> properties.
        /// Depending on the current value of the View property of the
        /// <see cref="ListView"/> control associated
        /// with the item, the <see cref="ImageList"/> used by the item could
        /// be one specified in the <see cref="ListView.LargeImageList"/>
        /// property or the <see cref="ListView.SmallImageList"/> property of the
        /// <see cref="ListView"/> control. If the <see cref="ListView.View"/>
        /// property is set
        /// to <see cref="ListViewView.LargeIcon"/>, the <see cref="ImageList"/>
        /// specified in the <see cref="ListView.LargeImageList"/> property is used;
        /// otherwise, the <see cref="ImageList"/> specified in the
        /// <see cref="ListView.SmallImageList"/> property is used. The images
        /// defined in the <see cref="ImageList"/>
        /// specified in the <see cref="ListView.SmallImageList"/> property should
        /// have the same index positions as the images in
        /// the <see cref="ImageList"/> specified in the
        /// <see cref="ListView.LargeImageList"/> property. If the index positions are
        /// the same for both
        /// <see cref="ImageList"/> instances, you can set a single index value
        /// for the <see cref="ImageIndex"/> property and the appropriate
        /// image will be displayed regardless of the value of the
        /// <see cref="ListView.View"/> property of the <see cref="ListView"/> control.
        /// </remarks>
        public virtual int? ImageIndex
        {
            get
            {
                if (Cells.Count == 0)
                    Cells.Add(new ListViewItemCell());
                return Cells[0].ImageIndex;
            }

            set
            {
                if (Cells.Count == 0)
                {
                    Cells.Add(new ListViewItemCell(string.Empty, value));
                    return;
                }

                Cells[0].ImageIndex = value;
            }
        }

        /// <summary>
        /// Gets a collection containing all column cells of the item.
        /// </summary>
        /// <remarks>Using the <see cref="Cells"/> property, you can add column cells,
        /// remove column cells, and obtain a count of column cells.</remarks>
        public BaseCollection<ListViewItemCell> Cells
        {
            get
            {
                if(cells is null)
                {
                    cells = new(CollectionSecurityFlags.NoNullOrReplace);
                    cells.ItemInserted += OnCellInserted;
                    cells.ItemRemoved += OnCellRemoved;
                }

                return cells;
            }
        }

        private long RequiredIndex
        {
            get
            {
                var index = Index;
                return index ?? throw new InvalidOperationException(
                    string.Format(ErrorMessages.Default.PropertyCannotBeNull, nameof(Index)));
            }
        }

        private ListView RequiredListView =>
            ListView ?? throw new InvalidOperationException(
                string.Format(ErrorMessages.Default.PropertyCannotBeNull, nameof(ListView)));

        /// <summary>
        /// Initiates the editing of the list view item label.
        /// </summary>
        public virtual void BeginLabelEdit() =>
            ListView?.Handler.BeginLabelEdit(RequiredIndex);

        /// <summary>
        /// Ensures that the item is visible within the control, scrolling the
        /// contents of the control, if necessary.
        /// </summary>
        public virtual void EnsureVisible() =>
            ListView?.Handler.EnsureItemVisible(RequiredIndex);

        /// <inheritdoc cref="ListControlItem.ToString"/>
        public override string? ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(ListViewItem);
            else
                return Text;
        }

        /// <summary>
        /// Creates copy of this <see cref="ListViewItem"/>.
        /// </summary>
        public virtual ListViewItem Clone()
        {
            var result = new ListViewItem();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns properties from another <see cref="ListViewItem"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        public virtual void Assign(ListViewItem item)
        {
            Tag = item.Tag;
            var count = item.Cells.Count;
            ResizeCells(count);
            for (int i = 0; i < count; i++)
                Cells[i].Assign(item.Cells[i]);
        }

        /// <summary>
        /// Retrieves the bounding rectangle for this item.
        /// </summary>
        /// <param name="portion">One of the
        /// <see cref="ListViewItemBoundsPortion"/> values that represents a portion of
        /// the item for which to retrieve the bounding rectangle.</param>
        /// <returns>A <see cref="RectD"/> that represents the bounding
        /// rectangle for the specified portion of this item.</returns>
        public virtual RectD GetItemBounds(
            ListViewItemBoundsPortion portion = ListViewItemBoundsPortion.EntireItem)
                => RequiredListView.GetItemBounds(RequiredIndex, portion);

        /// <summary>
        /// Sets owner control and item index. Do not call directly.
        /// </summary>
        /// <param name="control">Owner control.</param>
        /// <param name="newIndex">Item index.</param>
        public virtual void InternalSetListViewAndIndex(ListView? control, long? newIndex)
        {
            ListView = control;
            Index = newIndex;
        }

        /// <summary>
        /// Applies columns to the owner control. Do not call directly.
        /// </summary>
        public virtual void ApplyColumns()
        {
            if (listView == null)
                return;
            ResizeCells(listView.Columns.Count);
        }

        internal void ResizeCells(int count)
        {
            Cells.SetCount(count, () => new ListViewItemCell());
        }

        private void OnCellInserted(object? sender, int index, ListViewItemCell item)
        {
            item.ColumnIndex = index;
            item.Item = this;
        }

        private void OnCellRemoved(object? sender, int index, ListViewItemCell item)
        {
            item.ColumnIndex = null;
            item.Item = null;
        }
    }
}