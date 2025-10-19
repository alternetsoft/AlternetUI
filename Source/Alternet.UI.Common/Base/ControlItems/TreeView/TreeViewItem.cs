using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item in a tree control.
    /// </summary>
    public partial class TreeViewItem : ListControlItemWithNotify, IComparable<TreeViewItem>
    {
        private bool isVisible = true;
        private BaseCollection<TreeViewItem>? items;
        private bool isExpanded;
        private TreeViewItem? parent;
        private object? handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
        /// </summary>
        public TreeViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class
        /// with the specified text.
        /// </summary>
        /// <param name="text">The text of the tree control item.</param>
        public TreeViewItem(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/>
        /// class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">
        /// The zero-based index of the image within the
        /// <see cref="ImageList"/>
        /// associated with the <see cref="StdTreeView"/> that contains the item.
        /// </param>
        public TreeViewItem(string text, int? imageIndex)
            : this()
        {
            Text = text;
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current item is selected.
        /// </summary>
        /// <remarks>Changing this property updates the selection state
        /// of the item within its associated container, if any.</remarks>
        [Browsable(false)]
        public new virtual bool IsSelected
        {
            get
            {
                return IsSelected(Owner?.ListContainer);
            }

            set
            {
                if(IsSelected == value)
                    return;
                SetSelected(Owner?.ListContainer, value);
                Owner?.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets item handle. This is used only when item is connected to native control.
        /// </summary>
        [Browsable(false)]
        public virtual object? Handle
        {
            get
            {
                return handle;
            }

            set
            {
                handle = value;
            }
        }

        /// <summary>
        /// Gets or sets whether item is expanded when it is clicked.
        /// </summary>
        public virtual bool ExpandOnClick { get; set; }

        /// <summary>
        /// Indicates whether expanding this tree item should automatically
        /// collapse its sibling items.
        /// When enabled, the item enforces exclusive expansion behavior within
        /// its level in the hierarchy.
        /// </summary>
        /// <remarks>
        /// This setting affects only user-driven interactions, such as clicks or keyboard expansion.
        /// Programmatic expansion through code is not affected and will not trigger sibling collapse.
        /// </remarks>
        /// <remarks>
        /// This is useful in scenarios where a tree control mimics
        /// an accordion-style UI, allowing only one child to be expanded at a time.
        /// </remarks>
        /// <example>
        /// If <c>AutoCollapseSiblings</c> is <c>true</c>, expanding
        /// one item will collapse others at the same depth.
        /// </example>
        public virtual bool AutoCollapseSiblings { get; set; }

        /// <summary>
        /// Gets or sets the parent item of this tree control item.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem? Parent
        {
            get => parent;

            set
            {
                if (parent == value)
                    return;

                SmartInvoke(Internal);

                void Internal()
                {
                    DoInsideSuspendedPropertyChanged(
                        () =>
                        {
                            Parent?.Remove(this);
                            value?.Add(this);
                        },
                        false);

                    RaisePropertyChanged(nameof(Parent));
                }
            }
        }

        /// <summary>
        /// Gets whether this control is the root control (has no parent).
        /// </summary>
        [Browsable(false)]
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Gets whether this item is a child of the root item.
        /// </summary>
        [Browsable(false)]
        public bool IsRootChild => Parent is not null && Parent.IsRoot;

        /// <inheritdoc/>
        public override Thickness ForegroundMargin
        {
            get
            {
                var indentLevel = IndentLevel - 1;
                var indentPx = Owner?.GetLevelMargin() ?? StdTreeView.DefaultLevelMargin;
                Thickness result = (indentPx * indentLevel, 0, 0, 0);
                return result;
            }

            set
            {
            }
        }

        /// <inheritdoc/>
        public override CheckState CheckState
        {
            get
            {
                if (IsExpanded)
                    return CheckState.Checked;
                return CheckState.Unchecked;
            }

            set
            {
            }
        }

        /// <inheritdoc/>
        public override bool? CheckBoxVisible
        {
            get
            {
                if(base.CheckBoxVisible is null)
                {
                    return HasItems;
                }

                return HasItems && base.CheckBoxVisible.Value;
            }

            set
            {
                base.CheckBoxVisible = value;
            }
        }

        /// <summary>
        /// Gets the root parent control in the chain of parent controls.
        /// If parent control is null, returns this control.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem Root
        {
            get
            {
                var parent = Parent;
                if (parent is null)
                    return this;
                var result = parent.Root;
                return result;
            }
        }

        /// <summary>
        /// Gets the owner of this tree control item.
        /// </summary>
        /// <remarks>
        /// If this item is the root item, the owner is the object passed to the constructor.
        /// Otherwise, the owner is the owner of the root item.
        /// </remarks>
        [Browsable(false)]
        public virtual ITreeViewItemContainer? Owner
        {
            get
            {
                if (IsRoot)
                    return null;
                return Root.Owner;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        public virtual bool IsVisible
        {
            get => isVisible;

            set
            {
                if (isVisible == value)
                    return;

                SmartInvoke(Internal);

                void Internal()
                {
                    isVisible = value;
                    RaisePropertyChanged(nameof(IsVisible));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is expanded.
        /// </summary>
        public virtual bool IsExpanded
        {
            get => isExpanded;

            set
            {
                if (isExpanded == value)
                    return;

                SmartInvoke(Internal);

                void Internal()
                {
                    var owner = Owner;

                    if (owner is not null)
                    {
                        bool cancel = false;

                        if (value)
                        {
                            owner.RaiseBeforeExpand(this, ref cancel);
                            if (cancel)
                                return;
                        }
                        else
                        {
                            owner.RaiseBeforeCollapse(this, ref cancel);
                            if (cancel)
                                return;
                        }
                    }

                    isExpanded = value;
                    RaisePropertyChanged(nameof(IsExpanded));

                    if (owner is not null)
                    {
                        if (value)
                        {
                            owner.RaiseAfterExpand(this);
                        }
                        else
                        {
                            owner.RaiseAfterCollapse(this);
                        }

                        owner.RaiseExpandedChanged(this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the first child item of this tree control item.
        /// </summary>
        [Browsable(false)]
        public TreeViewItem? FirstChild
        {
            get
            {
                if (items is null || items.Count == 0)
                    return null;
                return items[0];
            }
        }

        /// <summary>
        /// Gets the last child item of this tree control item.
        /// </summary>
        [Browsable(false)]
        public TreeViewItem? LastChild
        {
            get
            {
                if (items is null || items.Count == 0)
                    return null;
                return items[items.Count - 1];
            }
        }

        /// <summary>
        /// Gets the child items of this tree control item.
        /// </summary>
        public virtual IReadOnlyList<TreeViewItem> Items
        {
            get
            {
                return items ?? [];
            }
        }

        /// <summary>
        /// Gets whether item has collapsed parents.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasCollapsedParents
        {
            get
            {
                if (Parent is null || Parent.IsExpanded)
                    return false;
                return Parent.HasCollapsedParents;
            }
        }

        /// <summary>
        /// Gets the number of child items in this tree control item.
        /// </summary>
        [Browsable(false)]
        public virtual int ItemCount
        {
            get
            {
                return items?.Count ?? 0;
            }
        }

        /// <summary>
        /// Gets items from the <see cref="StdTreeView"/> (if item is on root level)
        /// or from the <see cref="Parent"/> (if item has parent).
        /// </summary>
        /// <remarks>
        /// If item has no parent and is not attached to the <see cref="StdTreeView"/> this
        /// property returns <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public IReadOnlyList<TreeViewItem>? ParentItems
        {
            get
            {
                IReadOnlyList<TreeViewItem>? items;
                if (Parent == null)
                    items = Root?.Items;
                else
                    items = Parent.Items;
                return items;
            }
        }

        /// <summary>
        /// Gets the zero-based index of the current item within its parent,
        /// or <see langword="null"/> if the item has no parent.
        /// </summary>
        public virtual int? Index
        {
            get
            {
                var items = ParentItems;
                if (items is null)
                    return null;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == this)
                        return i;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this item is the last item in its parent items collection.
        /// </summary>
        [Browsable(false)]
        public bool IsLast
        {
            get
            {
                var items = ParentItems;
                if (items is null || items.Count == 0)
                    return false;
                return items[items.Count - 1] == this;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this item is the first item in its parent items collection.
        /// </summary>
        [Browsable(false)]
        public bool IsFirst
        {
            get
            {
                var items = ParentItems;
                if (items is null || items.Count == 0)
                    return false;
                return items[0] == this;
            }
        }

        /// <summary>
        /// Gets next sibling if item is not the last one, otherwise gets previous sibling.
        /// If item has no siblings, <see cref="Parent"/> is returned.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem? NextOrPrevSibling
        {
            get
            {
                var items = ParentItems;
                if (items == null)
                    return null;

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != this)
                        continue;
                    if (i == items.Count - 1)
                    {
                        if (i > 0)
                            return items[i - 1];
                        return Parent;
                    }
                    else
                        return items[i + 1];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this item has child items.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasItems => items is not null && items.Count > 0;

        /// <summary>
        /// Gets a value indicating whether this item has any visible child items.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasVisibleItems
        {
            get
            {
                if (items is null)
                    return false;
                foreach (var item in items)
                {
                    if (item.IsVisible)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the indent level of this item.
        /// </summary>
        [Browsable(false)]
        public virtual int IndentLevel
        {
            get
            {
                if (Parent is null)
                    return 0;
                return Parent.IndentLevel + 1;
            }
        }

        /// <summary>
        /// Removes the tree item from the tree view control.
        /// </summary>
        /// <remarks>
        /// When the <see cref="Remove()"/> method is called, the tree item, and
        /// any child tree items that are
        /// assigned to the <see cref="TreeViewItem"/>, are removed from the
        /// tree view control.
        /// </remarks>
        public virtual void Remove()
        {
            Parent = null;
        }

        /// <inheritdoc/>
        public virtual int CompareTo(TreeViewItem other)
        {
            return base.CompareTo(other);
        }

        /// <summary>
        /// Expands the tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Expand"/> method expands the
        /// <see cref="TreeViewItem"/>, while leaving the child items expanded
        /// state unchanged.
        /// </remarks>
        public virtual void Expand()
        {
            IsExpanded = true;
        }

        /// <summary>
        /// Collapses the tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Collapse"/> method collapses the
        /// <see cref="TreeViewItem"/>, while leaving the child items
        /// expanded state unchanged.
        /// </remarks>
        public virtual void Collapse()
        {
            IsExpanded = false;
        }

        /// <inheritdoc/>
        public override void ResetCachedImages()
        {
            base.ResetCachedImages();

            if (HasItems)
            {
                foreach (var item in Items)
                {
                    item.ResetCachedImages();
                }
            }
        }

        /// <summary>
        /// Toggles the tree item to either the expanded or collapsed state.
        /// </summary>
        /// <remarks>
        /// The tree item is toggled to the state opposite its current state,
        /// either expanded or collapsed.
        /// </remarks>
        public virtual void Toggle()
        {
            IsExpanded = !IsExpanded;
        }

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as
        /// necessary.
        /// </summary>
        /// <remarks>
        /// When the <see cref="EnsureVisible"/> method is called, the tree
        /// is expanded and scrolled to ensure that the current tree
        /// item is visible in the <see cref="StdTreeView"/>. This method is useful
        /// if you are selecting a tree item in code based on
        /// certain criteria. By calling this method after you select the item,
        /// the user can see and interact with the
        /// selected item.
        /// </remarks>
        public virtual void EnsureVisible() => Owner?.EnsureVisible(this);

        /// <summary>
        /// Scrolls the item into view.
        /// </summary>
        public virtual void ScrollIntoView() => Owner?.ScrollIntoView(this);

        /// <summary>
        /// Removes the specified child item from this tree control item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public virtual bool Remove(TreeViewItem item)
        {
            bool hasItems = HasItems;

            if (item.Parent == this)
            {
                item.InternalSetParent(null);
            }
            else
                return false;

            var result = items?.Remove(item) ?? false;

            if(HasItems != hasItems)
                RaisePropertyChanged(nameof(HasItems));

            Owner?.RaiseItemRemoved(item);

            return result;
        }

        /// <summary>
        /// Expands all parent items of this tree control item.
        /// </summary>
        public virtual bool ExpandAllParents()
        {
            if (Parent is null)
                return false;
            var result = Parent.ExpandAllParents();
            if (!Parent.IsExpanded)
            {
                Parent.IsExpanded = true;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Retrieves the child item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the child item to retrieve.</param>
        /// <returns>
        /// The <see cref="TreeViewItem"/> at the specified index, or <c>null</c>
        /// if the index is out of range or there are no child items.
        /// </returns>
        public virtual TreeViewItem? GetItem(int index)
        {
            if (items is null || index >= items.Count || index < 0)
                return null;
            return items[index];
        }

        /// <summary>
        /// Creates a new instance of <see cref="TreeViewItem"/>.
        /// </summary>
        /// <returns>A new <see cref="TreeViewItem"/> instance.</returns>
        public virtual TreeViewItem CreateItem()
        {
            return new TreeViewItem();
        }

        /// <summary>
        /// Creates a new <see cref="TreeViewItem"/>, assigns text to it,
        /// adds it as the last child of this item in the tree, and returns it.
        /// </summary>
        /// <param name="text">The text to assign to the item.</param>
        /// <returns>The newly created <see cref="TreeViewItem"/> with
        /// the specified text, added as the last child of this item in the tree.</returns>
        public virtual TreeViewItem AddWithText(string text)
        {
            TreeViewItem result = CreateItem();
            result.Text = text;
            Add(result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="TreeViewItem"/>, assigns text to it,
        /// adds it as the first child of this item in the tree, and returns it.
        /// </summary>
        /// <param name="text">The text to assign to the item.</param>
        /// <returns>The newly created <see cref="TreeViewItem"/> with
        /// the specified text, added as the first child of this item in the tree.</returns>
        public virtual TreeViewItem PrependWithText(string text)
        {
            TreeViewItem result = CreateItem();
            result.Text = text;
            Prepend(result);
            return result;
        }

        /// <summary>
        /// Adds the specified child item to this tree control item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(TreeViewItem item)
        {
            int index = items?.Count ?? 0;
            Insert(index, item);
        }

        /// <summary>
        /// Inserts the specified <see cref="TreeViewItem"/> at the beginning of the item list.
        /// </summary>
        /// <param name="item">The item to prepend.</param>
        public void Prepend(TreeViewItem item)
        {
            Insert(0, item);
        }

        /// <summary>
        /// Inserts the specified <see cref="TreeViewItem"/> at the given
        /// index within the item list.
        /// </summary>
        /// <param name="index">The position at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        public virtual void Insert(int index, TreeViewItem item)
        {
            SmartInvoke(Internal);

            void Internal()
            {
                bool hasItems = HasItems;

                items ??= new();
                item.InternalSetParent(this);
                items.Insert(index, item);

                if (HasItems != hasItems)
                    RaisePropertyChanged(nameof(HasItems));

                Owner?.RaiseItemAdded(item);
            }
        }

        /// <summary>
        /// Sets child items of this tree control item to the specified collection.
        /// </summary>
        /// <param name="items">The items to set.</param>
        public virtual void SetItems(IEnumerable<TreeViewItem> items)
        {
            SmartInvoke(Internal);
            void Internal()
            {
                Owner?.BeginUpdate();
                try
                {
                    Clear();
                    AddRange(items);
                }
                finally
                {
                    Owner?.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Sorts the items in descending order based on their comparison logic.
        /// </summary>
        /// <remarks>This method sorts the collection of items in descending order using a default
        /// comparison logic. If the collection is empty, the method performs no operation.</remarks>
        public virtual void SortDescending()
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.Sort(Comparison);

                int Comparison(TreeViewItem x, TreeViewItem y)
                {
                    var result = x.CompareTo(y);
                    return result < 0 ? 1 : result > 0 ? -1 : 0;
                }
            },
            true);
        }

        /// <summary>
        /// Sorts child items in ascending order.
        /// </summary>
        /// <remarks>This method sorts child items using the default comparer.
        /// If there are no child items, the method performs no operation.</remarks>
        public virtual void Sort()
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.Sort();
            },
            true);
        }

        /// <summary>
        /// Sorts the child items of the current <see cref="TreeViewItem"/>
        /// using the specified comparer.
        /// </summary>
        /// <remarks>This method has no effect if the current <see cref="TreeViewItem"/>
        /// has no child items. </remarks>
        /// <param name="comparer">An <see cref="IComparer{TreeViewItem}"/> implementation
        /// used to compare and sort the child items.
        /// If <paramref name="comparer"/> is <see langword="null"/>,
        /// the default comparer for <see cref="TreeViewItem"/> is used.</param>
        public virtual void Sort(IComparer<TreeViewItem>? comparer)
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.Sort(comparer);
            },
            true);
        }

        /// <summary>
        /// Sorts a range of child items using the specified comparer.
        /// </summary>
        /// <remarks>This method sorts the specified range of child items. If there are
        /// no items, the method returns without performing any operation.</remarks>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The number of items in the range to sort.</param>
        /// <param name="comparer">The <see cref="IComparer{TreeViewItem}"/> implementation
        /// to use when comparing items, or <see langword="null"/> to use
        /// the default comparer.</param>
        public virtual void Sort(int index, int count, IComparer<TreeViewItem>? comparer)
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.Sort(index, count, comparer);
            },
            true);
        }

        /// <summary>
        /// Sorts child items using the specified comparison.
        /// </summary>
        /// <remarks>This method has no effect if the current <see cref="TreeViewItem"/>
        /// The comparison delegate is applied to the items to determine their relative order.
        /// has no child items. </remarks>
        /// <param name="comparison">A delegate that defines the comparison logic
        /// to determine the order of the items.</param>
        public virtual void Sort<T2>(Comparison<T2> comparison)
            where T2 : TreeViewItem
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.Sort(Comparison);

                int Comparison(TreeViewItem x, TreeViewItem y)
                {
                    return comparison((T2)x, (T2)y);
                }
            },
            true);
        }

        /// <summary>
        /// Sorts the items in descending order based on the specified comparison logic.
        /// </summary>
        /// <remarks>This method performs the sorting operation only if the collection contains items.
        /// The sorting is done in descending order by reversing the result of the provided
        /// comparison logic.</remarks>
        /// <typeparam name="T2">The type of the items to be sorted.
        /// Must derive from <see cref="TreeViewItem"/>.</typeparam>
        /// <param name="comparison">A delegate that defines the comparison
        /// logic to determine the order of the items. The delegate should return
        /// a negative value if the first item is less than the second,
        /// zero if they are equal, and a positive value if
        /// the first item is greater than the second.</param>
        public virtual void SortDescending<T2>(Comparison<T2> comparison)
            where T2 : TreeViewItem
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.Sort(Comparison);

                int Comparison(TreeViewItem x, TreeViewItem y)
                {
                    var result = comparison((T2)x, (T2)y);
                    return result < 0 ? 1 : result > 0 ? -1 : 0;
                }
            },
            true);
        }

        /// <summary>
        /// Sorts the items in descending order based on the specified comparison logic.
        /// </summary>
        /// <remarks>This method performs the sorting operation only if the child collection contains items.
        /// The sorting is executed in descending order by reversing the result of the provided
        /// comparison logic.</remarks>
        /// <param name="comparison">A delegate that defines the comparison logic
        /// to apply to the items. The delegate
        /// should return a value less than zero if the first item is less than the second,
        /// zero if they are equal, or
        /// greater than zero if the first item is greater than the second.</param>
        public virtual void SortDescending(Comparison<TreeViewItem> comparison)
        {
            if (!HasItems)
                return;

            DoInsideUpdate(
            () =>
            {
                items?.SortDescending(comparison);
            },
            true);
        }

        /// <summary>
        /// Calls the specified action inside an update block,
        /// ensuring that the owner of this tree control item is updated correctly.
        /// </summary>
        /// <param name="action">The action to call.</param>
        /// <param name="treeChanged">Whether to update owner control.</param>
        public virtual void DoInsideUpdate(Action action, bool treeChanged = true)
        {
            SmartInvoke(Internal);

            void Internal()
            {
                Owner?.BeginUpdate();
                try
                {
                    action();
                    if(treeChanged)
                        Owner?.TreeChanged();
                }
                finally
                {
                    Owner?.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Adds a range of <see cref="TreeViewItem"/> child items to this tree control item.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public virtual void AddRange(IEnumerable<TreeViewItem> items)
        {
            SmartInvoke(Internal);
            void Internal()
            {
                if (items is null)
                    return;
                Owner?.BeginUpdate();
                try
                {
                    foreach (var item in items)
                    {
                        item.InternalSetParent(this);
                        Add(item);
                    }

                    Owner?.TreeChanged();
                    RaisePropertyChanged(nameof(HasItems));
                }
                finally
                {
                    Owner?.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Clears all child items from this tree control item.
        /// </summary>
        public virtual void Clear()
        {
            SmartInvoke(Internal);

            void Internal()
            {
                if (items is null || items.Count == 0)
                    return;

                Owner?.BeginUpdate();
                try
                {
                    for(int i = items.Count - 1; i >= 0; i--)
                    {
                        items[i].InternalSetParent(null);
                    }

                    items.Clear();
                    Owner?.TreeChanged();
                    RaisePropertyChanged(nameof(HasItems));
                }
                finally
                {
                    Owner?.EndUpdate();
                }
            }
        }

        /// <inheritdoc/>
        public override ItemCheckBoxInfo GetCheckBoxInfo(
            IListControlItemContainer? container,
            RectD rect)
        {
            var result = base.GetCheckBoxInfo(container, rect);
            if (Owner is null || Owner.TreeButtons != TreeViewButtonsKind.Null)
                result.KeepTextPaddingWithoutCheckBox = true;
            return result;
        }

        /// <summary>
        /// Sets <see cref="IsExpanded"/> property of child items.
        /// </summary>
        /// <param name="expanded">New value of <see cref="IsExpanded"/> property.</param>
        /// <param name="onlyVisible">Whether to update only visible items.</param>
        public virtual void SetItemsExpanded(bool expanded, bool onlyVisible = true)
        {
            if (!HasItems)
                return;

            DoInsideUpdate(() =>
            {
                foreach(var item in Items)
                {
                    if (onlyVisible && !item.IsVisible)
                        continue;
                    if (!item.HasItems)
                        continue;
                    item.IsExpanded = expanded;
                }
            });
        }

        /// <summary>
        /// Sets <see cref="IsExpanded"/> property of child items recursively.
        /// </summary>
        /// <param name="expanded">New value of <see cref="IsExpanded"/> property.</param>
        /// <param name="onlyVisible">Whether to update only visible items.</param>
        public virtual void SetItemsExpandedRecursive(bool expanded, bool onlyVisible = true)
        {
            if (!HasItems)
                return;

            DoInsideUpdate(() =>
            {
                foreach (var item in Items)
                {
                    if (onlyVisible && !item.IsVisible)
                        continue;
                    if (!item.HasItems)
                        continue;
                    item.IsExpanded = expanded;
                    item.SetItemsExpandedRecursive(expanded, onlyVisible);
                }
            });
        }

        /// <summary>
        /// Expands this item and all the child items recursively.
        /// After calling this method, value of <see cref="IsExpanded"/> property
        /// will be <see langword="true"/> for this item and all its child items.
        /// </summary>
        public virtual void ExpandAll()
        {
            DoInsideUpdate(() =>
            {
                IsExpanded = true;
                SetItemsExpandedRecursive(expanded: true, onlyVisible: false);
            });
        }

        /// <summary>
        /// Collapses this item and all the child items recursively.
        /// After calling this method, value of <see cref="IsExpanded"/> property
        /// will be <see langword="false"/> for this item and all its child items.
        /// </summary>
        public virtual void CollapseAll()
        {
            DoInsideUpdate(() =>
            {
                IsExpanded = false;
                SetItemsExpandedRecursive(expanded: false, onlyVisible: false);
            });
        }

        /// <summary>
        /// Expands child items.
        /// </summary>
        public void ExpandItems(bool onlyVisible = true)
        {
            SetItemsExpanded(expanded: true, onlyVisible);
        }

        /// <summary>
        /// Copies the properties and child items from the specified <see cref="TreeViewItem"/>
        /// to the current instance.
        /// </summary>
        /// <remarks>This method copies the visibility and expansion state of the source item,
        /// clears the current item's child collection, and adds clones of the source
        /// item's children to the current instance.
        /// The cloning operation ensures that the child items are independent
        /// of the source item's children.</remarks>
        /// <param name="assignFrom">The <see cref="TreeViewItem"/> whose
        /// properties and child items  are to be copied.
        /// Cannot be <see langword="null"/>.</param>
        public virtual void Assign(TreeViewItem assignFrom)
        {
            DoInsideUpdate(Internal);

            void Internal()
            {
                base.Assign(assignFrom);
                IsVisible = assignFrom.IsVisible;
                IsExpanded = assignFrom.IsExpanded;
                Clear();

                if (!assignFrom.HasItems)
                    return;
                foreach (var item in assignFrom.Items)
                {
                    var newItem = item.Clone();
                    Add(newItem);
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TreeViewItem"/>
        /// class with the same properties as the current instance.
        /// </summary>
        /// <remarks>The returned <see cref="TreeViewItem"/> is a deep copy of the current instance,
        /// including all relevant properties.</remarks>
        /// <returns>A new <see cref="TreeViewItem"/> instance that is a copy of the current instance.</returns>
        public new virtual TreeViewItem Clone()
        {
            var result = new TreeViewItem();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Collapses child items.
        /// </summary>
        public void CollapseItems(bool onlyVisible = true)
        {
            SetItemsExpanded(expanded: false, onlyVisible);
        }

        /// <inheritdoc/>
        public override bool SetSelected(IListControlItemContainer? container, bool value)
        {
            var result = base.SetSelected(container, value);
            Owner?.RaiseItemSelectedChanged(this, value);
            return result;
        }

        /// <summary>
        /// Retrieves all child items of this tree control item.
        /// If a child item is expanded, its children are also included in the result.
        /// All child items are processed recursively.
        /// </summary>
        /// <returns>An enumerable collection containing all child items.</returns>
        /// <param name="prm">The parameters that define which items should be
        /// included in the result.</param>
        public virtual IEnumerable<TreeViewItem> EnumExpandedItems(
            EnumExpandedItemsParams? prm = null)
        {
            if (items is null || items.Count == 0)
                yield break;

            prm ??= EnumExpandedItemsParams.Default;

            foreach (var item in items)
            {
                if (!Condition(item))
                    continue;
                yield return item;

                if (!prm.Recursive || !item.HasItems)
                    continue;

                if (prm.OnlyExpanded && !item.IsExpanded)
                    continue;

                var subItems = item.EnumExpandedItems(prm);

                foreach(var subItem in subItems)
                {
                    if (Condition(subItem))
                        yield return subItem;
                }
            }

            bool Condition(TreeViewItem itm)
            {
                if (prm.OnlyVisible)
                {
                    if (!itm.IsVisible)
                        return false;
                }

                if(prm.Condition is not null)
                {
                    return prm.Condition(itm);
                }

                return true;
            }
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            Owner?.RaiseItemPropertyChanged(this, propertyName);
        }

        /// <summary>
        /// Used to invoke the update action.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        protected virtual void SmartInvoke(Action action)
        {
            Invoke(action);
        }

        private void InternalSetParent(TreeViewItem? newParent)
        {
            parent = newParent;
            RaisePropertyChanged(nameof(Parent));
        }

        /// <summary>
        /// Represents the parameters used for enumerating expanded tree control items.
        /// </summary>
        public record EnumExpandedItemsParams
        {
            /// <summary>
            /// The default instance of <see cref="EnumExpandedItemsParams"/>.
            /// </summary>
            public static readonly EnumExpandedItemsParams Default = new();

            /// <summary>
            /// Specifies whether to include only visible items in the enumeration.
            /// Defaults to <c>true</c>.
            /// </summary>
            public bool OnlyVisible = true;

            /// <summary>
            /// Specifies whether to include only items with expanded parents in the enumeration.
            /// Defaults to <c>true</c>.
            /// </summary>
            public bool OnlyExpanded = true;

            /// <summary>
            /// Specifies whether to process all child items recursively.
            /// Defaults to <c>true</c>.
            /// </summary>
            public bool Recursive = true;

            /// <summary>
            /// A function that determines whether an item should be included
            /// in the enumeration.
            /// This function takes a <see cref="TreeViewItem"/> as input and
            /// returns <c>true</c> if the item meets the condition; otherwise, <c>false</c>.
            /// </summary>
            public Func<TreeViewItem, bool>? Condition;
        }
    }
}
