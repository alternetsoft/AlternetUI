using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list view control, which displays a collection of items that can be displayed using one of several different views.
    /// </summary>
    /// <remarks>
    /// A <see cref="ListView"/> control allows you to display a list of items with item text and, optionally, an icon to identify the type of item.
    /// The <see cref="ListViewItem"/> class represents an item within a ListView control.
    /// The items that are displayed in the list can be shown in one of several different views.
    /// Items can be displayed as large icons, as small icons, or as small icons in a vertical list.
    /// Items can also be subdivided into columns in the <see cref="ListViewView.Details"/> view, which allows you to display the items
    /// in a grid with column headers. <see cref="ListView"/> supports single or multiple selection.
    /// </remarks>
    public class ListView : Control
    {
        private ListViewView view = ListViewView.List;

        private ImageList? smallImageList = null;
        private ImageList? largeImageList = null;

        private HashSet<int> selectedIndices = new HashSet<int>();

        private ListViewSelectionMode selectionMode = ListViewSelectionMode.Single;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
            Columns.ItemInserted += Columns_ItemInserted;
            Columns.ItemRemoved += Columns_ItemRemoved;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property or the <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the selected index in the <see cref="ListView"/> has been changed.
        /// This can be useful when you need to display information in other controls based on the current selection in the <see cref="ListView"/>.
        /// <para>
        /// You can use the event handler for this event to load the information in the other controls. If the <see cref="SelectionMode"/> property
        /// is set to <see cref="ListViewSelectionMode.Multiple"/>, any change to the <see cref="SelectedIndices"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// <para>
        /// The <see cref="SelectedIndices"/> collection changes whenever an individual <see cref="ListViewItem"/> selection changes.
        /// The property change can occur programmatically or when the user selects an item or clears the selection of an item.
        /// </para>
        /// </remarks>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Occurs when the <see cref="View"/> property value changes.
        /// </summary>
        public event EventHandler? ViewChanged;

        /// <summary>
        /// Occurs when the <see cref="SmallImageList"/> property value changes.
        /// </summary>
        public event EventHandler? SmallImageListChanged;

        /// <summary>
        /// Occurs when the <see cref="LargeImageList"/> property value changes.
        /// </summary>
        public event EventHandler? LargeImageListChanged;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all currently selected items in the <see cref="ListView"/>.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the currently selected items in the control.
        /// If no items are currently selected, an empty <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListView"/>, this property returns a collection containing the indexes to all items that are selected
        /// in the <see cref="ListView"/>. For a single-selection <see cref="ListView"/>, this property returns a collection containing a
        /// single element containing the index of the only selected item in the <see cref="ListView"/>.
        /// <para>
        /// The <see cref="ListView"/> class provides a number of ways to reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item in a single-selection <see cref="ListView"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain the item that is currently selected in the <see cref="ListView"/>,
        /// instead of the index position of the item, use the <see cref="SelectedItem"/> property. In addition,
        /// you can use the <see cref="SelectedItems"/> property if you want to obtain all the selected items in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        public IReadOnlyList<int> SelectedIndices
        {
            get
            {
                CheckDisposed();
                return selectedIndices.ToArray();
            }

            set
            {
                CheckDisposed();

                ClearSelectedCore();

                bool changed = false;
                foreach (var index in value)
                {
                    if (SetSelectedCore(index, true))
                        changed = true;
                }

                if (changed)
                    RaiseSelectionChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in a <see cref="ListView"/>.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value of <c>null</c> is returned if no item is selected.</value>
        /// <remarks>
        /// For a standard <see cref="ListView"/>, you can use this property to determine the index of the item that is selected
        /// in the <see cref="ListView"/>. If the <see cref="SelectionMode"/> property of the <see cref="ListView"/> is set to either
        /// <see cref="ListViewSelectionMode.Multiple"/> (which indicates a multiple-selection <see cref="ListView"/>) and multiple items
        /// are selected in the list, this property can return the index to any selected item.
        /// <para>
        /// To retrieve a collection containing the indexes of all selected items in a multiple-selection <see cref="ListView"/>,
        /// use the <see cref="SelectedIndices"/> property. If you want to obtain the item that is currently selected in the <see cref="ListView"/>,
        /// use the <see cref="SelectedItem"/> property. In addition, you can use the <see cref="SelectedItems"/> property to obtain
        /// all the selected items in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value is less than 0 or greater than or equal to the item count.</exception>
        public int? SelectedIndex
        {
            get
            {
                CheckDisposed();
                return selectedIndices.FirstOrDefault();
            }

            set
            {
                CheckDisposed();

                if (value != null && (value < 0 || value >= Items.Count))
                    throw new ArgumentOutOfRangeException(nameof(value));

                ClearSelected();
                if (value != null)
                    SetSelected(value.Value, true);
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item in the ListView.
        /// </summary>
        /// <value>A <see cref="ListViewItem"/> object that represents the current selection in the control, or <c>null</c> if no item is selected.</value>
        /// <remarks>
        /// <para>
        /// You can use this property to determine the item that is selected in the <see cref="ListView"/>.
        /// If the <see cref="SelectionMode"/> property of the <see cref="ListView"/> is set to
        /// <see cref="ListViewSelectionMode.Multiple"/> and multiple items are selected in the list, this property can return any selected item.
        /// </para>
        /// <para>
        /// To retrieve a collection containing all selected items in a multiple-selection <see cref="ListView"/>, use the <see cref="SelectedItems"/> property.
        /// If you want to obtain the index position of the currently selected item in the <see cref="ListView"/>, use the <see cref="SelectedIndex"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property to obtain all the selected indexes in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        public ListViewItem? SelectedItem
        {
            get
            {
                CheckDisposed();

                if (SelectedIndex == null)
                    return null;

                return Items[SelectedIndex.Value];
            }

            set
            {
                CheckDisposed();

                if (value == null)
                {
                    SelectedIndex = -1;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        /// <summary>
        /// Gets a collection containing the currently selected items in the <see cref="ListView"/>.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{ListViewItem}"/> containing the currently selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListView"/>, this property returns a collection containing all the items that are selected
        /// in the <see cref="ListView"/>. For a single-selection <see cref="ListView"/>, this property returns a collection containing a
        /// single element containing the index of the only selected item in the <see cref="ListView"/>.
        /// <para>
        /// The <see cref="ListView"/> class provides a number of ways to reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item in a single-selection <see cref="ListView"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain the item that is currently selected in the <see cref="ListView"/>,
        /// instead of the index position of the item, use the <see cref="SelectedItem"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property to obtain all the selected indexes in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        public IReadOnlyList<ListViewItem> SelectedItems
        {
            get
            {
                CheckDisposed();

                return SelectedIndices.Select(x => Items[x]).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the method in which items are selected in the <see cref="ListView"/>.
        /// </summary>
        /// <value>One of the <see cref="ListViewSelectionMode"/> values. The default is <see cref="ListViewSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine how many items in the <see cref="ListView"/>
        /// a user can select at one time.
        /// </remarks>
        public ListViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();

                return selectionMode;
            }

            set
            {
                CheckDisposed();

                if (selectionMode == value)
                    return;

                selectionMode = value;

                SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection containing all items in the control.
        /// </summary>
        /// <value>A <see cref="Collection{ListViewItem}"/> that contains all the items in the <see cref="ListView"/> control.</value>
        /// <remarks>Using the <see cref="Collection{ListViewItem}"/> returned by this property, you can add items, remove items, and obtain a count of items.</remarks>
        public Collection<ListViewItem> Items { get; } = new Collection<ListViewItem> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Gets the collection of all columns that appear in the control in the <see cref="ListViewView.Details"/> <see cref="View"/>..
        /// </summary>
        /// <value>A collection that represents the columns that appear when the <see cref="View"/> property is set to <see cref="ListViewView.Details"/>.</value>
        public Collection<ListViewColumn> Columns { get; } = new Collection<ListViewColumn> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Gets or sets how items are displayed in the control.
        /// </summary>
        /// <value>One of the <see cref="ListViewView"/> values. The default is <see cref="ListViewView.List"/>.</value>
        public ListViewView View
        {
            get
            {
                CheckDisposed();
                return view;
            }

            set
            {
                CheckDisposed();

                if (view == value)
                    return;

                view = value;

                ViewChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying items as small icons in the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the icons to use when the <see cref="View"/> property
        /// is set to any value other than <see cref="ListViewView.LargeIcon"/>. The default is <c>null</c>.</value>
        /// <remarks>
        /// <para>
        /// The <see cref="SmallImageList"/> property allows you to specify an <see cref="ImageList"/> object that contains icons to use when displaying
        /// items with small icons (when the <see cref="View"/> property is set to any value other than <see cref="ListViewView.LargeIcon"/>). The <see cref="ListView"/> control
        /// can accept any graphics format that the <see cref="ImageList"/> control supports when displaying icons. The <see cref="ListView"/> control
        /// is not limited to .ico files. Once an <see cref="ImageList"/> is assigned to the <see cref="SmallImageList"/> property, you can set the
        /// <see cref="ListViewItem.ImageIndex"/> property of each <see cref="ListViewItem"/> in the <see cref="ListView"/> control to the index position of the appropriate
        /// image in the <see cref="ImageList"/>. The size of the icons for the <see cref="SmallImageList"/> is specified by the <see cref="ImageList.PixelImageSize"/> property.
        /// </para>
        /// <para>
        /// Because only one index can be specified for the <see cref="ListViewItem.ImageIndex"/> property, the <see cref="ImageList"/> objects
        /// specified in the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties should have the same index positions for the
        /// images to display. For example, if the <see cref="ListViewItem.ImageIndex"/> property of a <see cref="ListViewItem"/> is set to 0, the images to use
        /// for both small and large icons should be at the same index position in the <see cref="ImageList"/> objects specified in
        /// the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties.
        /// </para>
        /// <para>
        /// To set the <see cref="ImageList"/> to use when displaying items with large icons (when the <see cref="View"/> property is set to <see cref="ListViewView.LargeIcon"/>),
        /// use the <see cref="LargeImageList"/> property.
        /// </para>
        /// </remarks>
        public ImageList? SmallImageList
        {
            get
            {
                CheckDisposed();
                return smallImageList;
            }

            set
            {
                CheckDisposed();

                if (smallImageList == value)
                    return;

                smallImageList = value;

                SmallImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying items as large icons in the control.
        /// </summary>
        /// <value>
        /// An <see cref="ImageList"/> that contains the icons to use when the <see cref="View"/> property
        /// is set to <see cref="ListViewView.LargeIcon"/>. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="LargeImageList"/> property allows you to specify an <see cref="ImageList"/> object that contains icons to use when displaying
        /// items with large icons (when the <see cref="View"/> property is set to <see cref="ListViewView.LargeIcon"/>). The <see cref="ListView"/> control
        /// can accept any graphics format that the <see cref="ImageList"/> control supports when displaying icons. The <see cref="ListView"/> control
        /// is not limited to .ico files. Once an <see cref="ImageList"/> is assigned to the <see cref="LargeImageList"/> property, you can set the
        /// <see cref="ListViewItem.ImageIndex"/> property of each <see cref="ListViewItem"/> in the <see cref="ListView"/> control to the index position of the appropriate
        /// image in the <see cref="ImageList"/>. The size of the icons for the <see cref="LargeImageList"/> is specified by the <see cref="ImageList.PixelImageSize"/> property.
        /// </para>
        /// <para>
        /// Because only one index can be specified for the <see cref="ListViewItem.ImageIndex"/> property, the <see cref="ImageList"/> objects
        /// specified in the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties should have the same index positions for the
        /// images to display. For example, if the <see cref="ListViewItem.ImageIndex"/> property of a <see cref="ListViewItem"/> is set to 0, the images to use
        /// for both small and large icons should be at the same index position in the <see cref="ImageList"/> objects specified in
        /// the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties.
        /// </para>
        /// <para>
        /// To set the <see cref="ImageList"/> to use when displaying items with small icons (when the <see cref="View"/>
        /// property is set to any value other than <see cref="ListViewView.LargeIcon"/>),
        /// use the <see cref="SmallImageList"/> property.
        /// </para>
        /// </remarks>
        public ImageList? LargeImageList
        {
            get
            {
                CheckDisposed();
                return largeImageList;
            }

            set
            {
                CheckDisposed();

                if (largeImageList == value)
                    return;

                largeImageList = value;

                LargeImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Unselects all items in the <see cref="ListView"/>.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the <see cref="SelectedIndex"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the list.
        /// </remarks>
        public void ClearSelected()
        {
            ClearSelectedCore();
            RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Selects or clears the selection for the specified item in a <see cref="ListView"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a <see cref="ListView"/> to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item; otherwise, false.</param>
        /// <remarks>
        /// You can use this method to set the selection of items in a multiple-selection <see cref="ListView"/>.
        /// To select an item in a single-selection <see cref="ListView"/>, use the <see cref="SelectedIndex"/> property.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The specified index was outside the range of valid values.</exception>
        public void SetSelected(int index, bool value)
        {
            if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(value));

            CheckDisposed();

            var changed = SetSelectedCore(index, value);

            if (changed)
                RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls <see cref="OnSelectionChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseSelectionChanged(EventArgs e)
        {
            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex"/> property or the <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>See <see cref="SelectionChanged"/> for details.</remarks>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        private void ClearSelectedCore()
        {
            selectedIndices.Clear();
        }

        private bool SetSelectedCore(int index, bool value)
        {
            bool changed;
            if (value)
                changed = selectedIndices.Add(index);
            else
                changed = selectedIndices.Remove(index);

            return changed;
        }

        private void Columns_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            e.Item.ListView = this;
            e.Item.Index = e.Index;
        }

        private void Columns_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            e.Item.ListView = null;
            e.Item.Index = null;
        }
    }
}