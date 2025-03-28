using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item in a tree control.
    /// </summary>
    public partial class TreeControlItem : ListControlItem
    {
        private List<TreeControlItem>? items;
        private object? owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class.
        /// </summary>
        public TreeControlItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class
        /// with the specified owner. Use this constructor only for the root items.
        /// </summary>
        /// <param name="owner">The owner of the item.</param>
        public TreeControlItem(object owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Gets or sets the parent item of this tree control item.
        /// </summary>
        public virtual TreeControlItem? Parent { get; set; }

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
        public TreeControlItem Root
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

        public virtual object? Owner
        {
            get
            {
                if (IsRoot)
                    return owner;
                return Root.Owner;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        public virtual bool IsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this item is expanded.
        /// </summary>
        public virtual bool IsExpanded { get; set; }

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
        /// Removes the specified child item from this tree control item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public virtual bool Remove(TreeControlItem item)
        {
            if (item.Parent == this)
            {
                item.Parent = null;
            }
            else
                return false;

            return items?.Remove(item) ?? false;
        }

        /// <summary>
        /// Adds the specified child item to this tree control item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public virtual void Add(TreeControlItem item)
        {
            items ??= new();
            item.Parent = this;
            items.Add(item);
        }

        /// <summary>
        /// Clears all child items from this tree control item.
        /// </summary>
        public virtual void Clear()
        {
            if (items is null)
                return;
            foreach(var item in items)
            {
                item.Parent = null;
            }

            items.Clear();
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
