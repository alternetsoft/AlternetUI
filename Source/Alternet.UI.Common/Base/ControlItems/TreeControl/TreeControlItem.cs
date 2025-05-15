﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item in a tree control.
    /// </summary>
    public partial class TreeControlItem : ListControlItemWithNotify
    {
        private bool isVisible = true;
        private BaseCollection<TreeControlItem>? items;
        private bool isExpanded;
        private TreeControlItem? parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class.
        /// </summary>
        public TreeControlItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class
        /// with the specified text.
        /// </summary>
        /// <param name="text">The text of the tree control item.</param>
        public TreeControlItem(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/>
        /// class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">
        /// The zero-based index of the image within the
        /// <see cref="ImageList"/>
        /// associated with the <see cref="VirtualTreeControl"/> that contains the item.
        /// </param>
        public TreeControlItem(string text, int? imageIndex)
            : this()
        {
            Text = text;
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets the parent item of this tree control item.
        /// </summary>
        public virtual TreeControlItem? Parent
        {
            get => parent;

            set
            {
                if (parent == value)
                    return;
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

        /// <summary>
        /// Gets whether this control is the root control (has no parent).
        /// </summary>
        [Browsable(false)]
        public bool IsRoot => Parent == null;

        /// <inheritdoc/>
        public override Thickness ForegroundMargin
        {
            get
            {
                var indentLevel = IndentLevel - 1;
                var indentPx = Owner?.GetLevelMargin() ?? VirtualTreeControl.DefaultLevelMargin;
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
        public virtual TreeControlItem Root
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
        public virtual ITreeControlItemContainer? Owner
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
                isVisible = value;
                RaisePropertyChanged(nameof(IsVisible));
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

                var owner = Owner;

                if(owner is not null)
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

                if(owner is not null)
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

        /// <summary>
        /// Gets the first child item of this tree control item.
        /// </summary>
        public TreeControlItem? FirstChild
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
        public TreeControlItem? LastChild
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
        public virtual IReadOnlyList<TreeControlItem> Items
        {
            get
            {
                return items ?? [];
            }
        }

        /// <summary>
        /// Gets whether item has collapsed parents.
        /// </summary>
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
        public virtual int ItemCount
        {
            get
            {
                return items?.Count ?? 0;
            }
        }

        /// <summary>
        /// Gets items from the <see cref="TreeView"/> (if item is on root level)
        /// or from the <see cref="Parent"/> (if item has parent).
        /// </summary>
        /// <remarks>
        /// If item has no parent and is not attached to the <see cref="TreeView"/> this
        /// property returns <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public IReadOnlyList<TreeControlItem>? ParentItems
        {
            get
            {
                IReadOnlyList<TreeControlItem>? items;
                if (Parent == null)
                    items = Root?.Items;
                else
                    items = Parent.Items;
                return items;
            }
        }

        /// <summary>
        /// Gets next sibling if item is not the last one, otherwise gets previous sibling.
        /// If item has no siblings, <see cref="Parent"/> is returned.
        /// </summary>
        [Browsable(false)]
        public virtual TreeControlItem? NextOrPrevSibling
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
        public virtual bool HasItems => items is not null && items.Count > 0;

        /// <summary>
        /// Gets the indent level of this item.
        /// </summary>
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
        /// assigned to the <see cref="TreeControlItem"/>, are removed from the
        /// tree view control.
        /// </remarks>
        public virtual void Remove()
        {
            Parent = null;
        }

        /// <summary>
        /// Expands the tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Expand"/> method expands the
        /// <see cref="TreeControlItem"/>, while leaving the child items expanded
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
        /// <see cref="TreeControlItem"/>, while leaving the child items
        /// expanded state unchanged.
        /// </remarks>
        public virtual void Collapse()
        {
            IsExpanded = false;
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
        /// Removes the specified child item from this tree control item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public virtual bool Remove(TreeControlItem item)
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
        /// The <see cref="TreeControlItem"/> at the specified index, or <c>null</c>
        /// if the index is out of range or there are no child items.
        /// </returns>
        public virtual TreeControlItem? GetItem(int index)
        {
            if (items is null || index >= items.Count || index < 0)
                return null;
            return items[index];
        }

        /// <summary>
        /// Creates a new instance of <see cref="TreeControlItem"/>.
        /// </summary>
        /// <returns>A new <see cref="TreeControlItem"/> instance.</returns>
        public virtual TreeControlItem CreateItem()
        {
            return new TreeControlItem();
        }

        /// <summary>
        /// Creates a new <see cref="TreeControlItem"/>, assigns text to it,
        /// adds it as the last child of this item in the tree, and returns it.
        /// </summary>
        /// <param name="text">The text to assign to the item.</param>
        /// <returns>The newly created <see cref="TreeControlItem"/> with
        /// the specified text, added as the last child of this item in the tree.</returns>
        public virtual TreeControlItem AddWithText(string text)
        {
            TreeControlItem result = CreateItem();
            result.Text = text;
            Add(result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="TreeControlItem"/>, assigns text to it,
        /// adds it as the first child of this item in the tree, and returns it.
        /// </summary>
        /// <param name="text">The text to assign to the item.</param>
        /// <returns>The newly created <see cref="TreeControlItem"/> with
        /// the specified text, added as the first child of this item in the tree.</returns>
        public virtual TreeControlItem PrependWithText(string text)
        {
            TreeControlItem result = CreateItem();
            result.Text = text;
            Prepend(result);
            return result;
        }

        /// <summary>
        /// Adds the specified child item to this tree control item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(TreeControlItem item)
        {
            int index = items?.Count ?? 0;
            Insert(index, item);
        }

        /// <summary>
        /// Inserts the specified <see cref="TreeControlItem"/> at the beginning of the item list.
        /// </summary>
        /// <param name="item">The item to prepend.</param>
        public void Prepend(TreeControlItem item)
        {
            Insert(0, item);
        }

        /// <summary>
        /// Inserts the specified <see cref="TreeControlItem"/> at the given index within the item list.
        /// </summary>
        /// <param name="index">The position at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        public virtual void Insert(int index, TreeControlItem item)
        {
            Invoke(Internal);

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
        /// Clears all child items from this tree control item.
        /// </summary>
        public virtual void Clear()
        {
            Invoke(Internal);

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
        /// Retrieves all child items of this tree control item.
        /// If a child item is expanded, its children are also included in the result.
        /// All child items are processed recursively.
        /// </summary>
        /// <returns>An enumerable collection containing all child items.</returns>
        /// <param name="prm">The parameters that define which items should be
        /// included in the result.</param>
        public virtual IEnumerable<TreeControlItem> EnumExpandedItems(
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

                if (!prm.Recursive || !item.HasItems || !item.IsExpanded)
                    continue;
                var subItems = item.EnumExpandedItems(prm);

                foreach(var subItem in subItems)
                {
                    if (Condition(subItem))
                        yield return subItem;
                }
            }

            bool Condition(TreeControlItem itm)
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

        private void InternalSetParent(TreeControlItem? newParent)
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
            /// Specifies whether to process all child items recursively.
            /// Defaults to <c>true</c>.
            /// </summary>
            public bool Recursive = true;

            /// <summary>
            /// A function that determines whether an item should be included
            /// in the enumeration.
            /// This function takes a <see cref="TreeControlItem"/> as input and
            /// returns <c>true</c> if the item meets the condition; otherwise, <c>false</c>.
            /// </summary>
            public Func<TreeControlItem, bool>? Condition;
        }
    }
}
