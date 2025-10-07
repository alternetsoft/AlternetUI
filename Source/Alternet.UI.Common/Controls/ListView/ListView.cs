using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list view control, which displays a collection of items that can be displayed
    /// using one of several different views.
    /// </summary>
    /// <remarks>
    /// A <see cref="ListView"/> control allows you to display a list of items with item text
    /// and, optionally, an icon to identify the type of item.
    /// The <see cref="ListViewItem"/> class represents an item within a ListView control.
    /// The items that are displayed in the list can be shown in one of several different views.
    /// Items can be displayed as large icons, as small icons, or as small icons in a vertical list.
    /// Items can also be subdivided into columns in the <see cref="ListViewView.Details"/> view,
    /// which allows you to display the items
    /// in a grid with column headers. <see cref="ListView"/> supports single or multiple selection.
    /// </remarks>
    [DefaultProperty("Items")]
    [DefaultEvent("SelectionChanged")]
    [ControlCategory("Common")]
    public partial class ListView : Control
    {
        private HashSet<long>? selectedIndices = null;

        private ListViewView view = ListViewView.List;
        private ImageList? smallImageList = null;
        private ImageList? largeImageList = null;
        private ListViewSelectionMode selectionMode = ListViewSelectionMode.Single;
        private int suspendEventsCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListView(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
            bool? hasBorder = AllPlatformDefaults.GetHasBorderOverride(ControlKind);

            if (hasBorder is not null)
                HasBorder = hasBorder.Value;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the selected index
        /// in the <see cref="ListView"/> has been changed.
        /// This can be useful when you need to display information in other controls based on
        /// the current selection in the <see cref="ListView"/>.
        /// <para>
        /// You can use the event handler for this event to load the information in the
        /// other controls. If the <see cref="SelectionMode"/> property
        /// is set to <see cref="ListViewSelectionMode.Multiple"/>, any change to the
        /// <see cref="SelectedIndices"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// <para>
        /// The <see cref="SelectedIndices"/> collection changes whenever an individual
        /// <see cref="ListViewItem"/> selection changes.
        /// The property change can occur programmatically or when the user selects an
        /// item or clears the selection of an item.
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
        /// Occurs when the user clicks a column header within the list view control.
        /// </summary>
        public event EventHandler<ListViewColumnEventArgs>? ColumnClick;

        /// <summary>
        /// Occurs before the item label text is edited. This event can be canceled.
        /// </summary>
        public event EventHandler<ListViewItemLabelEditEventArgs>? BeforeLabelEdit;

        /// <summary>
        /// Occurs after the item label text is edited. This event can be canceled.
        /// </summary>
        public event EventHandler<ListViewItemLabelEditEventArgs>? AfterLabelEdit;

        /// <summary>
        /// Gets or sets a boolean value which specifies whether the column header is visible in
        /// <see cref="ListViewView.Details"/> view.
        /// </summary>
        public virtual bool ColumnHeaderVisible
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ColumnHeaderVisible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ColumnHeaderVisible = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether events are currently suspended.
        /// </summary>
        [Browsable(false)]
        public virtual bool EventsSuspended
        {
            get => suspendEventsCounter > 0;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ListView;

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit the labels of items in the
        /// control.
        /// </summary>
        /// <value><see langword="true"/> if the user can edit the labels of items at run time;
        /// otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.</value>
        public virtual bool AllowLabelEdit
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.AllowLabelEdit;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.AllowLabelEdit = value;
            }
        }

        /// <summary>
        /// Gets or sets the first fully-visible item in the list view control.
        /// </summary>
        /// <value>A <see cref="ListViewItem"/> that represents the first fully-visible item
        /// in the list view control.</value>
        [Browsable(false)]
        public virtual ListViewItem? TopItem
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.TopItem;
            }
        }

        /// <summary>
        /// Gets or sets the grid line display mode for this list view.
        /// </summary>
        public virtual ListViewGridLinesDisplayMode GridLinesDisplayMode
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.GridLinesDisplayMode;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.GridLinesDisplayMode = value;
            }
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all currently selected
        /// items in the <see cref="ListView"/>.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the currently selected
        /// items in the control.
        /// If no items are currently selected, an empty <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListView"/>, this property returns a collection
        /// containing the indexes to all items that are selected
        /// in the <see cref="ListView"/>. For a single-selection <see cref="ListView"/>, this
        /// property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// <see cref="ListView"/>.
        /// <para>
        /// The <see cref="ListView"/> class provides a number of ways to reference selected
        /// items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item in a
        /// single-selection <see cref="ListView"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain
        /// the item that is currently selected in the <see cref="ListView"/>,
        /// instead of the index position of the item, use the <see cref="SelectedItem"/>
        /// property. In addition,
        /// you can use the <see cref="SelectedItems"/> property if you want to obtain
        /// all the selected items in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<long> SelectedIndices
        {
            get
            {
                if (Items.Count == 0)
                    return Array.Empty<long>();

                CheckDisposed();
                UpdateSelectedIndices();
                return selectedIndices!.ToArray();
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
        /// Gets or sets the zero-based index of the currently selected item in a
        /// <see cref="ListView"/>.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value of <c>null</c>
        /// is returned if no item is selected.</value>
        /// <remarks>
        /// For a standard <see cref="ListView"/>, you can use this property to determine the
        /// index of the item that is selected
        /// in the <see cref="ListView"/>. If the <see cref="SelectionMode"/> property of the
        /// <see cref="ListView"/> is set to either
        /// <see cref="ListViewSelectionMode.Multiple"/> (which indicates a multiple-selection
        /// <see cref="ListView"/>) and multiple items
        /// are selected in the list, this property can return the index to any selected item.
        /// <para>
        /// To retrieve a collection containing the indexes of all selected items in a
        /// multiple-selection <see cref="ListView"/>,
        /// use the <see cref="SelectedIndices"/> property. If you want to obtain the item that
        /// is currently selected in the <see cref="ListView"/>,
        /// use the <see cref="SelectedItem"/> property. In addition, you can use the
        /// <see cref="SelectedItems"/> property to obtain
        /// all the selected items in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value is less than
        /// 0 or greater than or equal to the item count.</exception>
        [Browsable(false)]
        public virtual long? SelectedIndex
        {
            get
            {
                CheckDisposed();
                var selected = SelectedIndices;
                if (selected == null || selected.Count == 0)
                    return null;
                return selected[0];
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
        /// <value>A <see cref="ListViewItem"/> object that represents the current selection
        /// in the control, or <c>null</c> if no item is selected.</value>
        /// <remarks>
        /// <para>
        /// You can use this property to determine the item that is selected in the
        /// <see cref="ListView"/>.
        /// If the <see cref="SelectionMode"/> property of the <see cref="ListView"/> is set to
        /// <see cref="ListViewSelectionMode.Multiple"/> and multiple items are selected in the
        /// list, this property can return any selected item.
        /// </para>
        /// <para>
        /// To retrieve a collection containing all selected items in a multiple-selection
        /// <see cref="ListView"/>, use the <see cref="SelectedItems"/> property.
        /// If you want to obtain the index position of the currently selected item in the
        /// <see cref="ListView"/>, use the <see cref="SelectedIndex"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property to obtain
        /// all the selected indexes in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public virtual ListViewItem? SelectedItem
        {
            get
            {
                CheckDisposed();

                var idx = SelectedIndex;
                if (idx == null)
                    return null;

                long index = (long)idx;

                if (index < 0 || index >= Items.Count)
                    return null;

                return Items[(int)index];
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
        /// <value>A <see cref="IReadOnlyList{ListViewItem}"/> containing the currently
        /// selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListView"/>, this property returns a collection
        /// containing all the items that are selected
        /// in the <see cref="ListView"/>. For a single-selection <see cref="ListView"/>, this
        /// property returns a collection containing a
        /// single element containing the index of the only selected item in the <see cref="ListView"/>.
        /// <para>
        /// The <see cref="ListView"/> class provides a number of ways to reference selected
        /// items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item in a
        /// single-selection <see cref="ListView"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain the
        /// item that is currently selected in the <see cref="ListView"/>,
        /// instead of the index position of the item, use the <see cref="SelectedItem"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property to obtain
        /// all the selected indexes in a multiple-selection <see cref="ListView"/>.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<ListViewItem> SelectedItems
        {
            get
            {
                CheckDisposed();

                return SelectedIndices.Select(x => Items[(int)x]).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the method in which items are selected in the <see cref="ListView"/>.
        /// </summary>
        /// <value>One of the <see cref="ListViewSelectionMode"/> values. The default is
        /// <see cref="ListViewSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine how many items
        /// in the <see cref="ListView"/>
        /// a user can select at one time.
        /// </remarks>
        public virtual ListViewSelectionMode SelectionMode
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

                if(!EventsSuspended)
                    SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection containing all items in the control.
        /// </summary>
        /// <value>A <see cref="BaseCollection{ListViewItem}"/> that contains all the items in the
        /// <see cref="ListView"/> control.</value>
        /// <remarks>Using the <see cref="BaseCollection{ListViewItem}"/> returned by this
        /// property, you can add items, remove items, and obtain a count of items.</remarks>
        public virtual BaseCollection<ListViewItem> Items { get; } =
            new BaseCollection<ListViewItem>(CollectionSecurityFlags.NoNullOrReplace);

        /// <summary>
        /// Gets the collection of all columns that appear in the control in the
        /// <see cref="ListViewView.Details"/> <see cref="View"/>..
        /// </summary>
        /// <value>A collection that represents the columns that appear when the
        /// <see cref="View"/> property is set to <see cref="ListViewView.Details"/>.</value>
        public virtual BaseCollection<ListViewColumn> Columns { get; } =
            new BaseCollection<ListViewColumn>(CollectionSecurityFlags.NoNullOrReplace);

        /// <summary>
        /// Gets or sets how items are displayed in the control.
        /// </summary>
        /// <value>One of the <see cref="ListViewView"/> values. The default is
        /// <see cref="ListViewView.List"/>.</value>
        public virtual ListViewView View
        {
            get
            {
                return view;
            }

            set
            {
                CheckDisposed();

                if (view == value)
                    return;

                view = value;

                if (!EventsSuspended)
                    ViewChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying items as small icons
        /// in the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the icons to use when
        /// the <see cref="View"/> property
        /// is set to any value other than <see cref="ListViewView.LargeIcon"/>.
        /// The default is <c>null</c>.</value>
        /// <remarks>
        /// <para>
        /// The <see cref="SmallImageList"/> property allows you to specify
        /// an <see cref="ImageList"/> object that contains icons to use when displaying
        /// items with small icons (when the <see cref="View"/> property is
        /// set to any value other than <see cref="ListViewView.LargeIcon"/>).
        /// The <see cref="ListView"/> control
        /// can accept any graphics format that the <see cref="ImageList"/> control
        /// supports when displaying icons. The <see cref="ListView"/> control
        /// is not limited to .ico files. Once an <see cref="ImageList"/> is assigned
        /// to the <see cref="SmallImageList"/> property, you can set the
        /// <see cref="ListViewItem.ImageIndex"/> property of each <see cref="ListViewItem"/>
        /// in the <see cref="ListView"/> control to the index position of the appropriate
        /// image in the <see cref="ImageList"/>. The size of the icons for the
        /// <see cref="SmallImageList"/> is specified by the <see cref="ImageList.ImageSize"/>
        /// property.
        /// </para>
        /// <para>
        /// Because only one index can be specified for the <see cref="ListViewItem.ImageIndex"/>
        /// property, the <see cref="ImageList"/> objects
        /// specified in the <see cref="LargeImageList"/> and <see cref="SmallImageList"/>
        /// properties should have the same index positions for the
        /// images to display. For example, if the <see cref="ListViewItem.ImageIndex"/>
        /// property of a <see cref="ListViewItem"/> is set to 0, the images to use
        /// for both small and large icons should be at the same index position
        /// in the <see cref="ImageList"/> objects specified in
        /// the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties.
        /// </para>
        /// <para>
        /// To set the <see cref="ImageList"/> to use when displaying items with large
        /// icons (when the <see cref="View"/> property is set to <see cref="ListViewView.LargeIcon"/>),
        /// use the <see cref="LargeImageList"/> property.
        /// </para>
        /// </remarks>
        public virtual ImageList? SmallImageList
        {
            get
            {
                return smallImageList;
            }

            set
            {
                CheckDisposed();

                if (smallImageList == value)
                    return;

                smallImageList = value;

                if (!EventsSuspended)
                    SmallImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying items as large
        /// icons in the control.
        /// </summary>
        /// <value>
        /// An <see cref="ImageList"/> that contains the icons to use when the <see cref="View"/>
        /// property
        /// is set to <see cref="ListViewView.LargeIcon"/>. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="LargeImageList"/> property allows you to specify an
        /// <see cref="ImageList"/> object that contains icons to use when displaying
        /// items with large icons (when the <see cref="View"/> property is set
        /// to <see cref="ListViewView.LargeIcon"/>). The <see cref="ListView"/> control
        /// can accept any graphics format that the <see cref="ImageList"/> control
        /// supports when displaying icons. The <see cref="ListView"/> control
        /// is not limited to .ico files. Once an <see cref="ImageList"/> is assigned
        /// to the <see cref="LargeImageList"/> property, you can set the
        /// <see cref="ListViewItem.ImageIndex"/> property of each <see cref="ListViewItem"/>
        /// in the <see cref="ListView"/> control to the index position of the appropriate
        /// image in the <see cref="ImageList"/>. The size of the icons for the
        /// <see cref="LargeImageList"/> is specified by the <see cref="ImageList.ImageSize"/>
        /// property.
        /// </para>
        /// <para>
        /// Because only one index can be specified for the <see cref="ListViewItem.ImageIndex"/>
        /// property, the <see cref="ImageList"/> objects
        /// specified in the <see cref="LargeImageList"/> and <see cref="SmallImageList"/>
        /// properties should have the same index positions for the
        /// images to display. For example, if the <see cref="ListViewItem.ImageIndex"/>
        /// property of a <see cref="ListViewItem"/> is set to 0, the images to use
        /// for both small and large icons should be at the same index position in the
        /// <see cref="ImageList"/> objects specified in
        /// the <see cref="LargeImageList"/> and <see cref="SmallImageList"/> properties.
        /// </para>
        /// <para>
        /// To set the <see cref="ImageList"/> to use when displaying items with small
        /// icons (when the <see cref="View"/>
        /// property is set to any value other than <see cref="ListViewView.LargeIcon"/>),
        /// use the <see cref="SmallImageList"/> property.
        /// </para>
        /// </remarks>
        public virtual ImageList? LargeImageList
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

                if (!EventsSuspended)
                    LargeImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected items in the <see cref="ListView"/>.
        /// </summary>
        /// <remarks>
        /// Indexes are returned in the descending order (maximal index is the first).
        /// </remarks>
        /// <seealso cref="SelectedIndices"/>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently selected items in the control.
        /// If no items are currently selected, an empty <see cref="IReadOnlyList{T}"/>
        /// is returned.
        /// </value>
        [Browsable(false)]
        public virtual IReadOnlyList<long> SelectedIndicesDescending
        {
            get
            {
#pragma warning disable
                long[] sortedCopy = SelectedIndices.OrderByDescending(i => i).ToArray();
#pragma warning restore
                return sortedCopy;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control has a border.
        /// </summary>
        /// <remarks>
        /// This property currently does nothing and <see cref="ListView"/>
        /// is always without the border.
        /// This is done to prevent bad behavior on Windows platform in some situations.
        /// In order to have border place <see cref="ListView"/> inside <see cref="Border"/> control.
        /// </remarks>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return base.Handler.HasBorder;
            }

            set
            {
                base.Handler.HasBorder = value;
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <summary>
        /// Gets a <see cref="IListViewHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new IListViewHandler Handler
        {
            get
            {
                CheckDisposed();
                return (IListViewHandler)base.Handler;
            }
        }

        /// <summary>
        /// Removes selected items from the <see cref="ListView"/>.
        /// </summary>
        public virtual void RemoveSelectedItems()
        {
            RemoveItems(SelectedIndicesDescending);
        }

        /// <summary>
        /// Removes items from the <see cref="ListView"/>.
        /// </summary>
        public virtual void RemoveItems(IReadOnlyList<long> items)
        {
            if (items == null || items.Count == 0)
                return;

            BeginUpdate();
            try
            {
                ClearSelected();
                foreach (var index in items)
                {
                    if (index < Items.Count && index >= 0)
                    {
                        Items.RemoveAt((int)index);
                    }
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Provides list view item information, at a given client point, in
        /// device-independent units.
        /// </summary>
        /// <param name="point">The <see cref="PointD"/> at which to retrieve item information.</param>
        /// <returns>The hit test result information.</returns>
        /// <remarks>
        /// Use this method to determine whether a point is located in a <see cref="ListViewItem"/>
        /// and where within the
        /// item the point is located, such as on the label or image area.
        /// </remarks>
        public virtual ListViewHitTestInfo HitTest(PointD point)
        {
            if (DisposingOrDisposed)
                return new();
            return Handler.HitTest(point);
        }

        /// <summary>
        /// Initiates the editing of the list view item label.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item within the
        /// <see cref="ListView.Items"/> collection
        /// whose label you want to edit.</param>
        public virtual void BeginLabelEdit(long itemIndex)
        {
            if (DisposingOrDisposed)
                return;
            Handler.BeginLabelEdit(itemIndex);
        }

        /// <summary>
        /// Retrieves the bounding rectangle for an item within the control.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item within the
        /// <see cref="ListView.Items"/> collection
        /// whose bounding rectangle you want to get.</param>
        /// <param name="portion">One of the <see cref="ListViewItemBoundsPortion"/>
        /// values that represents a portion of
        /// the item for which to retrieve the bounding rectangle.</param>
        /// <returns>A <see cref="RectD"/> that represents the bounding rectangle
        /// for the specified portion of the
        /// specified <see cref="ListViewItem"/>.</returns>
        public virtual RectD GetItemBounds(
            long itemIndex,
            ListViewItemBoundsPortion portion = ListViewItemBoundsPortion.EntireItem)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetItemBounds(itemIndex, portion);
        }

        /// <summary>
        /// Temporarily suspends the raising of events.
        /// </summary>
        /// <remarks>This method increments an internal counter to track the suspension state.
        /// Events will remain suspended until the counter is decremented to zero by a corresponding
        /// call to <see cref="ResumeEvents" />. Ensure that each
        /// call to <see cref="SuspendEvents" /> is paired with a call to
        /// <see cref="ResumeEvents" /> to avoid unintended behavior.</remarks>
        public virtual void SuspendEvents()
        {
            suspendEventsCounter++;
        }

        /// <summary>
        /// Resumes the processing of events by decrementing the suspension counter.
        /// </summary>
        /// <remarks>This method decreases the internal suspension counter, allowing events to be
        /// processed if the counter reaches zero. Ensure that each call to
        /// <see cref="ResumeEvents"/> corresponds to a
        /// prior call to a method that increments the suspension counter,
        /// such as a suspend method.</remarks>
        public virtual void ResumeEvents()
        {
            if (suspendEventsCounter > 0)
                suspendEventsCounter--;
        }

        /// <summary>
        /// Gets whether there are columns with the specified width mode.
        /// </summary>
        public virtual bool HasSuchColumns(ListViewColumnWidthMode mode)
        {
            foreach (var column in Columns)
            {
                if (column.WidthMode == mode)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all items and columns from the control.
        /// </summary>
        public virtual void Clear()
        {
            if (DisposingOrDisposed)
                return;
            Handler.Clear();
        }

        /// <summary>
        /// Raises the <see cref="ColumnClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ListViewColumnEventArgs"/> that contains
        /// the event data.</param>
        public void RaiseColumnClick(ListViewColumnEventArgs e)
        {
            OnColumnClick(e);
            if (!EventsSuspended)
                ColumnClick?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeLabelEdit"/> event and calls <see cref="OnBeforeLabelEdit"/>.
        /// </summary>
        /// <param name="e">An <see cref="ListViewItemLabelEditEventArgs"/> that
        /// contains the event data.</param>
        public void RaiseBeforeLabelEdit(ListViewItemLabelEditEventArgs e)
        {
            OnBeforeLabelEdit(e);
            if (!EventsSuspended)
                BeforeLabelEdit?.Invoke(this, e);
        }

        /// <summary>
        /// Selects or clears the selection for the specified item in a <see cref="ListView"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a <see cref="ListView"/>
        /// to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item; otherwise, false.</param>
        /// <remarks>
        /// You can use this method to set the selection of items in a multiple-selection
        /// <see cref="ListView"/>.
        /// To select an item in a single-selection <see cref="ListView"/>, use
        /// the <see cref="SelectedIndex"/> property.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The specified index was
        /// outside the range of valid values.</exception>
        public virtual void SetSelected(long index, bool value)
        {
            if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(value));

            CheckDisposed();

            var changed = SetSelectedCore(index, value);

            if (changed)
                RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls
        /// <see cref="OnSelectionChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseSelectionChanged(EventArgs e)
        {
            OnSelectionChanged(e);
            if (!EventsSuspended)
                SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterLabelEdit"/> event and calls <see cref="OnAfterLabelEdit"/>.
        /// </summary>
        /// <param name="e">An <see cref="ListViewItemLabelEditEventArgs"/> that contains
        /// the event data.</param>
        public void RaiseAfterLabelEdit(ListViewItemLabelEditEventArgs e)
        {
            OnAfterLabelEdit(e);
            if (!EventsSuspended)
                AfterLabelEdit?.Invoke(this, e);
        }

        /// <summary>
        /// Updates columns width and title.
        /// </summary>
        public virtual void UpdateColumns()
        {
            DoInsideUpdate(() =>
            {
                foreach (var column in Columns)
                    column.RaiseChanged(ListViewColumn.ColumnEventType.AllChanged);
            });
        }

        /// <summary>
        /// Removed all items from the control.
        /// </summary>
        public virtual void RemoveAll()
        {
            BeginUpdate();
            try
            {
                ClearSelected();
                Items.Clear();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Handles column change events. This method is called when
        /// column's width, title or other properties are changed.
        /// </summary>
        public virtual void HandleColumnChanged(
            ListViewColumn column,
            ListViewColumn.ColumnEventType eventType)
        {
            if (DisposingOrDisposed)
                return;

            var index = column.Index;

            if (index is null || index < 0)
                return;

            var setWidth = false;
            var setTitle = false;

            switch (eventType)
            {
                case ListViewColumn.ColumnEventType.TitleChanged:
                    setTitle = true;
                    break;
                case ListViewColumn.ColumnEventType.WidthChanged:
                    setWidth = true;
                    break;
                case ListViewColumn.ColumnEventType.AllChanged:
                    setWidth = true;
                    setTitle = true;
                    break;
            }

            if (setTitle)
            {
                Handler.SetColumnTitle(index.Value, column.Title);
            }

            if (setWidth)
            {
                var widthMode = column.WidthMode;
                var width = column.Width;

                if(widthMode == ListViewColumnWidthMode.FixedInPercent)
                {
                    var calculatedWidth = column.WidthInPercentToDips((int)width, column.MinAutoWidth);
                    if (calculatedWidth is null)
                        return;
                    widthMode = ListViewColumnWidthMode.Fixed;
                    width = calculatedWidth.Value;
                }

                Handler.SetColumnWidth(index.Value, width, widthMode);
            }
        }

        /// <summary>
        /// Unselects all items in the <see cref="ListView"/>.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the <see cref="SelectedIndex"/>
        /// property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the list.
        /// </remarks>
        public virtual void ClearSelected()
        {
            ClearSelectedCore();
            RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Internal method. Do not call it directly.
        /// </summary>
        [Browsable(false)]
        public virtual void SelectedIndicesAreDirty()
        {
            selectedIndices = null;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateListViewHandler(this);
        }

        /// <summary>
        /// Called when the user clicks a column header within the list view control.
        /// </summary>
        /// <param name="e">An <see cref="ListViewColumnEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnColumnClick(ListViewColumnEventArgs e)
        {
        }

        /// <summary>
        /// Called before a list view item label is edited.
        /// </summary>
        /// <param name="e">An <see cref="ListViewItemLabelEditEventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnBeforeLabelEdit(ListViewItemLabelEditEventArgs e)
        {
        }

        /// <summary>
        /// Called after a list view item label is edited.
        /// </summary>
        /// <param name="e">An <see cref="ListViewItemLabelEditEventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnAfterLabelEdit(ListViewItemLabelEditEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (HasSuchColumns(ListViewColumnWidthMode.FixedInPercent))
            {
                App.AddIdleTask(() =>
                {
                    try
                    {
                        if (IsDisposed || App.Terminating)
                            return;

                        foreach (var column in Columns)
                        {
                            if (column.WidthMode != ListViewColumnWidthMode.FixedInPercent)
                                continue;
                            column.RaiseChanged(ListViewColumn.ColumnEventType.WidthChanged);
                        }
                    }
                    catch
                    {
                        if(DebugUtils.IsDebugDefined)
                            throw;
                    }
                });
            }
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>See <see cref="SelectionChanged"/> for details.</remarks>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        private void ClearSelectedCore()
        {
            selectedIndices?.Clear();
        }

        private void UpdateSelectedIndices()
        {
            if (DisposingOrDisposed)
                return;
            if (Items.Count == 0)
                return;
            if (selectedIndices == null)
            {
                selectedIndices = new();
                var indices = Handler.SelectedIndices;
                selectedIndices.UnionWith(indices);
            }
        }

        private bool SetSelectedCore(long index, bool value)
        {
            UpdateSelectedIndices();
            bool changed;
            if (value)
                changed = selectedIndices!.Add(index);
            else
                changed = selectedIndices!.Remove(index);

            return changed;
        }
    }
}