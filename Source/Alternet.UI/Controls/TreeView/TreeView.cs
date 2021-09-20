using Alternet.Drawing;
using Alternet.Base.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items with optional images, each represented by a <see cref="TreeViewItem"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Items"/> collection holds all the <see cref="TreeViewItem"/> objects that are assigned to the <see cref="TreeView"/> control.
    /// The items in this collection are referred to as the root items. Any item that is subsequently added to a root item is referred to as a child node.
    /// Each <see cref="TreeViewItem"/> can contain a collection of other <see cref="TreeViewItem"/> objects.
    /// </para>
    /// <para>
    /// You can display images next to the tree nodes by assigning an <see cref="ImageList"/> to the <see cref="TreeView.ImageList"/> property and
    /// referencing the index value of an <see cref="Image"/> in the <see cref="ImageList"/> to assign that <see cref="Image"/>.
    /// Set the <see cref="TreeView.ImageIndex"/> property to the index value of the <see cref="Image"/> that you want to display for all items by default.
    /// Individual items can override the default images by setting the <see cref="TreeViewItem.ImageIndex"/> property.
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
    public class TreeView : Control
    {
        private ImageList? imageList = null;

        private HashSet<TreeViewItem> selectedItems = new HashSet<TreeViewItem>();

        private TreeViewSelectionMode selectionMode = TreeViewSelectionMode.Single;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedItem"/> property or the <see cref="SelectedItems"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the selected item in the <see cref="TreeView"/> has been changed.
        /// This can be useful when you need to display information in other controls based on the current selection in the <see cref="TreeView"/>.
        /// <para>
        /// If the <see cref="SelectionMode"/> property is set to <see cref="TreeViewSelectionMode.Multiple"/>,
        /// any change to the <see cref="SelectedItems"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// <para>
        /// The <see cref="SelectedItems"/> collection changes whenever an individual <see cref="TreeViewItem"/> selection changes.
        /// The property change can occur programmatically or when the user selects an item or clears the selection of an item.
        /// </para>
        /// </remarks>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Occurs when an item is added to this <see cref="TreeView"/> control, at any nesting level.
        /// </summary>
        public event EventHandler<TreeViewItemContainmentEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from this <see cref="TreeView"/> control, at any nesting level.
        /// </summary>
        public event EventHandler<TreeViewItemContainmentEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<TreeViewItemExpandedChangedEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event EventHandler<TreeViewItemExpandedChangedEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs after <see cref="TreeViewItem.IsExpanded"/> property value of a tree item belonging to this <see cref="TreeView"/> changes.
        /// </summary>
        public event EventHandler<TreeViewItemExpandedChangedEventArgs>? ExpandedChanged;

        /// <summary>
        /// Occurs when the <see cref="ImageList"/> property value changes.
        /// </summary>
        public event EventHandler? ImageListChanged;

        /// <summary>
        /// Gets or sets the currently selected item in the <see cref="TreeView"/>.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> object that represents the current selection in the control, or <c>null</c> if no item is selected.</value>
        /// <remarks>
        /// <para>
        /// You can use this property to determine the item that is selected in the <see cref="TreeView"/>.
        /// If the <see cref="SelectionMode"/> property of the <see cref="TreeView"/> is set to
        /// <see cref="TreeViewSelectionMode.Multiple"/> and multiple items are selected in the tree, this property can return any selected item.
        /// </para>
        /// <para>
        /// To retrieve a collection containing all selected items in a multiple-selection <see cref="TreeView"/>, use the <see cref="SelectedItems"/> property.
        /// </para>
        /// </remarks>
        public TreeViewItem? SelectedItem
        {
            get
            {
                CheckDisposed();
                return SelectedItems.FirstOrDefault();
            }

            set
            {
                CheckDisposed();

                ClearSelected();
                if (value != null)
                    selectedItems.Add(value);
                RaiseSelectionChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection containing the currently selected items in the <see cref="TreeView"/>.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{TreeViewItem}"/> containing the currently selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="TreeView"/>, this property returns a collection containing all the items that are selected
        /// in the <see cref="TreeView"/>. For a single-selection <see cref="TreeView"/>, this property returns a collection containing a
        /// single element containing the only selected item in the <see cref="TreeView"/>.
        /// </remarks>
        public IReadOnlyList<TreeViewItem> SelectedItems
        {
            get
            {
                CheckDisposed();
                return selectedItems.ToArray();
            }

            set
            {
                CheckDisposed();

                selectedItems.Clear();
                foreach (var item in value)
                    selectedItems.Add(item);
                RaiseSelectionChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the method in which items are selected in the <see cref="TreeView"/>.
        /// </summary>
        /// <value>One of the <see cref="TreeViewSelectionMode"/> values. The default is <see cref="TreeViewSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine how many items in the <see cref="TreeView"/>
        /// a user can select at one time.
        /// </remarks>
        public TreeViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return selectionMode;
            }

            set
            {
                CheckDisposed();

                if (selectionMode == value)
                    return;

                selectionMode = value;

                SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a collection containing all the root items in the control.
        /// </summary>
        /// <value>A <see cref="Collection{TreeViewItem}"/> that contains all the root items in the <see cref="TreeView"/> control.</value>
        /// <remarks>
        /// Using the <see cref="Collection{TreeViewItem}"/> returned by this property, you can add items, remove items, and obtain a count of items.
        /// The <see cref="Items"/> property holds a collection of <see cref="TreeViewItem"/> objects, each of which has a <see cref="Items"/> property
        /// that can contain its own child items collection.
        /// </remarks>
        public Collection<TreeViewItem> Items { get; } = new Collection<TreeViewItem> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying item images in the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the images to use for items in the <see cref="TreeView"/>. The default is <c>null</c>.</value>
        /// <remarks>
        /// If the <see cref="ImageList"/> property value is anything other than <c>null</c>, all the tree items display the
        /// first <see cref="Image"/> stored in the <see cref="ImageList"/>.
        /// You can specify which images from the list are displayed for items by default by setting the <see cref="ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image is displayed by setting the <see cref="TreeViewItem.ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public ImageList? ImageList
        {
            get
            {
                CheckDisposed();
                return imageList;
            }

            set
            {
                CheckDisposed();

                if (imageList == value)
                    return;

                imageList = value;

                ImageListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the index of the image that is displayed for the items in this <see cref="TreeView"/> by default.
        /// </summary>
        /// <value>
        /// The zero-based index of the image in the <see cref="ImageList"/> that
        /// is displayed for the items in this <see cref="TreeView"/> by default. The default is <c>null</c>.
        /// The <c>null</c> value means no image is displayed for the items by default.
        /// </value>
        /// <remarks>
        /// The effect of setting this property depends on the value of the <see cref="ImageList"/> property.
        /// You can specify which images from the list are displayed for items by default by setting the <see cref="ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image is displayed by setting the <see cref="TreeViewItem.ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public int? ImageIndex { get; set; }

        /// <summary>
        /// Unselects all items in the <see cref="TreeView"/>.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the <see cref="SelectedItem"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the tree.
        /// </remarks>
        public void ClearSelected()
        {
            ClearSelectedCore();
            RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Selects or clears the selection for the specified item in a <see cref="TreeView"/>.
        /// </summary>
        /// <param name="item">The item in a <see cref="TreeView"/> to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item; otherwise, false.</param>
        /// <remarks>
        /// You can use this method to set the selection of items in a multiple-selection <see cref="TreeView"/>.
        /// To select an item in a single-selection <see cref="TreeView"/>, use the <see cref="SelectedItem"/> property.
        /// </remarks>
        public void SetSelected(TreeViewItem item, bool value)
        {
            CheckDisposed();

            var changed = SetSelectedCore(item, value);

            if (changed)
                RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls <see cref="OnSelectionChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseSelectionChanged(EventArgs e)
        {
            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterCollapse"/> event and calls <see cref="OnAfterCollapse"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/> that contains the event data.</param>
        public void RaiseAfterCollapse(TreeViewItemExpandedChangedEventArgs e)
        {
            e.Item.IsExpanded = false;
            OnAfterCollapse(e);
            AfterCollapse?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterExpand"/> event and calls <see cref="OnAfterExpand"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/> that contains the event data.</param>
        public void RaiseAfterExpand(TreeViewItemExpandedChangedEventArgs e)
        {
            e.Item.IsExpanded = true;
            OnAfterExpand(e);
            AfterExpand?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ExpandedChanged"/> event and calls <see cref="OnExpandedChanged"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/> that contains the event data.</param>
        public void RaiseExpandedChanged(TreeViewItemExpandedChangedEventArgs e)
        {
            OnExpandedChanged(e);
            ExpandedChanged?.Invoke(this, e);
        }

        internal void RaiseItemAdded(TreeViewItemContainmentEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnItemAdded(e);
            ItemAdded?.Invoke(this, e);
        }

        internal void RaiseItemRemoved(TreeViewItemContainmentEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnItemRemoved(e);
            ItemRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Called after a tree item is expanded.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnAfterExpand(TreeViewItemExpandedChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called after a tree item is collapsed.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnAfterCollapse(TreeViewItemExpandedChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called after <see cref="TreeViewItem.IsExpanded"/> property value of a tree item belonging to this <see cref="TreeView"/> changes.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnExpandedChanged(TreeViewItemExpandedChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when an item is added to this <see cref="TreeView"/> control, at any nesting level.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemContainmentEventArgs"/> that contains the event data.</param>
        protected virtual void OnItemAdded(TreeViewItemContainmentEventArgs e)
        {
        }

        /// <summary>
        /// Called when an item is removed from this <see cref="TreeView"/> control, at any nesting level.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemContainmentEventArgs"/> that contains the event data.</param>
        protected virtual void OnItemRemoved(TreeViewItemContainmentEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="SelectedItem"/> property or the <see cref="SelectedItems"/> collection has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>See <see cref="SelectionChanged"/> for details.</remarks>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<TreeViewItem> e)
        {
            TreeViewItem.OnChildItemRemoved(e.Item);
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<TreeViewItem> e)
        {
            TreeViewItem.OnChildItemAdded(e.Item, null, this, e.Index);
        }

        private void ClearSelectedCore()
        {
            selectedItems.Clear();
        }

        private bool SetSelectedCore(TreeViewItem item, bool value)
        {
            bool changed;
            if (value)
                changed = selectedItems.Add(item);
            else
                changed = selectedItems.Remove(item);

            return changed;
        }
    }
}