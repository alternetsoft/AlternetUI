using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a tree control that inherits from <see cref="VirtualListBox"/>.
    /// This control is under development, please do not use it.
    /// </summary>
    public partial class VirtualTreeControl : VirtualListBox, ITreeControlItemContainer
    {
        /// <summary>
        /// Default margin for each level in the tree.
        /// </summary>
        public static int DefaultLevelMargin = 16;

        private readonly TreeControlRootItem rootItem;
        private TreeViewButtonsKind treeButtons = TreeViewButtonsKind.Null;
        private bool needTreeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeControl"/> class.
        /// </summary>
        public VirtualTreeControl()
        {
            rootItem = new(this);
            SelectionUnderImage = true;
            CheckBoxVisible = true;
            CheckOnClick = false;

            void ToggleExpanded(ListControlItem? listItem)
            {
                if (listItem is not TreeControlItem item)
                    return;
                if (!item.HasItems)
                    return;
                item.IsExpanded = !item.IsExpanded;
            }

            MouseDown += (s, e) =>
            {
                var itemIndex = HitTestCheckBox(e.Location);
                if (itemIndex is null)
                    return;
                ToggleExpanded(Items[itemIndex.Value]);
            };

            DoubleClick += (s, e) =>
            {
                ToggleExpanded(SelectedItem);
            };

            TreeButtons = TreeViewButtonsKind.PlusMinusSquare;
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
        /// Gets or sets the type of tree view buttons.
        /// </summary>
        /// <value>The type of tree view buttons.</value>
        public TreeViewButtonsKind TreeButtons
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
                DoInsideUpdate(() =>
                {
                    CheckImageUnchecked = closed;
                    CheckImageChecked = opened;
                });
            }
        }

        /// <summary>
        /// Gets the root item of the tree view.
        /// </summary>
        public TreeControlItem RootItem => rootItem;

        /// <summary>
        /// Gets the collection of visible items contained in the tree view.
        /// </summary>
        protected new IListControlItems<ListControlItem> Items => base.Items;

        /// <summary>
        /// Selects the specified item in the tree view and scrolls to it.
        /// </summary>
        public virtual void SelectItem(UI.TreeControlItem? item)
        {
            if (item is null)
                return;

            bool InternalSelect()
            {
                var index = Items.IndexOf(item);

                if (index > 0)
                {
                    SelectedItem = item;
                    this.ScrollToRow(index);
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

        /// <summary>
        /// Clears all items from the tree view.
        /// </summary>
        public virtual void Clear()
        {
            BeginUpdate();
            try
            {
                SetItemsFast(new(), VirtualListBox.SetItemsKind.ChangeField);
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
        public virtual bool RemoveSelectedItem(bool selectSibling = false)
        {
            var item = SelectedItem as TreeControlItem;
            if (item is null)
                return false;

            if (selectSibling)
            {
                var index = Items.IndexOf(item);
                var result = Items.Remove(item);
                if (result)
                {
                    if (index > 0)
                        index--;
                    index = Math.Min(Items.Count - 1, index);
                    if (index >= 0)
                    {
                        item = Items[index] as TreeControlItem;
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
            if (item.Parent == rootItem)
            {
                Items.Add(item);
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
                Items?.Remove(item);
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

        /// <summary>
        /// Updates the tree view when the tree structure changes.
        /// </summary>
        protected virtual void TreeChanged()
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

            VirtualListBoxItems collection = new(rootItem.EnumExpandedItems());

            int indentPx = DefaultLevelMargin;

            foreach (var item in collection)
            {
                var treeItem = (TreeControlItem)item;

                var indentLevel = treeItem.IndentLelel - 1;

                item.ForegroundMargin = (indentPx * indentLevel, 0, 0, 0);
                item.CheckBoxVisible = treeItem.HasItems;
                item.IsChecked = treeItem.IsExpanded;
            }

            SetItemsFast(collection, VirtualListBox.SetItemsKind.ChangeField);
        }
    }
}
