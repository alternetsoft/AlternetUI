using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items with optional images,
    /// each represented by a <see cref="TreeControlItem"/>. This control uses
    /// internal <see cref="VirtualListControl"/> for displaying items, it can be accessed via
    /// <see cref="ListBox"/> property.
    /// </summary>
    public partial class VirtualTreeControl : Border, ITreeControlItemContainer
    {
        /// <summary>
        /// Specifies the default type of buttons used in the tree view control to expand
        /// or collapse nodes.
        /// </summary>
        /// <remarks>
        /// This field determines the initial value of the <see cref="TreeButtons"/> property
        /// for instances of <see cref="VirtualTreeControl"/>.
        /// The default value is <see cref="TreeViewButtonsKind.Angle"/>.
        /// </remarks>
        public static TreeViewButtonsKind DefaultTreeButtons = TreeViewButtonsKind.Angle;

        /// <summary>
        /// Default margin for each level in the tree.
        /// </summary>
        public static int DefaultLevelMargin = 16;

        private VirtualListBox? listBox;
        private TreeControlRootItem rootItem;
        private TreeViewButtonsKind treeButtons = TreeViewButtonsKind.Null;
        private bool needTreeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeControl"/> class.
        /// </summary>
        public VirtualTreeControl()
        {
            rootItem = new(this);

            ListBox.MouseDown += (s, e) =>
            {
                var isOnCheckBox = false;

                var itemIndex = ListBox.HitTest(e.Location);
                if (itemIndex is null)
                    return;

                if (ListBox.HitTestCheckBox(e.Location) is not null)
                    isOnCheckBox = true;

                var item = ListBox.Items[itemIndex] as TreeControlItem;

                if (item is not null && (isOnCheckBox || item.ExpandOnClick))
                {
                    ToggleExpanded(item);
                }
            };

            ListBox.DoubleClick += (s, e) =>
            {
                ToggleExpanded(SelectedItem);
            };

            TreeButtons = DefaultTreeButtons;

            void ToggleExpanded(TreeControlItem? item)
            {
                Invoke(() =>
                {
                    var visibleBegin = ListBox.TopIndex;

                    ListBox.DoInsideUpdate(() =>
                    {
                        DoInsideUpdate(() =>
                        {
                            ToggleExpandedAndCollapseSiblings(item, item?.AutoCollapseSiblings ?? false);
                            ListBox.SelectedItem = item;
                        });

                        ListBox.ScrollToRow(visibleBegin, false);
                    });
                });
            }
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
        /// Occurs when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs before the tree item is expanded. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeControlCancelEventArgs>? BeforeExpand;

        /// <summary>
        /// Occurs before the tree item is collapsed. This event can be canceled.
        /// </summary>
        public event EventHandler<UI.TreeControlCancelEventArgs>? BeforeCollapse;

        /// <summary>
        /// Occurs after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        public event EventHandler<UI.TreeControlEventArgs>? ExpandedChanged;

        /// <summary>
        /// Gets or sets the selection mode (single or multiple).
        /// </summary>
        /// <remarks>The selection mode determines whether multiple items can be selected
        /// at once and how the selection behaves.
        /// For example, <see cref="ListBoxSelectionMode.Single"/> allows only one item to be
        /// selected, while  <see cref="ListBoxSelectionMode.Multiple"/> allows multiple
        /// items to be selected.</remarks>
        public ListBoxSelectionMode SelectionMode
        {
            get
            {
                return ListBox.SelectionMode;
            }

            set
            {
                ListBox.SelectionMode = value;
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="VirtualListBox"/> used by this tree control.
        /// </summary>
        [Browsable(false)]
        public virtual VirtualListBox ListBox
        {
            get
            {
                if(listBox is null)
                {
                    listBox = CreateListBox();
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
        /// <value>The selected <see cref="TreeControlItem"/>.</value>
        [Browsable(false)]
        public virtual TreeControlItem? SelectedItem
        {
            get
            {
                return ListBox.SelectedItem as TreeControlItem;
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

        IListControlItemContainer ITreeControlItemContainer.ListContainer => ListBox;

        /// <summary>
        /// Gets or sets the root item of the tree view.
        /// </summary>
        [Browsable(false)]
        public virtual TreeControlRootItem RootItem
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

        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        internal new Thickness? MinChildMargin
        {
            get => base.MinChildMargin;
            set => base.MinChildMargin = value;
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
        public virtual void ExpandAllParents(TreeControlItem? item)
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
        /// Scrolls control so the specified item will be fully visible.
        /// </summary>
        /// <param name="item">Item to show into the view.</param>
        public virtual void ScrollIntoView(TreeControlItem? item)
        {
            if (item is null)
                return;

            if (item.HasCollapsedParents)
            {
                ExpandAllParents(item);
            }
            else
            {
                var index = ListBox.Items.IndexOf(item);
                ListBox.EnsureVisible(index);
            }
        }

        /// <summary>
        /// Selects the specified item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectItem(TreeControlItem? item)
        {
            item?.ExpandAllParents();
            ScrollIntoView(item);
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
            UI.TreeControlItem? parentItem,
            UI.TreeControlItem childItem,
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
        /// Adds a new item with the specified title to the tree view on the root level.
        /// </summary>
        /// <param name="title">The title of the item to add.</param>
        /// <remarks>
        /// This method creates a new <see cref="TreeControlItem"/> with the specified title
        /// and adds it to the root level of the tree view.
        /// </remarks>
        public virtual TreeControlItem Add(string title)
        {
            TreeControlItem item = new(title);
            Add(item);
            return item;
        }

        /// <summary>
        /// Adds the specified item to the tree view on the root level.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="selectItem">If true, the item will be selected after being added.</param>
        public virtual bool Add(UI.TreeControlItem item, bool selectItem = false)
        {
            return AddChild(null, item, selectItem);
        }

        /// <summary>
        /// Removes the specified item (with sub-items) from the tree view.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public virtual bool Remove(UI.TreeControlItem? item)
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
        public virtual void RemoveItemAndSelectSibling(TreeControlItem? item)
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
        public virtual void RaiseItemAdded(UI.TreeControlItem item)
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
        /// Called when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public virtual void RaiseItemRemoved(UI.TreeControlItem item)
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
        public virtual void RaiseAfterExpand(UI.TreeControlItem item)
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
        public virtual void RaiseAfterCollapse(UI.TreeControlItem item)
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
        public virtual void RaiseBeforeExpand(UI.TreeControlItem item, ref bool cancel)
        {
            if (BeforeExpand is null)
                return;

            var localCancel = cancel;

            Invoke(Internal);

            void Internal()
            {
                UI.TreeControlCancelEventArgs e = new(item);
                BeforeExpand(this, e);
                localCancel = e.Cancel;
            }

            cancel = localCancel;
        }

        /// <summary>
        /// Retrieves the tree control item located at the current mouse cursor position.
        /// </summary>
        /// <returns>The <see cref="TreeControlItem"/> at the mouse cursor position,
        /// or <c>null</c> if no item is found.</returns>
        public virtual TreeControlItem? GetNodeAtMouseCursor()
        {
            var index = ListBox.HitTest(Mouse.GetPosition(ListBox));
            if (index is null)
                return null;
            var item = ListBox.GetItem(index.Value);
            return item as TreeControlItem;
        }

        /// <summary>
        /// Called before the tree item is collapsed.
        /// </summary>
        public virtual void RaiseBeforeCollapse(UI.TreeControlItem item, ref bool cancel)
        {
            if (BeforeCollapse is null)
                return;

            var localCancel = cancel;

            Invoke(Internal);

            void Internal()
            {
                UI.TreeControlCancelEventArgs e = new(item);
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
        public virtual void RaiseExpandedChanged(UI.TreeControlItem item)
        {
            if (ExpandedChanged is not null)
            {
                Invoke(() =>
                {
                    ExpandedChanged(this, new(item));
                });
            }
        }

        void ITreeControlItemContainer.BeginUpdate()
        {
            Invoke(() => BeginUpdate());
        }

        void ITreeControlItemContainer.EndUpdate()
        {
            Invoke(() => EndUpdate());
        }

        /// <summary>
        /// Toggles the expanded or collapsed state of the specified tree control item.
        /// </summary>
        /// <param name="item">The <see cref="TreeControlItem"/> to toggle. If null or not
        /// a valid tree item, no action is taken.</param>
        /// <remarks>
        /// If the specified item has child items, this method switches its state
        /// between expanded and collapsed.
        /// It does not affect the state of child items.
        /// </remarks>
        public virtual bool ToggleExpanded(TreeControlItem? item)
        {
            if (item is null)
                return false;
            if (!item.HasItems)
                return false;

            item.IsExpanded = !item.IsExpanded;
            return true;
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
            TreeControlItem? item,
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
        /// The update is wrapped in <see cref="TreeControlItem.DoInsideUpdate"/>
        /// to optimize rendering and batch changes.
        /// </remarks>
        public virtual void ApplyVisibilityFilter(
            string? filter,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            TreeControlItem.EnumExpandedItemsParams prm = new();
            prm.OnlyVisible = false;
            prm.OnlyExpanded = false;

            var items = RootItem.EnumExpandedItems(prm);

            var noFilter = string.IsNullOrEmpty(filter);
            filter ??= string.Empty;

            RootItem.DoInsideUpdate(() =>
            {
                foreach (var item in items)
                {
                    var txt = ListBox.GetItemText(item, true);
                    item.IsVisible = noFilter || txt.Contains(filter, comparison);
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
    }
}
