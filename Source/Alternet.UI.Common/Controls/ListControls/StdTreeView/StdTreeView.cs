using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items with optional images,
    /// each represented by a <see cref="TreeViewItem"/>. This control uses
    /// internal <see cref="VirtualListControl"/> for displaying items, it can be accessed via
    /// <see cref="ListBox"/> property.
    /// </summary>
    [ControlCategory("Common")]
    public partial class StdTreeView : Border, ITreeViewItemContainer
    {
        /// <summary>
        /// Gets or sets the names of properties of <see cref="TreeViewItem"/>
        /// that should raise an invalidation of the container when changed.
        /// </summary>
        public static string[] ItemPropertyNamesToRaiseInvalidate = new[]
        {
            nameof(TreeViewItem.Text),
            nameof(TreeViewItem.ImageIndex),
            nameof(TreeViewItem.Value),
            nameof(TreeViewItem.DisplayText),
        };

        /// <summary>
        /// Specifies the default type of buttons used in the tree view control to expand
        /// or collapse nodes.
        /// </summary>
        /// <remarks>
        /// This field determines the initial value of the <see cref="TreeButtons"/> property
        /// for instances of <see cref="StdTreeView"/>.
        /// The default value is <see cref="TreeViewButtonsKind.Angle"/>.
        /// </remarks>
        public static TreeViewButtonsKind DefaultTreeButtons = TreeViewButtonsKind.Angle;

        /// <summary>
        /// Default margin for each level in the tree.
        /// </summary>
        public static int DefaultLevelMargin = 16;

        private VirtualListBox? listBox;
        private TreeViewRootItem rootItem;
        private TreeViewButtonsKind treeButtons = TreeViewButtonsKind.Null;
        private bool needTreeChanged;
        private ListBoxHeader? header;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdTreeView"/> class.
        /// </summary>
        public StdTreeView()
        {
            base.Layout = LayoutStyle.Vertical;

            rootItem = new(this);

            ListBox.MouseDown += OnListBoxMouseDown;
            ListBox.DoubleClick += OnListBoxDoubleClick;
            ListBox.KeyDown += OnListBoxKeyDown;

            TreeButtons = DefaultTreeButtons;
        }

        /// <summary>
        /// Occurs when the selection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the
        /// selected index in the control has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the control.
        /// </remarks>
        public event EventHandler? SelectionChanged
        {
            add
            {
                ListBox.SelectionChanged += value;
            }

            remove
            {
                ListBox.SelectionChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when a property of a <see cref="TreeViewItem"/> changes.
        /// This is not fired for all properties of the item.
        /// </summary>
        public event EventHandler<TreeViewItemPropChangedEventArgs>? ItemPropertyChanged;

        /// <summary>
        /// Occurs when the selection state of an item changes.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ItemSelectedChanged;

        /// <summary>
        /// Occurs when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when tree structure changes.
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Occurs when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs before the tree item is expanded. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeViewCancelEventArgs>? BeforeExpand;

        /// <summary>
        /// Occurs before the tree item is collapsed. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeViewCancelEventArgs>? BeforeCollapse;

        /// <summary>
        /// Occurs after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public event EventHandler<UI.TreeViewEventArgs>? ExpandedChanged;

        /// <summary>
        /// Gets or sets the selection mode (single or multiple).
        /// </summary>
        /// <remarks>The selection mode determines whether multiple items can be selected
        /// at once and how the selection behaves.
        /// For example, <see cref="TreeViewSelectionMode.Single"/> allows only one item to be
        /// selected, while  <see cref="TreeViewSelectionMode.Multiple"/> allows multiple
        /// items to be selected.</remarks>
        public TreeViewSelectionMode SelectionMode
        {
            get
            {
                return (TreeViewSelectionMode)ListBox.SelectionMode;
            }

            set
            {
                ListBox.SelectionMode = (ListBoxSelectionMode)value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control has columns defined.
        /// </summary>
        /// <value>
        /// <c>true</c> if the control has one or more columns; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool HasColumns => ListBox.HasColumns;

        /// <summary>
        /// Gets or sets the collection of columns displayed in the list control.
        /// </summary>
        /// <remarks>Use this property to configure the columns that appear in the list control. Modifying
        /// the collection allows customization of the column layout, order, and properties.</remarks>
        [Browsable(false)]
        public BaseCollection<ListControlColumn> Columns
        {
            get => ListBox.Columns;

            set => ListBox.Columns = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to track changes of
        /// <see cref="TreeViewItem"/> properties. Default is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public virtual bool TrackItemPropertyChanges { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to track changes of
        /// <see cref="TreeViewItem"/> selection state. Default is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public virtual bool TrackItemSelectedChanges { get; set; } = true;

        /// <summary>
        /// Gets the underlying <see cref="VirtualListBox"/> used by this tree control.
        /// </summary>
        [Browsable(false)]
        public virtual VirtualListBox ListBox
        {
            get
            {
                if (listBox is null)
                {
                    listBox = CreateListBox();
                    listBox.VerticalAlignment = VerticalAlignment.Fill;
                    listBox.ParentFont = true;
                    listBox.HasBorder = false;
                    listBox.SelectionUnderImage = true;
                    listBox.CheckBoxVisible = true;
                    listBox.CheckOnClick = false;
                    listBox.Parent = this;
                }

                return listBox;
            }
        }

        /// <summary>
        /// Gets the header with columns associated with the control.
        /// </summary>
        /// <remarks>The header is automatically created when accessed for the first time.
        /// It is not visible by default.</remarks>
        [Browsable(false)]
        public virtual ListBoxHeader Header
        {
            get
            {
                if (header is null)
                {
                    header = CreateHeader();
                    header.ParentForeColor = true;
                    header.ParentBackColor = true;
                    header.OnlyBottomBorder();
                    header.VerticalAlignment = VerticalAlignment.Top;
                    header.Visible = false;

                    header.ColumnDeleted += OnHeaderColumnDeleted;
                    header.ColumnInserted += OnHeaderColumnInserted;
                    header.ColumnSizeChanged += OnHeaderColumnSizeChanged;
                    header.ColumnVisibleChanged += OnHeaderColumnVisibleChanged;

                    Children.Prepend(header);

                    ListBox.HeaderControl = header;
                }

                return header;
            }
        }

        /// <summary>
        /// Gets first root item in the control or <c>null</c> if there are no items.
        /// </summary>
        /// <remarks>
        /// This property returns the first item even if it is not visible.
        /// </remarks>
        [Browsable(false)]
        public virtual TreeViewItem? FirstItem
        {
            get
            {
                if (RootItem.ItemCount == 0)
                    return null;
                return RootItem.Items[0];
            }
        }

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem? LastItem
        {
            get
            {
                static TreeViewItem? GetLastItem(TreeViewItem? item)
                {
                    if (item is null)
                        return null;
                    if (!item.HasItems)
                        return item;
                    var child = item.LastChild;
                    return GetLastItem(child);
                }

                return GetLastItem(LastRootItem);
            }
        }

        /// <summary>
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        /// <remarks>
        /// This property returns the last root item even if it is not visible.
        /// </remarks>
        [Browsable(false)]
        public virtual TreeViewItem? LastRootItem
        {
            get
            {
                var count = RootItem.ItemCount;
                if (count == 0)
                    return null;
                return RootItem.Items[count - 1];
            }
        }

        /// <summary>
        /// Gets the collection of root items contained within the tree control.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<TreeViewItem> Items
        {
            get => RootItem.Items;
        }

        /// <summary>
        /// Gets or sets a value indicating whether expand buttons are displayed
        /// next to tree items that contain child tree items.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if expand buttons are displayed next to tree
        /// items that contain
        /// child tree items; otherwise, <see langword="false"/>. The default is
        /// <see langword="true"/>.
        /// </value>
        public virtual bool ShowExpandButtons
        {
            get
            {
                return ListBox.CheckBoxVisible;
            }

            set
            {
                ListBox.CheckBoxVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the first visible item in the control.
        /// Returned value is the index in <see cref="VisibleItems"/> collection.</summary>
        /// <returns>The zero-based index of the first visible item in the control.</returns>
        [Browsable(false)]
        public virtual int TopIndex
        {
            get => ListBox.TopIndex;
            set => ListBox.TopIndex = value;
        }

        /// <summary>
        /// Gets or sets the first fully-visible tree item in the control.
        /// This is slower than <see cref="TopIndex"/>, so if you have item index, use that instead.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> that represents the first
        /// fully-visible tree item in the control.</value>
        [Browsable(false)]
        public virtual TreeViewItem? TopItem
        {
            get
            {
                var index = ListBox.TopIndex;

                return ListBox.GetItem(index) as TreeViewItem;
            }

            set
            {
                if (value is null || TopItem == value)
                    return;
                var index = ListBox.Items.IndexOf(value);
                if (index < 0)
                    return;
                ListBox.TopIndex = index;
            }
        }

        /// <summary>
        /// Gets a collection containing the currently selected items in the control.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{ListControlItem}"/> containing the
        /// currently selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="StdTreeView"/>, this property returns
        /// a collection containing all the items that are selected
        /// in the control. For a single-selection
        /// <see cref="StdTreeView"/>, this property returns a collection containing a
        /// single element containing the only selected item in the control.
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<ListControlItem> SelectedListItems
        {
            get
            {
                return ListBox.SelectedItems;
            }

            set
            {
                ListBox.SelectedItems = value;
            }
        }

        /// <summary>
        /// Gets a collection containing the currently selected items in the control.
        /// This is slower than <see cref="SelectedListItems"/>.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{TreeControlItem}"/> containing the
        /// currently selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="StdTreeView"/>, this property returns
        /// a collection containing all the items that are selected
        /// in the <see cref="StdTreeView"/>. For a single-selection
        /// <see cref="StdTreeView"/>, this property returns a collection containing a
        /// single element containing the only selected item in the
        /// <see cref="StdTreeView"/>.
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<TreeViewItem> SelectedItems
        {
            get
            {
                return ListBox.SelectedItems.Cast<TreeViewItem>().ToArray();
            }

            set
            {
                ListBox.SelectedItems = value;
            }
        }

        /// <inheritdoc/>
        public override bool HasOwnInterior => ListBox.HasOwnInterior;

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> associated with the tree control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the images to be used by the tree control.
        /// If no <see cref="ImageList"/> is set, this property returns null.</value>
        public ImageList? ImageList
        {
            get
            {
                return ListBox.ImageList;
            }

            set
            {
                ListBox.ImageList = value;
            }
        }

        /// <inheritdoc/>
        public override Color RealForegroundColor
        {
            get
            {
                return ListBox.RealForegroundColor;
            }
        }

        /// <inheritdoc/>
        public override Color RealBackgroundColor
        {
            get
            {
                return ListBox.RealBackgroundColor;
            }
        }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => ListBox.BackgroundColor;

            set
            {
                base.BackgroundColor = value;
                ListBox.BackgroundColor = value;
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get => ListBox.ForegroundColor;

            set
            {
                base.ForegroundColor = value;
                ListBox.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to select alternative style
        /// of +/- buttons and to show rotating ("twisting") arrows instead.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if twisted buttons are used;
        /// <see langword="false" />, if +/- buttons are used.
        /// The default is <see langword="true" />.
        /// </returns>
        /// <remarks>
        /// This property is an alias for <see cref="TreeButtons"/> property and
        /// was added for the compatibility with the original TreeView control.
        /// </remarks>
        [Browsable(false)]
        public virtual bool TwistButtons
        {
            get
            {
                return TreeButtons == TreeViewButtonsKind.Angle;
            }

            set
            {
                if(value)
                {
                    TreeButtons = TreeViewButtonsKind.Angle;
                }
                else
                {
                    TreeButtons = TreeViewButtonsKind.PlusMinusSquare;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of tree view buttons.
        /// </summary>
        /// <value>The type of tree view buttons.</value>
        public virtual TreeViewButtonsKind TreeButtons
        {
            get
            {
                return treeButtons;
            }

            set
            {
                if (treeButtons == value)
                    return;
                treeButtons = value;
                var (closed, opened) = KnownSvgImages.GetTreeViewButtonImages(treeButtons);
                ListBox.CheckImageUnchecked = closed;
                ListBox.CheckImageChecked = opened;
            }
        }

        /// <summary>
        /// Gets or sets the selected item in the tree view.
        /// </summary>
        /// <value>The selected <see cref="TreeViewItem"/>.</value>
        [Browsable(false)]
        public virtual TreeViewItem? SelectedItem
        {
            get
            {
                return ListBox.SelectedItem as TreeViewItem;
            }

            set
            {
                ListBox.SelectedItem = value;
            }
        }

        /// <summary>
        /// Alias for <see cref="ContextMenuStrip"/> property.
        /// </summary>
        [Browsable(false)]
        public ContextMenuStrip ContextMenu
        {
            get
            {
                return ContextMenuStrip;
            }

            set
            {
                ContextMenuStrip = value;
                ListBox.ContextMenuStrip = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return base.ContextMenuStrip;
            }

            set
            {
                base.ContextMenuStrip = value;
                ListBox.ContextMenuStrip = value;
            }
        }

        IListControlItemContainer ITreeViewItemContainer.ListContainer => ListBox;

        /// <summary>
        /// Gets or sets the root item of the tree view.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewRootItem RootItem
        {
            get
            {
                return rootItem;
            }

            set
            {
                if (rootItem == value)
                    return;
                if (value is null)
                    value = new(this);
                else
                    value.SetOwner(this);
                rootItem?.SetOwner(null);
                rootItem = value;
                RefreshTree();
            }
        }

        /// <summary>
        /// Gets the collection of visible items contained in the tree view.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<ListControlItem> VisibleItems => ListBox.Items;

        /// <inheritdoc/>
        [Browsable(false)]
        public override LayoutStyle? Layout
        {
            get => base.Layout;

            set
            {
            }
        }

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        [Browsable(false)]
        internal new Thickness? MinChildMargin
        {
            get => base.MinChildMargin;
            set => base.MinChildMargin = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        internal new float PaddingTop
        {
            get => base.PaddingTop;
            set => base.PaddingTop = value;
        }

        internal new float PaddingBottom
        {
            get => base.PaddingBottom;
            set => base.PaddingBottom = value;
        }

        internal new float PaddingLeft
        {
            get => base.PaddingLeft;
            set => base.PaddingLeft = value;
        }

        internal new float PaddingRight
        {
            get => base.PaddingRight;
            set => base.PaddingRight = value;
        }

        /// <inheritdoc/>
        public virtual Coord GetLevelMargin()
        {
            var checkSize = ListControlItem.GetCheckBoxSize(ListBox, null, null).Width;
            return checkSize;
        }

        /// <summary>
        /// Expands all parent items of the specified item.
        /// </summary>
        public virtual void ExpandAllParents(TreeViewItem? item)
        {
            if (item is null)
                return;

            BeginUpdate();
            try
            {
                item.ExpandAllParents();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as necessary.
        /// </summary>
        /// <remarks>
        /// When this method is cis called, the tree is
        /// expanded and scrolled to ensure that the current tree
        /// item is visible in the control. This method is useful
        /// if you are selecting a tree item in code based on
        /// certain criteria. By calling this method after you select the item,
        /// the user can see and interact with the selected item.
        /// </remarks>
        public virtual void EnsureVisible(TreeViewItem? item)
        {
            ScrollIntoView(item);
        }

        /// <summary>
        /// Scrolls control so the specified item will be fully visible.
        /// </summary>
        /// <param name="item">Item to show into the view.</param>
        public virtual void ScrollIntoView(TreeViewItem? item)
        {
            if (item is null)
                return;

            if (item.HasCollapsedParents)
            {
                ExpandAllParents(item);
            }

            var index = ListBox.Items.IndexOf(item);
            ListBox.EnsureVisible(index);
        }

        /// <summary>
        /// Selects the specified item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectItem(TreeViewItem? item)
        {
            item?.ExpandAllParents();
            EnsureVisible(item);
            SelectedItem = item;
        }

        /// <summary>
        /// Adds a child item to the specified parent item in the tree view.
        /// </summary>
        /// <param name="parentItem">The parent item to which the child item will be added.
        /// If null, the child item will be added to the root item.</param>
        /// <param name="childItem">The child item to add.</param>
        /// <param name="selectItem">If true, the child item will be selected after being added.</param>
        /// <returns>true if the child item was successfully added; otherwise, false.</returns>
        public virtual bool AddChild(
            UI.TreeViewItem? parentItem,
            UI.TreeViewItem childItem,
            bool selectItem = false)
        {
            if (childItem.Parent is not null || childItem.Owner is not null)
                return false;

            parentItem ??= rootItem;

            parentItem.Add(childItem);

            if (selectItem)
            {
                SelectItem(childItem);
            }

            return true;
        }

        /// <inheritdoc/>
        public override int EndUpdate()
        {
            var result = base.EndUpdate();
            if (result == 0 && needTreeChanged)
            {
                TreeChanged();
            }

            return result;
        }

        /// <inheritdoc/>
        public override int BeginUpdate()
        {
            var result = base.BeginUpdate();
            return result;
        }

        /// <summary>
        /// Clears all items from the tree view.
        /// </summary>
        public virtual void Clear()
        {
            BeginUpdate();
            try
            {
                ListBox.SetItemsFast(new(), VirtualListBox.SetItemsKind.ChangeField);
                rootItem.Clear();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Selects or clears the selection for the specified item.
        /// </summary>
        /// <param name="item">The item to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item;
        /// otherwise, <c>false</c>.</param>
        /// <remarks>
        /// You can use this method to set the selection of items in a
        /// multiple-selection tree control.
        /// To select an item in a single-selection tree control,
        /// use the <see cref="SelectedItem"/> property.
        /// </remarks>
        public void SetSelected(TreeViewItem item, bool value)
        {
            item.IsSelected = value;
        }

        /// <summary>
        /// Adds a new item with the specified title to the tree view on the root level.
        /// </summary>
        /// <param name="title">The title of the item to add.</param>
        /// <remarks>
        /// This method creates a new <see cref="TreeViewItem"/> with the specified title
        /// and adds it to the root level of the tree view.
        /// </remarks>
        public virtual TreeViewItem Add(string title)
        {
            TreeViewItem item = new(title);
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds the specified item to the tree view on the root level.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="selectItem">If true, the item will be selected after being added.</param>
        public virtual bool Add(UI.TreeViewItem item, bool selectItem = false)
        {
            return AddChild(null, item, selectItem);
        }

        /// <summary>
        /// Removes the specified item (with sub-items) from the tree view.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public virtual bool Remove(UI.TreeViewItem? item)
        {
            if (item is null)
                return false;
            if (item.Owner != this)
                return false;
            if (item.Parent is null)
                return false;
            item.Parent.Remove(item);

            return true;
        }

        /// <summary>
        /// Unselects all items in the control.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the
        /// <see cref="SelectedItem"/> property to <c>null</c> in single-selection mode.
        /// You can use this method to quickly unselect all items in the tree.
        /// </remarks>
        public virtual void ClearSelected()
        {
            ListBox.ClearSelected();
        }

        /// <summary>
        /// Removes selected items from the control.
        /// </summary>
        public virtual void RemoveSelected()
        {
            var visibleBegin = ListBox.TopIndex;

            ListBox.DoInsideUpdate(() =>
            {
                DoInsideUpdate(() =>
                {
                    var items = ListBox.SelectedItems;
                    ListBox.ClearSelected();
                    foreach (var item in items)
                    {
                        (item as TreeViewItem)?.Remove();
                    }
                });

                ListBox.ScrollToRow(visibleBegin, false);
            });
        }

        /// <summary>
        /// Removes the currently selected item from the tree view.
        /// </summary>
        public void RemoveSelectedItem()
        {
            RemoveSelectedItem(false);
        }

        /// <summary>
        /// Removes item and selects its sibling (next or previous on the same level).
        /// </summary>
        /// <param name="item">Item to remove.</param>
        public virtual void RemoveItemAndSelectSibling(TreeViewItem? item)
        {
            if (DisposingOrDisposed)
                return;
            if (item is null)
                return;
            var newItem = item.NextOrPrevSibling;
            item.Remove();
            SelectItem(newItem);
        }

        /// <summary>
        /// Removes the currently selected item from the tree view and optionally selects on
        /// of the remaining items.
        /// </summary>
        public virtual void RemoveSelectedItem(bool selectSibling)
        {
            if (selectSibling)
            {
                RemoveItemAndSelectSibling(SelectedItem);
            }
            else
            {
                Remove(SelectedItem);
            }
        }

        /// <summary>
        /// Selects the next type of tree view button.
        /// </summary>
        public void SelectNextTreeButton()
        {
            var newValue = (int)TreeButtons + 1;
            if (newValue > EnumUtils.GetMaxValueUseLastAsInt<TreeViewButtonsKind>())
                newValue = 0;
            TreeButtons = (TreeViewButtonsKind)newValue;
        }

        /// <summary>
        /// Called when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        public virtual void RaiseItemAdded(UI.TreeViewItem item)
        {
            Invoke(Internal);

            void Internal()
            {
                if (item.Parent == rootItem && !item.IsExpanded)
                {
                    if (InUpdates)
                        needTreeChanged = true;
                    else
                        ListBox.Items.Add(item);
                }
                else
                {
                    TreeChanged();
                }

                if (ItemAdded is not null)
                {
                    ItemAdded(this, new(item));
                }
            }
        }

        /// <summary>
        /// Retrieves the first column with the specified name from the collection of columns.
        /// </summary>
        /// <param name="name">The name of the column to locate.
        /// The comparison is case-sensitive and uses ordinal string comparison.</param>
        /// <returns>The first <see cref="ListControlColumn"/> with the specified name, or <see langword="null"/> if no matching
        /// column is found.</returns>
        public virtual ListControlColumn? ColumnByName(string name)
        {
            if(!HasColumns)
                return null;

            foreach (var column in Columns)
            {
                if (string.Equals(column.Name, name, StringComparison.Ordinal))
                    return column;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the columns that match the specified names.
        /// </summary>
        /// <remarks>The order of the returned columns matches the order of the provided names for columns
        /// that are found. If a name does not correspond to an existing column, it is skipped and not included in the
        /// result.</remarks>
        /// <param name="names">An array of column names to search for. Each name is matched against available columns.</param>
        /// <returns>An array of <see cref="ListControlColumn"/> objects corresponding to the specified names. The array contains
        /// only columns that were found; columns with names not found are omitted.</returns>
        public virtual ListControlColumn[] ColumnsByNames(string[] names)
        {
            List<ListControlColumn> result = new();
            foreach (var name in names)
            {
                var column = ColumnByName(name);
                if (column is not null)
                    result.Add(column);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Reorders the columns in the control to match the specified sequence.
        /// </summary>
        /// <remarks>If the control does not contain any columns, or if <paramref name="columns"/> is null
        /// or empty, this method has no effect. Columns specified that are not present in the control are
        /// ignored.</remarks>
        /// <param name="columns">An array of <see cref="ListControlColumn"/>
        /// objects that defines the desired order of columns. Only columns
        /// currently present in the control are affected. Columns not included
        /// in the array retain their relative order
        /// after the reordered columns.</param>
        public virtual void SetColumnsOrder(params ListControlColumn[] columns)
        {
            if (!HasColumns || columns is null || columns.Length == 0)
                return;

            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];

                if (column is not null && Columns.Contains(column))
                {
                    Columns.Remove(column);
                    Columns.Insert(i, column);
                }
            }
        }

        /// <summary>
        /// Sets the display order of columns based on the specified array of column names.
        /// </summary>
        /// <param name="columnNames">An array of column names that defines the desired display order.
        /// Each name should correspond to an existing column. Cannot be null.</param>
        public virtual void SetColumnsOrder(params string[] columnNames)
        {
            SetColumnsOrder(ColumnsByNames(columnNames));
        }

        /// <summary>
        /// Sets the visibility of the specified column in the control.
        /// </summary>
        /// <param name="column">The column whose visibility is to be changed. Can be null.</param>
        /// <param name="visible">true to make the column visible; false to hide the column.</param>
        /// <returns>true if the column visibility was successfully changed; otherwise, false.</returns>
        public virtual bool SetColumnVisible(ListControlColumn? column, bool visible)
        {
            if (column is null || header is null)
                return false;
            var result = Header.SetColumnVisible(column, visible);
            return result;
        }

        /// <summary>
        /// Adds a new column to the list control with the specified title, optional width, and optional click handler.
        /// This method adds a new column to <see cref="Columns"/> and also to the <see cref="Header"/>.
        /// Both the column in the collection and the header column are linked via the
        /// <see cref="ListControlColumn.ColumnKey"/> property.
        /// </summary>
        /// <remarks>The returned column is immediately added to the control and can be further customized
        /// after creation. If a width is specified, it is used as the column's suggested width. The click handler, if
        /// provided, is associated with the column header.</remarks>
        /// <param name="title">The display title of the column. Can be null to indicate an unnamed column.</param>
        /// <param name="width">The suggested width of the column, or null to use the default width.</param>
        /// <param name="onClick">An optional action to invoke when the column header is clicked.
        /// If null, no click handler is assigned.</param>
        /// <returns>A ListControlColumn instance representing the newly added column.</returns>
        public virtual ListControlColumn AddColumn(
            string? title,
            Coord width,
            Action? onClick = null)
        {
            var columnId = Header.AddColumn(
                title,
                width: width,
                onClick: onClick);

            ListControlColumn column = new(title);
            column.ColumnKey = columnId;
            column.SuggestedWidth = width;

            Columns.Add(column);

            return column;
        }

        /// <summary>
        /// Called when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public virtual void RaiseItemRemoved(UI.TreeViewItem item)
        {
            Invoke(Internal);

            void Internal()
            {
                if (item.HasItems && item.IsExpanded)
                {
                    TreeChanged();
                }
                else
                {
                    if (InUpdates)
                        needTreeChanged = true;
                    else
                        ListBox.Items?.Remove(item);
                }

                if (ItemRemoved is not null)
                {
                    ItemRemoved(this, new(item));
                }
            }
        }

        /// <summary>
        /// Called after the tree item is expanded.
        /// </summary>
        public virtual void RaiseAfterExpand(UI.TreeViewItem item)
        {
            Invoke(Internal);

            void Internal()
            {
                var selectedItems = ListBox.SelectedItems;
                TreeChanged();
                ListBox.SelectedItems = selectedItems;

                if (AfterExpand is not null)
                {
                    AfterExpand(this, new(item));
                }
            }
        }

        /// <summary>
        /// Called after the tree item is collapsed.
        /// </summary>
        public virtual void RaiseAfterCollapse(UI.TreeViewItem item)
        {
            Invoke(Internal);

            void Internal()
            {
                var selectedItems = ListBox.SelectedItems;
                TreeChanged();
                ListBox.SelectedItems = selectedItems;

                if (AfterCollapse is not null)
                {
                    AfterCollapse(this, new(item));
                }
            }
        }

        /// <summary>
        /// Called before the tree item is expanded.
        /// </summary>
        public virtual void RaiseBeforeExpand(UI.TreeViewItem item, ref bool cancel)
        {
            if (BeforeExpand is null)
                return;

            var localCancel = cancel;

            Invoke(Internal);

            void Internal()
            {
                UI.TreeViewCancelEventArgs e = new(item);
                BeforeExpand(this, e);
                localCancel = e.Cancel;
            }

            cancel = localCancel;
        }

        /// <summary>
        /// Retrieves the tree control item located at the current mouse cursor position.
        /// </summary>
        /// <returns>The <see cref="TreeViewItem"/> at the mouse cursor position,
        /// or <c>null</c> if no item is found.</returns>
        public virtual TreeViewItem? GetNodeAtMouseCursor()
        {
            return GetNodeAt(Mouse.GetPosition(ListBox));
        }

        /// <summary>
        /// Retrieves the tree view item that is at the specified point.
        /// </summary>
        /// <param name="point">
        /// The <see cref="PointD" /> to evaluate and retrieve the node from.
        /// </param>
        /// <returns>
        /// The <see cref="TreeViewItem" /> at the specified point, in client coordinates,
        /// or <see langword="null" /> if there is no item at that location.
        /// </returns>
        public virtual TreeViewItem? GetNodeAt(PointD point)
        {
            var index = ListBox.HitTest(point);
            if (index is null)
                return null;
            var item = ListBox.GetItem(index.Value);
            return item as TreeViewItem;
        }

        /// <summary>
        /// Collapses all child tree items.
        /// </summary>
        public virtual void CollapseAll()
        {
            Invoke(() =>
            {
                RootItem.CollapseAll();
            });
        }

        /// <summary>
        /// Expands all child tree items.
        /// </summary>
        public virtual void ExpandAll()
        {
            Invoke(() =>
            {
                RootItem.ExpandAll();
            });
        }

        /// <summary>
        /// Called before the tree item is collapsed.
        /// </summary>
        public virtual void RaiseBeforeCollapse(UI.TreeViewItem item, ref bool cancel)
        {
            if (BeforeCollapse is null)
                return;

            var localCancel = cancel;

            Invoke(Internal);

            void Internal()
            {
                UI.TreeViewCancelEventArgs e = new(item);
                BeforeCollapse(this, e);
                localCancel = e.Cancel;
            }

            cancel = localCancel;
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return ListBox.SetFocus();
        }

        /// <summary>
        /// Called after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public virtual void RaiseExpandedChanged(UI.TreeViewItem item)
        {
            if (ExpandedChanged is not null)
            {
                Invoke(() =>
                {
                    ExpandedChanged(this, new(item));
                });
            }
        }

        void ITreeViewItemContainer.BeginUpdate()
        {
            Invoke(() => BeginUpdate());
        }

        void ITreeViewItemContainer.EndUpdate()
        {
            Invoke(() => EndUpdate());
        }

        /// <summary>
        /// Toggles the expanded state of the specified tree item and optionally
        /// collapses its sibling items.
        /// If <paramref name="collapseSiblings"/> is <c>true</c> and the item has
        /// a parent, all sibling items are collapsed before toggling.
        /// </summary>
        /// <param name="item">The target tree item to expand or collapse.</param>
        /// <param name="collapseSiblings">
        /// Indicates whether sibling items under the same parent should be collapsed
        /// before expanding the target item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the toggle action was successfully applied; <c>false</c>
        /// if the item is <c>null</c> or has no child items.
        /// </returns>
        /// <remarks>
        /// This method is typically used during user interactions to enforce exclusive
        /// expansion behavior.
        /// It does not apply collapse logic if the item has no parent or
        /// if <paramref name="collapseSiblings"/> is <c>false</c>.
        /// </remarks>
        public virtual bool ToggleExpandedAndCollapseSiblings(
            TreeViewItem? item,
            bool collapseSiblings)
        {
            if (item is null)
                return false;
            if (!item.HasItems)
                return false;

            if (!collapseSiblings || item.Parent is null)
            {
                item.IsExpanded = !item.IsExpanded;
                return true;
            }

            DoInsideUpdate(() =>
            {
                var oldExpanded = item.IsExpanded;
                item.Parent.CollapseItems(false);
                item.IsExpanded = !oldExpanded;
            });

            return true;
        }

        /// <summary>
        /// Ensures that the child item of the root item with the specified index
        /// is visible within the tree view control.
        /// </summary>
        /// <param name="index">The zero-based index of the item to make visible.</param>
        /// <returns>
        /// <c>true</c> if the item was successfully scrolled into view; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method scrolls the tree view as necessary to bring the specified item into view.
        /// If the index is out of range or the item is already visible, no action is taken.
        /// </remarks>
        public virtual bool EnsureVisible(int index)
        {
            return ListBox.EnsureVisible(index);
        }

        /// <summary>
        /// Selects the last item in the tree view control.
        /// </summary>
        /// <remarks>
        /// This method ensures that the last item
        /// is selected. If no items are present, no action is taken.
        /// </remarks>
        public virtual void SelectLastItem()
        {
            ListBox.SelectLastItem();
        }

        /// <summary>
        /// Toggles the expanded or collapsed state of the specified tree control item.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> to toggle. If null or not
        /// a valid tree item, no action is taken.</param>
        /// <remarks>
        /// If the specified item has child items, this method switches its state
        /// between expanded and collapsed.
        /// It does not affect the state of child items.
        /// </remarks>
        public virtual bool ToggleExpanded(TreeViewItem? item)
        {
            if (item is null)
                return false;
            if (!item.HasItems)
                return false;

            item.IsExpanded = !item.IsExpanded;
            return true;
        }

        /// <summary>
        /// Expands or collapses the specified <see cref="TreeViewItem"/> while preserving
        /// the current scroll position and selection.
        /// </summary>
        /// <remarks>This method ensures that the scroll position and the selected item in the associated
        /// <see cref="ListBox"/> remain unchanged after the operation.</remarks>
        /// <param name="item">The <see cref="TreeViewItem"/> to expand or collapse.
        /// If <paramref name="item"/> is <c>null</c> or does not
        /// have child items, the method does nothing.</param>
        /// <param name="expanded"><see langword="true"/> to expand the item;
        /// <see langword="false"/> to collapse it.</param>
        public virtual void SetExpandedAndKeepPosAndSelection(TreeViewItem? item, bool expanded)
        {
            if(item is null)
                return;

            Post(() =>
            {
                var visibleBegin = ListBox.TopIndex;

                ListBox.DoInsideUpdate(() =>
                {
                    DoInsideUpdate(() =>
                    {
                        item.IsExpanded = expanded;
                    });

                    ListBox.ScrollToRow(visibleBegin, false);
                    ListBox.SelectedItem = item;
                });
            });
        }

        /// <summary>
        /// Toggles the expanded state of the specified <see cref="TreeViewItem"/> while preserving
        /// the current scroll position and selection.
        /// </summary>
        /// <remarks>This method ensures that the scroll position and selection in the inner
        /// <see cref="ListBox"/> remain unchanged after toggling the expanded state.
        /// If the <paramref name="item"/> has the <c>AutoCollapseSiblings</c> property set
        /// to <see langword="true"/>, sibling items will be collapsed.</remarks>
        /// <param name="item">The <see cref="TreeViewItem"/> to toggle.
        /// If <c>null</c>, no action is performed.</param>
        public virtual void ToggleExpandedAndKeepPosAndSelection(TreeViewItem? item)
        {
            if (item is null)
                return;

            Post(() =>
            {
                var visibleBegin = ListBox.TopIndex;

                ListBox.DoInsideUpdate(() =>
                {
                    DoInsideUpdate(() =>
                    {
                        ToggleExpandedAndCollapseSiblings(item, item?.AutoCollapseSiblings ?? false);
                    });

                    ListBox.ScrollToRow(visibleBegin, false);
                    ListBox.SelectedItem = item;
                });
            });
        }

        /// <summary>
        /// Removes all items from the tree view control.
        /// </summary>
        /// <remarks>
        /// This method clears the tree view by calling the <see cref="Clear"/> method,
        /// which removes all items and resets the tree structure.
        /// </remarks>
        public void RemoveAll()
        {
            Clear();
        }

        /// <summary>
        /// Selects the first item in the tree view control.
        /// </summary>
        /// <remarks>
        /// This method ensures that the first item
        /// is selected. If no items are present, no action is taken.
        /// </remarks>
        public virtual void SelectFirstItem()
        {
            ListBox.SelectFirstItem();
        }

        /// <summary>
        /// Changes visual style of the control to look like <see cref="ListBox"/>.
        /// </summary>
        public virtual void MakeAsListBox()
        {
            if (DisposingOrDisposed)
                return;
        }

        /// <summary>
        /// Adds a separator item to the tree control.
        /// </summary>
        /// <remarks>The separator item is used to visually divide groups of items within the tree
        /// control.</remarks>
        /// <returns>A <see cref="TreeViewItem"/> representing the added separator.</returns>
        public virtual TreeViewItem AddSeparator()
        {
            TreeViewSeparatorItem item = new();
            Add(item);
            return item;
        }

        /// <summary>
        /// Selects the first item in the tree view control and scrolls to it.
        /// </summary>
        /// <remarks>
        /// This method ensures that the first item is selected and visible.
        /// If no items are present, no action is taken.
        /// </remarks>
        public virtual void SelectFirstItemAndScroll()
        {
            ListBox.SelectFirstItemAndScroll();
        }

        void ITreeViewItemContainer.RaiseItemPropertyChanged(TreeViewItem item, string? propertyName)
        {
            OnItemPropertyChanged(item, propertyName);
            ItemPropertyChanged?.Invoke(this, new TreeViewItemPropChangedEventArgs(item, propertyName));
        }

        void ITreeViewItemContainer.RaiseItemSelectedChanged(TreeViewItem item, bool selected)
        {
            OnItemSelectedChanged(item, selected);
            ItemSelectedChanged?.Invoke(this, new TreeViewEventArgs(item));
        }

        /// <summary>
        /// Selects the last item in the tree view control and scrolls to it.
        /// </summary>
        /// <remarks>
        /// This method ensures that the last item is selected and visible.
        /// If no items are present, no action is taken.
        /// </remarks>
        public virtual void SelectLastItemAndScroll()
        {
            ListBox.SelectLastItemAndScroll();
        }

        /// <summary>
        /// Updates the tree view when the tree structure changes.
        /// </summary>
        public virtual void TreeChanged()
        {
            Invoke(Internal);

            void Internal()
            {
                if (InUpdates)
                {
                    needTreeChanged = true;
                    return;
                }
                else
                {
                    needTreeChanged = false;
                }

                Changed?.Invoke(this, EventArgs.Empty);

                RefreshTree();
            }
        }

        /// <summary>
        /// Applies a visibility filter to items within the tree control,
        /// updating their <c>IsVisible</c> property
        /// based on a text-matching condition.
        /// </summary>
        /// <param name="filter">
        /// The text used to determine item visibility. If <c>null</c> or empty,
        /// all items will be visible.
        /// </param>
        /// <param name="comparison">
        /// The <see cref="StringComparison"/> strategy used to compare item text
        /// against the filter.
        /// Defaults to <see cref="StringComparison.CurrentCultureIgnoreCase"/>.
        /// </param>
        /// <remarks>
        /// This method enumerates all items (including those not currently visible),
        /// extracts their textual representation
        /// and sets each item's <c>IsVisible</c> flag accordingly.
        /// The update is wrapped in <see cref="TreeViewItem.DoInsideUpdate"/>
        /// to optimize rendering and batch changes.
        /// </remarks>
        /// <param name="prm">The parameters used to control item enumeration.
        /// Optional. If not specified, default parameters will be used (all items
        /// are processed regardless of visibility or expansion state).</param>
        public virtual void ApplyVisibilityFilter(
            string? filter,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase,
            TreeViewItem.EnumExpandedItemsParams? prm = null)
        {
            if (prm is null)
            {
                prm = new();
                prm.OnlyVisible = false;
                prm.OnlyExpanded = false;
            }

            var items = RootItem.EnumExpandedItems(prm);

            RootItem.DoInsideUpdate(() =>
            {
                foreach (var item in items)
                {
                    item.IsVisible = ItemContainsText(item, filter, comparison);
                }
            });
        }

        /// <summary>
        /// Resets the cached images for all items in the collection.
        /// </summary>
        /// <remarks>This method iterates through all items in the collection and invokes their
        /// <see cref="ListControlItem.ResetCachedImages"/> method to clear any
        /// cached image data. Use this method to ensure that all items
        /// refresh their cached images,  for example, after an update
        /// to the underlying image source.</remarks>
        public virtual void ResetCachedImagesInItems()
        {
            foreach (var item in Items)
            {
                item.ResetCachedImages();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified item contains the specified substring.
        /// </summary>
        /// <param name="item">The tree view item to check.</param>
        /// <param name="subString">The substring to search for.</param>
        /// <param name="comparison">The string comparison options.</param>
        /// <returns></returns>
        public virtual bool ItemContainsText(
            TreeViewItem? item,
            string? subString,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (item is null)
                return false;
            if(string.IsNullOrEmpty(subString))
                return true;
            var txt = ListBox.GetItemText(item, true);
            return txt.Contains(subString!, comparison);
        }

        /// <summary>
        /// Applies a visibility to all items in the tree control,
        /// updating their <c>IsVisible</c> property.
        /// </summary>
        /// <param name="value">The visibility value to apply.</param>
        /// <param name="prm">The parameters used to control item enumeration.
        /// Optional. If not specified, default parameters will be used (all items
        /// are processed regardless of visibility or expansion state).</param>
        public virtual void ApplyVisibility(
            bool value,
            TreeViewItem.EnumExpandedItemsParams? prm = null)
        {
            if (prm is null)
            {
                prm = new();
                prm.OnlyVisible = false;
                prm.OnlyExpanded = false;
            }

            var items = RootItem.EnumExpandedItems(prm);

            RootItem.DoInsideUpdate(() =>
            {
                foreach (var item in items)
                {
                    item.IsVisible = value;
                }
            });
        }

        /// <summary>
        /// Applies a visibility filter to items within the tree control,
        /// updating their <c>IsVisible</c> property
        /// based on a predicate condition.
        /// </summary>
        /// <param name="filter">The predicate used to determine item visibility.</param>
        /// <param name="prm">The parameters used to control item enumeration.
        /// Optional. If not specified, default parameters will be used (all items
        /// are processed regardless of visibility or expansion state).</param>
        public virtual void ApplyVisibilityFilter(
            Predicate<TreeViewItem> filter,
            TreeViewItem.EnumExpandedItemsParams? prm = null)
        {
            if(prm is null)
            {
                prm = new();
                prm.OnlyVisible = false;
                prm.OnlyExpanded = false;
            }

            var items = RootItem.EnumExpandedItems(prm);

            RootItem.DoInsideUpdate(() =>
            {
                foreach (var item in items)
                {
                    item.IsVisible = filter(item);
                }
            });
        }

        /// <summary>
        /// Refreshes the tree view by updating the list of visible items
        /// based on the current state of the tree structure.
        /// </summary>
        /// <remarks>
        /// This method regenerates the collection of items displayed in the
        /// control by enumerating the expanded items
        /// from the root of the tree. It ensures that the visual representation
        /// of the tree matches its logical structure.
        /// </remarks>
        protected virtual void RefreshTree()
        {
            Invoke(() =>
            {
                List<ListControlItem> list = new(rootItem.EnumExpandedItems());
                NotNullCollection<ListControlItem> collection = new(list);
                ListBox.SetItemsFast(collection, VirtualListBox.SetItemsKind.ChangeField);
            });
        }

        /// <summary>
        /// Handles the <see cref="AbstractControl.MouseDown"/> event for the inner <see cref="ListBox"/>.
        /// </summary>
        /// <remarks>By default, this method determines whether the mouse click occurred on a checkbox
        /// or an item in the inner <see cref="ListBox"/>.
        /// If the click is on a checkbox or the item supports expansion on click, the item's
        /// expanded state is toggled.</remarks>
        /// <param name="sender">The source of the event, typically the <see cref="ListBox"/>.</param>
        /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected virtual void OnListBoxMouseDown(object? sender, MouseEventArgs e)
        {
            var isOnCheckBox = false;

            var itemIndex = ListBox.HitTest(e.Location);
            if (itemIndex is null)
                return;

            if (ListBox.HitTestCheckBox(e.Location) is not null)
                isOnCheckBox = true;

            var item = ListBox.Items[itemIndex] as TreeViewItem;

            if (item is not null && (isOnCheckBox || item.ExpandOnClick))
            {
                ToggleExpandedAndKeepPosAndSelection(item);
            }
        }

        /// <summary>
        /// Handles the behavior when the left arrow key in the inner list box.
        /// </summary>
        /// <remarks>If the currently selected item is expanded and has child items, this method collapses
        /// the item. If the selected item is not expandable or already collapsed, the parent item
        /// (if available) is selected instead. The event is marked as handled in both cases.</remarks>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnListBoxLeftKeyDown(KeyEventArgs e)
        {
            var item = SelectedItem;
            if (item is not null && item.HasItems && item.IsExpanded)
            {
                SetExpandedAndKeepPosAndSelection(item, false);
                e.Suppressed();
            }
            else
            if (item?.Parent is not null && item.Parent.Parent is not null)
            {
                SelectItem(item.Parent);
                e.Suppressed();
            }
        }

        /// <summary>
        /// Handles the right arrow key press event for the inner list box,
        /// expanding the selected item if it has child items.
        /// </summary>
        /// <remarks>If the currently selected item has child items and is not already expanded, this
        /// method expands the item and suppresses further processing
        /// of the key event. This method is intended to be
        /// overridden in derived classes to customize the behavior of the right arrow key press.</remarks>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnListBoxRightKeyDown(KeyEventArgs e)
        {
            var item = SelectedItem;

            if (item is not null && item.HasItems && !item.IsExpanded)
            {
                SetExpandedAndKeepPosAndSelection(item, true);
                e.Suppressed();
            }
        }

        /// <summary>
        /// Handles the behavior when the asterisk (*) key is pressed in the inner list box.
        /// </summary>
        /// <remarks>This method expands all child items of the currently selected item in the list box,
        /// if one is selected. The event is suppressed to prevent
        /// further processing of the key press.</remarks>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data,
        /// including the key pressed and its state.</param>
        protected virtual void OnListBoxAsteriskKeyDown(KeyEventArgs e)
        {
            var item = SelectedItem;
            if (item is not null)
            {
                item.ExpandAll();
                e.Suppressed();
            }
        }

        /// <summary>
        /// Handles the <see cref="AbstractControl.KeyDown"/> event for the inner list box.
        /// </summary>
        /// <remarks>This method is invoked when a key is pressed while the inner list box has focus.
        /// Use this method to implement custom key handling logic.</remarks>
        /// <param name="sender">The source of the event, typically
        /// the list box that triggered the event.</param>
        /// <param name="e">A <see cref="KeyEventArgs"/> that contains the event data,
        /// including the key pressed.</param>
        protected virtual void OnListBoxKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.NumPadPlus:
                case Key.PlusSign:
                    OnListBoxRightKeyDown(e);
                    break;
                case Key.Left:
                case Key.Minus:
                case Key.NumPadMinus:
                    OnListBoxLeftKeyDown(e);
                    break;
                case Key.NumPadStar:
                case Key.Asterisk:
                    OnListBoxAsteriskKeyDown(e);
                    break;
                case Key.E:
                    if (e.ControlAndShift)
                    {
                        ExpandAll();
                        e.Suppressed();
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the double-click event on the inner list box, toggling the expanded state
        /// of the selected item while preserving its position and selection.
        /// </summary>
        /// <remarks>This method is called when a double-click event occurs on the inner list box. Derived
        /// classes can override this method to provide custom handling for
        /// the double-click event. When overriding,
        /// ensure the base method is called to maintain the default behavior.</remarks>
        /// <param name="sender">The source of the event, typically the list box
        /// that was double-clicked.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnListBoxDoubleClick(object? sender, EventArgs e)
        {
            ToggleExpandedAndKeepPosAndSelection(SelectedItem);
        }

        /// <summary>
        /// Determines whether the tree view should be invalidated
        /// when a property of an item changes.
        /// </summary>
        /// <param name="item">The tree item whose property changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <returns></returns>
        protected virtual bool InvalidateWhenItemPropertyChanged(
            TreeViewItem item,
            string? propertyName)
        {
            return ItemPropertyNamesToRaiseInvalidate.Contains(propertyName);
        }

        /// <summary>
        /// Called when the selection state of a tree item changes.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> whose selection state changed.</param>
        /// <param name="selected">Indicates whether the item is selected.</param>
        protected virtual void OnItemSelectedChanged(TreeViewItem item, bool selected)
        {
            if (!TrackItemSelectedChanges)
                return;
            if (!ListBox.ItemsLastPainted.Contains(item))
                return;
            ListBox.Invalidate();
        }

        /// <summary>
        /// Called when a property of a tree item changes.
        /// </summary>
        /// <param name="item">The tree item whose property changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnItemPropertyChanged(TreeViewItem item, string? propertyName)
        {
            if(!TrackItemPropertyChanges)
                return;
            if (string.IsNullOrEmpty(propertyName))
                return;
            if (!ListBox.ItemsLastPainted.Contains(item))
                return;
            if(InvalidateWhenItemPropertyChanged(item, propertyName))
            {
                ListBox.Invalidate();
            }
        }

        /// <summary>
        /// Raises the event that occurs when the visibility of a header column changes.
        /// </summary>
        /// <param name="sender">The source of the event, typically the control whose header column visibility has changed.</param>
        /// <param name="e">An object containing data related to the header column visibility change.</param>
        protected virtual void OnHeaderColumnVisibleChanged(object? sender, ListBoxHeader.ColumnEventArgs e)
        {
            if (!HasColumns)
                return;

            var newVisible = e.Column.Visible;
            var id = e.Column.UniqueId;

            foreach (var col in ListBox.Columns)
            {
                if (col.ColumnKey == id)
                {
                    var oldVisible = col.IsVisible;
                    col.IsVisible = newVisible;

                    if (oldVisible != newVisible)
                    {
                        ListBox.Invalidate();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Handles the event that occurs when the size of a header column changes.
        /// </summary>
        /// <remarks>Override this method to provide custom handling when a header column's size is
        /// modified. This method is called when the column size change event is raised.</remarks>
        /// <param name="sender">The source of the event, typically the header control whose column size has changed.</param>
        /// <param name="e">A <see cref="ListBoxHeader.ColumnEventArgs"/> that contains the event data for the column size change.</param>
        protected virtual void OnHeaderColumnSizeChanged(object? sender, ListBoxHeader.ColumnEventArgs e)
        {
            if (!HasColumns)
                return;

            var newWidth = e.Column.Width;
            var id = e.Column.UniqueId;

            foreach (var col in ListBox.Columns)
            {
                if (col.ColumnKey == id)
                {
                    var oldWidth = col.SuggestedWidth;
                    col.SuggestedWidth = newWidth;

                    if (oldWidth != newWidth)                    
                    {
                        ListBox.Invalidate();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Raises the event that occurs when a column is inserted into the header.
        /// </summary>
        /// <remarks>Override this method to provide custom handling when a column is inserted into the
        /// header. This method is called after a column has been added and can be used to perform additional processing
        /// or to raise related events.</remarks>
        /// <param name="sender">The source of the event, typically the control that raised the event.</param>
        /// <param name="e">A <see cref="ListBoxHeader.ColumnEventArgs"/> that contains the event data for the inserted column.</param>
        protected virtual void OnHeaderColumnInserted(object? sender, ListBoxHeader.ColumnEventArgs e)
        {
        }

        /// <summary>
        /// Raises the event that occurs when a header column is deleted from the list box.
        /// </summary>
        /// <remarks>Override this method to provide custom handling when a header column is deleted. This
        /// method is called after a column has been removed from the header.</remarks>
        /// <param name="sender">The source of the event, typically the list box control.</param>
        /// <param name="e">A <see cref="ListBoxHeader.ColumnEventArgs"/> that contains the event data for the deleted column.</param>
        protected virtual void OnHeaderColumnDeleted(object? sender, ListBoxHeader.ColumnEventArgs e)
        {
            if (!HasColumns)
                return;
        }

        /// <inheritdoc/>
        protected override bool GetDefaultParentBackColor()
        {
            return false;
        }

        /// <inheritdoc/>
        protected override bool GetDefaultParentForeColor()
        {
            return false;
        }

        /// <inheritdoc/>
        protected override void OnContextMenuCreated(EventArgs e)
        {
            ListBox.ContextMenuStrip = ContextMenu;
        }

        /// <summary>
        /// Creates and returns a new instance of a <see cref="ListBoxHeader"/>.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes to provide a custom
        /// implementation of the <see cref="ListBoxHeader"/>. By default, it returns
        /// a new instance of <see cref="ListBoxHeader"/>.</remarks>
        /// <returns>A new instance of <see cref="ListBoxHeader"/>.</returns>
        protected virtual ListBoxHeader CreateHeader()
        {
            return new ListBoxHeader();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="VirtualListBox"/> used by the tree control.
        /// </summary>
        /// <remarks>
        /// This method can be overridden in derived classes to provide a custom implementation
        /// of the <see cref="VirtualListBox"/>. By default, it returns a standard instance
        /// of <see cref="VirtualListBox"/>.
        /// </remarks>
        /// <returns>A new instance of <see cref="VirtualListBox"/>.</returns>
        protected virtual VirtualListBox CreateListBox()
        {
            return new VirtualListBox();
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            ResetCachedImagesInItems();
            base.OnSystemColorsChanged(e);
        }
    }
}
