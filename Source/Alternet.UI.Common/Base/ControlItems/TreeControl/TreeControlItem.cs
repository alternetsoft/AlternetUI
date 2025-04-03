using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item in a tree control.
    /// </summary>
    public partial class TreeControlItem : ListControlItemWithNotify
    {
        private bool isVisible = true;
        private List<TreeControlItem>? items;
        private bool isExpanded;
        private TreeControlItem? parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class.
        /// </summary>
        public TreeControlItem()
        {
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
        public virtual IEnumerable<TreeControlItem> Items => items ?? [];

        /// <summary>
        /// Gets a value indicating whether this item has child items.
        /// </summary>
        public virtual bool HasItems => items is not null && items.Count > 0;

        /// <summary>
        /// Gets the indent level of this item.
        /// </summary>
        public virtual int IndentLelel
        {
            get
            {
                if (Parent is null)
                    return 0;
                return Parent.IndentLelel + 1;
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
        public virtual void ExpandAllParents()
        {
            if (Parent is null)
                return;
            Parent.IsExpanded = true;
            Parent.ExpandAllParents();
        }

        /// <summary>
        /// Adds the specified child item to this tree control item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public virtual void Add(TreeControlItem item)
        {
            bool hasItems = HasItems;

            items ??= new();
            item.InternalSetParent(this);
            items.Add(item);

            if (HasItems != hasItems)
                RaisePropertyChanged(nameof(HasItems));

            Owner?.RaiseItemAdded(item);
        }

        /// <summary>
        /// Clears all child items from this tree control item.
        /// </summary>
        public virtual void Clear()
        {
            if (items is null || items.Count == 0)
                return;

            Owner?.BeginUpdate();
            try
            {
                foreach (var item in items)
                {
                    item.InternalSetParent(null);
                }

                items.Clear();
                RaisePropertyChanged(nameof(HasItems));
            }
            finally
            {
                Owner?.EndUpdate();
            }
        }

        /// <summary>
        /// Retrieves all child items of this tree control item.
        /// If a child item is expanded, its children are also included in the result.
        /// All child items are processed recursively.
        /// </summary>
        /// <returns>An enumerable collection containing all child items.</returns>
        /// <param name="prm">The parameters that define which items should be
        /// included in the result.</param>
        public virtual IEnumerable<TreeControlItem> EnumExpandedItems(EnumExpandedItemsParams? prm = null)
        {
            if (items is null)
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
