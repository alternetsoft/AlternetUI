using System;
using System.ComponentModel;
using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item (also known as a node) of a <see cref="TreeView"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Items"/> collection holds all the child
    /// <see cref="TreeViewItem"/> objects assigned to the current
    /// <see cref="TreeViewItem"/>.
    /// You can add or remove a <see cref="TreeViewItem"/>; when you do this,
    /// all child tree items are added or removed.
    /// Each <see cref="TreeViewItem"/> can contain a collection of other
    /// <see cref="TreeViewItem"/> objects.
    /// </para>
    /// <para>
    /// The <see cref="TreeViewItem"/> text label is set by setting the
    /// <see cref="Text"/> property explicitly.
    /// The alternative is to create the tree item using one of the
    /// <see cref="TreeViewItem"/> constructors
    /// that has a string parameter that represents the <see cref="Text"/>
    /// property.
    /// The label is displayed next to the <see cref="TreeViewItem"/> image,
    /// if one is displayed.
    /// </para>
    /// <para>
    /// To display images next to the tree items, assign an
    /// <see cref="ImageList"/> to the <see cref="TreeView.ImageList"/> property
    /// of the parent <see cref="TreeView"/> control and assign an
    /// <see cref="Image"/> by referencing its index value in
    /// the <see cref="TreeView.ImageList"/> property. To do that, set the
    /// <see cref="ImageIndex"/> property to the index value
    /// of the <see cref="Image"/> you want to display in the
    /// <see cref="TreeViewItem"/>.
    /// </para>
    /// <para>
    /// <see cref="TreeView"/> items can be expanded to display the next level
    /// of child items.
    /// The user can expand the <see cref="TreeViewItem"/> by clicking
    /// the expand button, if one is displayed
    /// next to the <see cref="TreeViewItem"/>, or you can expand the
    /// <see cref="TreeViewItem"/> by calling the <see cref="TreeViewItem.Expand"/>
    /// method.
    /// To expand all the child item levels in the <see cref="Items"/>
    /// collection, call the <see cref="TreeViewItem.ExpandAll"/> method.
    /// You can collapse the child <see cref="TreeViewItem"/> level by
    /// calling the <see cref="TreeViewItem.Collapse"/> method,
    /// or the user can press the expand button, if one is displayed next
    /// to the <see cref="TreeViewItem"/>.
    /// You can also call the <see cref="TreeViewItem.Toggle"/> method to
    /// alternate between the expanded and collapsed states.
    /// </para>
    /// </remarks>
    public class TreeViewItem : BaseControlItem
    {
        private string text = string.Empty;
        private TreeView? treeView;
        private int? imageIndex;
        private BaseCollection<TreeViewItem>? items;
        private bool isBold;
        private Color? textColor;
        private Color? backgroundColor;
        private object? handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class
        /// with default values.
        /// </summary>
        public TreeViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/>
        /// class with the specified item text.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        public TreeViewItem(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/>
        /// class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">
        /// The zero-based index of the image within the
        /// <see cref="TreeView.ImageList"/>
        /// associated with the <see cref="TreeView"/> that contains the
        /// item.
        /// </param>
        public TreeViewItem(string text, int? imageIndex)
        {
            this.text = text;
            this.imageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets whether item is shown in bold font.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsBold
        {
            get
            {
                return isBold;
            }

            set
            {
                if (isBold == value)
                    return;
                isBold = value;

                treeView?.Handler.SetItemIsBold(this, isBold);
            }
        }

        /// <summary>
        /// Gets or sets background color of the item.
        /// </summary>
        [Browsable(false)]
        public virtual Color? BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                if (backgroundColor == value)
                    return;
                backgroundColor = value;
                treeView?.Handler.SetItemBackgroundColor(this, value);
            }
        }

        /// <summary>
        /// Gets or sets text color of the item.
        /// </summary>
        [Browsable(false)]
        public virtual Color? TextColor
        {
            get
            {
                return textColor;
            }

            set
            {
                if (textColor == value)
                    return;
                textColor = value;
                treeView?.Handler.SetItemTextColor(this, value);
            }
        }

        /// <summary>
        /// Gets the parent tree item of the current item.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> that represents the parent of
        /// the current tree item, or <c>null</c> if the item is a root.</value>
        /// <remarks>If the tree item is at the root level, the
        /// <see cref="Parent"/> property returns <c>null</c>.</remarks>
        [Browsable(false)]
        public TreeViewItem? Parent { get; private set; }

        /// <summary>
        /// Gets the owner <see cref="TreeView"/> that the tree item is assigned to.
        /// </summary>
        /// <value>
        /// A <see cref="TreeView"/> that represents the parent tree view that
        /// the tree item is assigned to,
        /// or <c>null</c> if the item has not been assigned to a tree view.
        /// </value>
        [Browsable(false)]
        public TreeView? TreeView
        {
            get => treeView;
            internal set
            {
                if (treeView == value)
                    return;

                treeView = value;
                ApplyTreeViewToAllChildren();
            }
        }

        /// <summary>
        /// Gets or sets the text of the item.
        /// </summary>
        /// <value>The text to display for the item.</value>
        public virtual string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;

                text = value;

                treeView?.Handler.SetItemText(this, text);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tree item is in the selected state.
        /// </summary>
        /// <value><see langword="true"/> if the tree item is in the selected
        /// state; otherwise, <see langword="false"/>.</value>
        [Browsable(false)]
        public bool IsSelected
        {
            get => RequiredTreeView.Handler.IsItemSelected(this);
            set
            {
                var treeView = RequiredTreeView;

                if (treeView.SelectionMode == TreeViewSelectionMode.Multiple)
                    treeView.SetSelected(this, value);
                else
                {
                    if (value)
                        treeView.SelectedItem = this;
                    else
                    {
                        if (treeView.SelectedItem == this)
                            treeView.SelectedItem = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tree item is in the
        /// focused state.
        /// </summary>
        [Browsable(false)]
        public bool IsFocused
        {
            get => RequiredTreeView.Handler.IsItemFocused(this);
            set => TreeView?.Handler.SetFocused(this, value);
        }

        /// <summary>
        /// Gets the position of the tree item in the tree item collection.
        /// </summary>
        /// <value>
        /// A zero-based index value that represents the position of the tree item in
        /// a <see cref="BaseCollection{TreeViewItem}"/>, or <c>null</c> if the item
        /// is not contained in any collection.
        /// </value>
        [Browsable(false)]
        public int? Index { get; private set; }

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the item.
        /// </summary>
        /// <value>
        /// The zero-based index of the image in the
        /// <see cref="TreeView.ImageList"/> that is displayed for the item.
        /// The default is <c>null</c>.
        /// The <c>null</c> value means the default image specified by
        /// <see cref="TreeView.ImageIndex"/> property is used.
        /// </value>
        /// <remarks>
        /// The effect of setting this property depends on the value of the
        /// <see cref="TreeView.ImageList"/> property.
        /// You can specify which images from the list are displayed for items
        /// by default by setting the <see cref="TreeView.ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image
        /// is displayed by setting the <see cref="ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override
        /// the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public virtual int? ImageIndex
        {
            get => imageIndex;
            set
            {
                if (imageIndex == value)
                    return;
                imageIndex = value;
                treeView?.Handler.SetItemImageIndex(this, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tree item is in the expanded state.
        /// </summary>
        /// <value><c>true</c> if the tree item is in the expanded state;
        /// otherwise, <c>false</c>. Default is <c>false</c>.</value>
        /// <remarks>
        /// To track changing of this state, see
        /// <see cref="TreeView.ExpandedChanged"/>,
        /// <see cref="TreeView.AfterExpand"/>,
        /// <see cref="TreeView.AfterCollapse"/> events.
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsExpanded { get; set; }

        /// <summary>
        /// Gets the collection of child <see cref="TreeViewItem"/> of the
        /// current tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Items"/> property can hold a collection of other
        /// <see cref="TreeViewItem"/> objects.
        /// Each of the tree items in the collection has an
        /// <see cref="Items"/> property that can contain its own item collection.
        /// </remarks>
        [Browsable(false)]
        public virtual BaseCollection<TreeViewItem> Items
        {
            get
            {
                if (items == null)
                {
                    items = new() { ThrowOnNullAdd = true };
                    items.ItemInserted += Items_ItemInserted;
                    items.ItemRemoved += Items_ItemRemoved;
                }

                return items;
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
        public BaseCollection<TreeViewItem>? ParentItems
        {
            get
            {
                BaseCollection<TreeViewItem>? items;
                if (Parent == null)
                    items = TreeView?.Items;
                else
                    items = Parent.Items;
                return items;
            }
        }

        /// <summary>
        /// Gets whether there are any items in the <see cref="Items"/> list.
        /// </summary>
        [Browsable(false)]
        public bool HasItems => items != null && items.Count > 0;

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
        /// Gets or sets item handle
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

        private TreeView RequiredTreeView =>
            TreeView ?? throw new InvalidOperationException(
                string.Format(ErrorMessages.Default.PropertyCannotBeNull, nameof(TreeView)));

        /// <summary>
        /// Initiates the editing of the tree item label.
        /// </summary>
        public virtual void BeginLabelEdit() => TreeView?.Handler.BeginLabelEdit(this);

        /// <summary>
        /// Ends the editing of the tree item label.
        /// </summary>
        /// <param name="cancel"><see langword="true"/> if the editing of the
        /// tree item label text was canceled without
        /// being saved; otherwise, <see langword="false"/>.</param>
        public virtual void EndLabelEdit(bool cancel) =>
            TreeView?.Handler.EndLabelEdit(this, cancel);

        /// <summary>
        /// Expands this <see cref="TreeViewItem"/> and all the child tree items.
        /// </summary>
        /// <remarks>
        /// The <see cref="ExpandAll"/> method expands this
        /// <see cref="TreeViewItem"/> and all the child tree items assigned
        /// to the <see cref="Items"/> collection.
        /// </remarks>
        public virtual void ExpandAll() => TreeView?.Handler.ExpandAllChildren(this);

        /// <summary>
        /// Collapses this <see cref="TreeViewItem"/> and all the child tree items.
        /// </summary>
        /// <remarks>
        /// The <see cref="CollapseAll"/> method collapses this
        /// <see cref="TreeViewItem"/> and all the child tree items assigned to
        /// the <see cref="Items"/> collection.
        /// </remarks>
        public virtual void CollapseAll() =>
            TreeView?.Handler.CollapseAllChildren(this);

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as
        /// necessary.
        /// </summary>
        /// <remarks>
        /// When the <see cref="EnsureVisible"/> method is called, the tree
        /// is expanded and scrolled to ensure that the current tree
        /// item is visible in the <see cref="TreeView"/>. This method is useful
        /// if you are selecting a tree item in code based on
        /// certain criteria. By calling this method after you select the item,
        /// the user can see and interact with the
        /// selected item.
        /// </remarks>
        public virtual void EnsureVisible() => TreeView?.Handler.EnsureVisible(this);

        /// <summary>
        /// Scrolls the item into view.
        /// </summary>
        public virtual void ScrollIntoView() => TreeView?.Handler.ScrollIntoView(this);

        /// <summary>
        /// Removes the current tree item from the tree view control.
        /// </summary>
        /// <remarks>
        /// When the <see cref="Remove"/> method is called, the tree item, and
        /// any child tree items that are
        /// assigned to the <see cref="TreeViewItem"/>, are removed from the
        /// <see cref="TreeView"/>.
        /// </remarks>
        public virtual void Remove()
        {
            if (Parent == null)
                TreeView?.Items.Remove(this);
            else
                Parent.Items.Remove(this);
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
        /// Creates copy of this <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="subItems">if <c>true</c>, <see cref="Items"/> are also cloned.</param>
        public virtual TreeViewItem Clone(bool subItems = true)
        {
            var result = new TreeViewItem();
            result.Assign(this, subItems);
            return result;
        }

        /// <summary>
        /// Assigns properties from another <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="item">Source of the properties to assign.</param>
        /// <param name="subItems">if <c>true</c>, <see cref="Items"/> are also assigned.</param>
        public virtual void Assign(TreeViewItem item, bool subItems = true)
        {
            Tag = item.Tag;
            Text = item.Text;
            ImageIndex = item.ImageIndex;
            Items.Clear();
            if (!item.HasItems || !subItems)
                return;
            foreach(var childItem in item.Items)
            {
                var newItem = childItem.Clone();
                Items.Add(newItem);
            }
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

        /// <inheritdoc cref="ListControlItem.ToString"/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return base.ToString() ?? nameof(TreeViewItem);
            else
                return Text;
        }

        internal static void OnChildItemAdded(
            TreeViewItem item,
            TreeViewItem? parent,
            TreeView? treeView,
            int index)
        {
            item.Parent = parent;
            item.TreeView = treeView;
            item.Index = index;

            treeView?.RaiseItemAdded(new TreeViewEventArgs(item));
        }

        internal static void OnChildItemRemoved(TreeViewItem item)
        {
            item.Parent = null;

            var oldTreeView = item.TreeView;
            item.TreeView = null;
            item.Index = null;

            oldTreeView?.RaiseItemRemoved(new TreeViewEventArgs(item));
        }

        private void ApplyTreeViewToAllChildren()
        {
            if (!HasItems)
                return;
            foreach (var item in Items)
            {
                item.TreeView = treeView;
                item.ApplyTreeViewToAllChildren();
            }
        }

        private void Items_ItemRemoved(object? sender, int index, TreeViewItem item)
        {
            OnChildItemRemoved(item);
        }

        private void Items_ItemInserted(object? sender, int index, TreeViewItem item)
        {
            OnChildItemAdded(item, this, TreeView, index);
        }
    }
}