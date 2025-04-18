﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a tree control that inherits from <see cref="VirtualListBox"/>.
    /// </summary>
    public partial class VirtualTreeControl : Control, ITreeControlItemContainer
    {
        /// <summary>
        /// Specifies the default type of buttons used in the tree view control to expand or collapse nodes.
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

        private readonly VirtualListBox listBox;
        private TreeControlRootItem rootItem;
        private TreeViewButtonsKind treeButtons = TreeViewButtonsKind.Null;
        private bool needTreeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeControl"/> class.
        /// </summary>
        public VirtualTreeControl()
        {
            listBox = CreateListBox();
            ListBox.ParentFont = true;
            ListBox.HasBorder = false;
            ListBox.Parent = this;
            rootItem = new(this);
            ListBox.SelectionUnderImage = true;
            ListBox.CheckBoxVisible = true;
            ListBox.CheckOnClick = false;

            ListBox.MouseDown += (s, e) =>
            {
                var itemIndex = ListBox.HitTestCheckBox(e.Location);
                if (itemIndex is null)
                    return;
                ToggleExpanded(ListBox.Items[itemIndex.Value] as TreeControlItem);
            };

            ListBox.DoubleClick += (s, e) =>
            {
                ToggleExpanded(SelectedItem);
            };

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
        /// Gets the underlying <see cref="VirtualListBox"/> used by this tree control.
        /// </summary>
        public VirtualListBox ListBox => listBox;

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

        /// <inheritdoc/>
        public override ContextMenuStrip ContextMenuStrip
        {
            get => ListBox.ContextMenuStrip;
            set => ListBox.ContextMenuStrip = value;
        }

        /// <summary>
        /// Alias for <see cref="ContextMenuStrip"/> property.
        /// </summary>
        [Browsable(false)]
        public ContextMenuStrip ContextMenu
        {
            get => ListBox.ContextMenuStrip;
            set => ListBox.ContextMenuStrip = value;
        }

        /// <summary>
        /// Gets or sets the root item of the tree view.
        /// </summary>
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
        public IEnumerable<ListControlItem> VisibleItems => ListBox.Items;

        /// <summary>
        /// Selects the specified item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectItem(UI.TreeControlItem? item)
        {
            if (item is null)
                return;

            bool InternalSelect()
            {
                var index = ListBox.Items.IndexOf(item);

                if (index > 0)
                {
                    SelectedItem = item;
                    ListBox.ScrollToRow(index);
                    return true;
                }

                return false;
            }

            if (InternalSelect())
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

            InternalSelect();
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
        /// Adds the specified item to the tree view on the root level.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="selectItem">If true, the item will be selected after being added.</param>
        public bool Add(UI.TreeControlItem item, bool selectItem = false)
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
        /// Removes the currently selected item from the tree view and optionally selects on
        /// of the remaining items.
        /// </summary>
        public virtual bool RemoveSelectedItem(bool selectSibling)
        {
            var item = SelectedItem as TreeControlItem;
            if (item is null)
                return false;

            if (selectSibling)
            {
                var index = ListBox.Items.IndexOf(item);
                var result = ListBox.Items.Remove(item);
                if (result)
                {
                    if (index > 0)
                        index--;
                    index = Math.Min(ListBox.Items.Count - 1, index);
                    if (index >= 0)
                    {
                        item = ListBox.Items[index] as TreeControlItem;
                        SelectItem(item);
                    }
                }

                return result;
            }
            else
            {
                return Remove(item);
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
            if (item.Parent == rootItem && !item.IsExpanded)
            {
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

        /// <summary>
        /// Called when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        public virtual void RaiseItemRemoved(UI.TreeControlItem item)
        {
            if (item.HasItems && item.IsExpanded)
            {
                TreeChanged();
            }
            else
            {
                ListBox.Items?.Remove(item);
            }

            if (ItemRemoved is not null)
            {
                ItemRemoved(this, new(item));
            }
        }

        /// <summary>
        /// Called after the tree item is expanded.
        /// </summary>
        public virtual void RaiseAfterExpand(UI.TreeControlItem item)
        {
            TreeChanged();

            if (AfterExpand is not null)
            {
                AfterExpand(this, new(item));
            }
        }

        /// <summary>
        /// Called after the tree item is collapsed.
        /// </summary>
        public virtual void RaiseAfterCollapse(UI.TreeControlItem item)
        {
            TreeChanged();

            if (AfterCollapse is not null)
            {
                AfterCollapse(this, new(item));
            }
        }

        /// <summary>
        /// Called before the tree item is expanded.
        /// </summary>
        public virtual void RaiseBeforeExpand(UI.TreeControlItem item, ref bool cancel)
        {
            if (BeforeExpand is not null)
            {
                UI.TreeControlCancelEventArgs e = new(item);
                BeforeExpand(this, e);
                cancel = e.Cancel;
            }
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
            if (BeforeCollapse is not null)
            {
                UI.TreeControlCancelEventArgs e = new(item);
                BeforeCollapse(this, e);
                cancel = e.Cancel;
            }
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
                ExpandedChanged(this, new(item));
            }
        }

        void ITreeControlItemContainer.BeginUpdate()
        {
            this.BeginUpdate();
        }

        void ITreeControlItemContainer.EndUpdate()
        {
            this.EndUpdate();
        }

        /// <summary>
        /// Toggles the expanded or collapsed state of the specified tree control item.
        /// </summary>
        /// <param name="treeItem">The <see cref="TreeControlItem"/> to toggle. If null or not
        /// a valid tree item, no action is taken.</param>
        /// <remarks>
        /// If the specified item has child items, this method switches its state between expanded and collapsed.
        /// It does not affect the state of child items.
        /// </remarks>
        public virtual void ToggleExpanded(TreeControlItem? treeItem)
        {
            if (treeItem is not TreeControlItem item)
                return;
            if (!item.HasItems)
                return;
            item.IsExpanded = !item.IsExpanded;
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
            VirtualListBoxItems collection = new(rootItem.EnumExpandedItems());
            ListBox.SetItemsFast(collection, VirtualListBox.SetItemsKind.ChangeField);
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
