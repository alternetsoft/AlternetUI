using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item (also known as a node) of a <see cref="TreeView"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Items"/> collection holds all the child <see cref="TreeViewItem"/> objects assigned to the current <see cref="TreeViewItem"/>.
    /// You can add or remove a <see cref="TreeViewItem"/>; when you do this, all child tree items are added or removed.
    /// Each <see cref="TreeViewItem"/> can contain a collection of other <see cref="TreeViewItem"/> objects.
    /// </para>
    /// <para>
    /// The <see cref="TreeViewItem"/> text label is set by setting the <see cref="Text"/> property explicitly.
    /// The alternative is to create the tree item using one of the <see cref="TreeViewItem"/> constructors
    /// that has a string parameter that represents the <see cref="Text"/> property.
    /// The label is displayed next to the <see cref="TreeViewItem"/> image, if one is displayed.
    /// </para>
    /// <para>
    /// To display images next to the tree items, assign an <see cref="ImageList"/> to the <see cref="TreeView.ImageList"/> property
    /// of the parent <see cref="TreeView"/> control and assign an <see cref="Image"/> by referencing its index value in
    /// the <see cref="TreeView.ImageList"/> property. To do that, set the <see cref="ImageIndex"/> property to the index value
    /// of the <see cref="Image"/> you want to display in the <see cref="TreeViewItem"/>.
    /// </para>
    /// <para>
    /// <see cref="TreeView"/> items can be expanded to display the next level of child items.
    /// The user can expand the <see cref="TreeViewItem"/> by clicking the expand button, if one is displayed
    /// next to the <see cref="TreeViewItem"/>, or you can expand the <see cref="TreeViewItem"/> by calling the <see cref="TreeViewItem.Expand"/> method.
    /// To expand all the child item levels in the <see cref="Items"/> collection, call the <see cref="TreeViewItem.ExpandAll"/> method.
    /// You can collapse the child <see cref="TreeViewItem"/> level by calling the <see cref="TreeViewItem.Collapse"/> method,
    /// or the user can press the expand button, if one is displayed next to the <see cref="TreeViewItem"/>.
    /// You can also call the <see cref="TreeViewItem.Toggle"/> method to alternate between the expanded and collapsed states.
    /// </para>
    /// </remarks>
    public class TreeViewItem
    {
        private string text = "";
        private TreeView? treeView;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class with default values.
        /// </summary>
        public TreeViewItem()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class with the specified item text.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        public TreeViewItem(string text) : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">
        /// The zero-based index of the image within the <see cref="TreeView.ImageList"/>
        /// associated with the <see cref="TreeView"/> that contains the item.
        /// </param>
        public TreeViewItem(string text, int imageIndex) : this(text)
        {
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets or sets an object that contains data to associate with the item.
        /// </summary>
        /// <value>An object that contains information that is associated with the item.</value>
        /// <remarks>
        /// The <see cref="Tag"/> property can be used to store any object that you want to associate with an item.
        /// Although you can store any item, the <see cref="Tag"/> property is typically used to store string information
        /// about the item, such as a unique identifier or the index position of the item's data in a database.
        /// </remarks>
        public object? Tag { get; set; }

        /// <summary>
        /// Gets the parent tree item of the current item.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> that represents the parent of the current tree item, or <c>null</c> if the item is a root.</value>
        /// <remarks>If the tree item is at the root level, the <see cref="Parent"/> property returns <c>null</c>.</remarks>
        public TreeViewItem? Parent { get; private set; }

        /// <summary>
        /// Gets the owner <see cref="TreeView"/> that the tree item is assigned to.
        /// </summary>
        /// <value>
        /// A <see cref="TreeView"/> that represents the parent tree view that the tree item is assigned to,
        /// or <c>null</c> if the node has not been assigned to a tree view.
        /// </value>
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
        public string Text
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
            }
        }

        /// <summary>
        /// Gets the position of the tree item in the tree item collection.
        /// </summary>
        /// <value>
        /// A zero-based index value that represents the position of the tree item in
        /// a <see cref="Collection{TreeViewItem}"/>, or <c>null</c> if the item is not contained in any collection.
        /// An item can be contained in either <see cref="Items"/> or <see cref="TreeView.Items"/> collection.
        /// </value>
        public int? Index { get; private set; }

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the item.
        /// </summary>
        /// <value>
        /// The zero-based index of the image in the <see cref="TreeView.ImageList"/> that is displayed for the item. The default is <c>null</c>.
        /// The <c>null</c> value means the default image specified by <see cref="TreeView.ImageIndex"/> property is used.
        /// </value>
        /// <remarks>
        /// The effect of setting this property depends on the value of the <see cref="TreeView.ImageList"/> property.
        /// You can specify which images from the list are displayed for items by default by setting the <see cref="TreeView.ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image is displayed by setting the <see cref="ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public int? ImageIndex { get; set; }

        /// <summary>
        /// Gets a value indicating whether the tree item is in the expanded state.
        /// </summary>
        /// <value><c>true</c> if the tree item is in the expanded state; otherwise, <c>false</c>. Default is <c>false</c>.</value>
        /// <remarks>
        /// To track changing of this state, see <see cref="TreeView.ExpandedChanged"/>, <see cref="TreeView.AfterExpand"/>, <see cref="TreeView.AfterCollapse"/> events.
        /// </remarks>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets the collection of child <see cref="TreeViewItem"/> of the current tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Items"/> property can hold a collection of other <see cref="TreeViewItem"/> objects.
        /// Each of the tree items in the collection has an <see cref="Items"/> property that can contain its own item collection.
        /// </remarks>
        public Collection<TreeViewItem> Items { get; } = new Collection<TreeViewItem> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Removes the current tree item from the tree view control.
        /// </summary>
        /// <remarks>
        /// When the <see cref="Remove"/> method is called, the tree item, and any child tree items that are
        /// assigned to the <see cref="TreeViewItem"/>, are removed from the <see cref="TreeView"/>.
        /// </remarks>
        public void Remove()
        {
            if (Parent == null)
            {
                if (TreeView != null)
                    TreeView.Items.Remove(this);
            }
            else
                Parent.Items.Remove(this);
        }

        // todo
        /// <summary>
        /// Expands the tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Expand"/> method expands the <see cref="TreeViewItem"/>, while leaving the child items expanded state unchanged.
        /// </remarks>
        public void Expand()
        {
            IsExpanded = true;
        }

        /// <summary>
        /// Expands this <see cref="TreeViewItem"/> and all the child tree items.
        /// </summary>
        /// <remarks>
        /// The <see cref="ExpandAll"/> method expands this <see cref="TreeViewItem"/> and all the child tree items assigned to the <see cref="Items"/> collection.
        /// </remarks>
        public void ExpandAll()
        {
            // todo
        }

        /// <summary>
        /// Collapses the tree item.
        /// </summary>
        /// <remarks>
        /// The <see cref="Collapse"/> method collapses the <see cref="TreeViewItem"/>, while leaving the child items expanded state unchanged.
        /// </remarks>
        public void Collapse()
        {
            IsExpanded = false;
        }

        /// <summary>
        /// Collapses this <see cref="TreeViewItem"/> and all the child tree items.
        /// </summary>
        /// <remarks>
        /// The <see cref="CollapseAll"/> method collapses this <see cref="TreeViewItem"/> and all the child tree items assigned to the <see cref="Items"/> collection.
        /// </remarks>
        public void CollapseAll()
        {
            // todo
        }

        /// <summary>
        /// Toggles the tree item to either the expanded or collapsed state.
        /// </summary>
        /// <remarks>
        /// The tree item is toggled to the state opposite its current state, either expanded or collapsed.
        /// </remarks>
        public void Toggle()
        {
            IsExpanded = !IsExpanded;
        }

        internal static void OnChildItemAdded(TreeViewItem item, TreeViewItem? parent, TreeView? treeView, int index)
        {
            item.Parent = parent;
            item.TreeView = treeView;
            item.Index = index;

            if (treeView != null)
                treeView.RaiseItemAdded(new TreeViewItemContainmentEventArgs(item));
        }

        internal static void OnChildItemRemoved(TreeViewItem item)
        {
            item.Parent = null;

            var oldTreeView = item.TreeView;
            item.TreeView = null;
            item.Index = null;

            if (oldTreeView != null)
                oldTreeView.RaiseItemRemoved(new TreeViewItemContainmentEventArgs(item));
        }

        private void ApplyTreeViewToAllChildren()
        {
            foreach (var item in Items)
            {
                item.TreeView = treeView;
                item.ApplyTreeViewToAllChildren();
            }
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<TreeViewItem> e)
        {
            OnChildItemAdded(e.Item, this, TreeView, e.Index);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<TreeViewItem> e)
        {
            OnChildItemRemoved(e.Item);
        }
    }
}