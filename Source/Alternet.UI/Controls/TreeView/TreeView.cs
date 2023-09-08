using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items with optional images,
    /// each represented by a <see cref="TreeViewItem"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Items"/> collection holds all the
    /// <see cref="TreeViewItem"/> objects that are assigned to the
    /// <see cref="TreeView"/> control.
    /// The items in this collection are referred to as the root items. Any item
    /// that is subsequently added to a root item is referred to as a child item.
    /// Each <see cref="TreeViewItem"/> can contain a collection of other
    /// <see cref="TreeViewItem"/> objects.
    /// </para>
    /// <para>
    /// You can display images next to the tree items by assigning an
    /// <see cref="ImageList"/> to the <see cref="TreeView.ImageList"/> property and
    /// referencing the index value of an <see cref="Image"/> in the
    /// <see cref="ImageList"/> to assign that <see cref="Image"/>.
    /// Set the <see cref="TreeView.ImageIndex"/> property to the index value of
    /// the <see cref="Image"/> that you want to display for all items by default.
    /// Individual items can override the default images by setting the
    /// <see cref="TreeViewItem.ImageIndex"/> property.
    /// </para>
    /// <para>
    /// <see cref="TreeView"/> items can be expanded to display the next level of
    /// child items.
    /// The user can expand the <see cref="TreeViewItem"/> by clicking the
    /// expand button, if one is displayed
    /// next to the <see cref="TreeViewItem"/>, or you can expand the
    /// <see cref="TreeViewItem"/> by calling the
    /// <see cref="TreeViewItem.Expand"/> method.
    /// To expand all the child item levels in the <see cref="Items"/> collection,
    /// call the <see cref="TreeViewItem.ExpandAll"/> method.
    /// You can collapse the child <see cref="TreeViewItem"/> level by calling the
    /// <see cref="TreeViewItem.Collapse"/> method,
    /// or the user can press the expand button, if one is displayed next to the
    /// <see cref="TreeViewItem"/>.
    /// You can also call the <see cref="TreeViewItem.Toggle"/> method to
    /// alternate between the expanded and collapsed states.
    /// </para>
    /// </remarks>
    public class TreeView : Control
    {
        private readonly HashSet<TreeViewItem> selectedItems = new();

        private ImageList? imageList = null;

        private TreeViewSelectionMode selectionMode = TreeViewSelectionMode.Single;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView()
        {
            Items.ItemInsertedFast += Items_ItemInsertedFast;
            Items.ItemRemovedFast += Items_ItemRemovedFast;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedItem"/> property or the
        /// <see cref="SelectedItems"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when
        /// the selected item in the <see cref="TreeView"/> has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the <see cref="TreeView"/>.
        /// <para>
        /// If the <see cref="SelectionMode"/> property is set to
        /// <see cref="TreeViewSelectionMode.Multiple"/>,
        /// any change to the <see cref="SelectedItems"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// <para>
        /// The <see cref="SelectedItems"/> collection changes whenever an
        /// individual <see cref="TreeViewItem"/> selection changes.
        /// The property change can occur programmatically or when the user selects
        /// an item or clears the selection of an item.
        /// </para>
        /// </remarks>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Occurs when an item is added to this <see cref="TreeView"/> control, at
        /// any nesting level.
        /// </summary>
        public event EventHandler<TreeViewItemContainmentEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from this <see cref="TreeView"/> control,
        /// at any nesting level.
        /// </summary>
        public event EventHandler<TreeViewItemContainmentEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<TreeViewItemExpandedChangedEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event
            EventHandler<TreeViewItemExpandedChangedEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs before the tree item is expanded. This event can be canceled.
        /// </summary>
        public event
            EventHandler<TreeViewItemExpandedChangingEventArgs>? BeforeExpand;

        /// <summary>
        /// Occurs before the tree item is collapsed. This event can be canceled.
        /// </summary>
        public event
            EventHandler<TreeViewItemExpandedChangingEventArgs>? BeforeCollapse;

        /// <summary>
        /// Occurs before the tree item label text is edited. This event can
        /// be canceled.
        /// </summary>
        public event EventHandler<TreeViewItemLabelEditEventArgs>? BeforeLabelEdit;

        /// <summary>
        /// Occurs after the tree item label text is edited. This event
        /// can be canceled.
        /// </summary>
        public event EventHandler<TreeViewItemLabelEditEventArgs>? AfterLabelEdit;

        /// <summary>
        /// Occurs after <see cref="TreeViewItem.IsExpanded"/> property
        /// value of a tree item belonging to this <see cref="TreeView"/> changes.
        /// </summary>
        public event
            EventHandler<TreeViewItemExpandedChangedEventArgs>? ExpandedChanged;

        /// <summary>
        /// Occurs when the <see cref="ImageList"/> property value changes.
        /// </summary>
        public event EventHandler? ImageListChanged;

        /// <summary>
        /// Gets or sets the currently selected item in the <see cref="TreeView"/>.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> object that represents the
        /// current selection in the control, or <c>null</c> if no item is selected.
        /// </value>
        /// <remarks>
        /// <para>
        /// You can use this property to determine the item that is selected in
        /// the <see cref="TreeView"/>.
        /// If the <see cref="SelectionMode"/> property of the
        /// <see cref="TreeView"/> is set to
        /// <see cref="TreeViewSelectionMode.Multiple"/> and multiple items
        /// are selected in the tree, this property can return any selected item.
        /// </para>
        /// <para>
        /// To retrieve a collection containing all selected items in a
        /// multiple-selection <see cref="TreeView"/>, use the
        /// <see cref="SelectedItems"/> property.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public TreeViewItem? SelectedItem
        {
            get
            {
                CheckDisposed();
                return selectedItems.FirstOrDefault();
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

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.TreeView;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                if (Handler is not NativeTreeViewHandler handler)
                    return true;
                return handler.NativeControl.HasBorder;
            }

            set
            {
                CheckDisposed();
                if (Handler is not NativeTreeViewHandler handler)
                    return;
                handler.NativeControl.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the display of the root node
        /// is supressed. This effectively causing the first-level nodes to
        /// appear as a series of root nodes.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the root node is hidden;
        /// <see langword="false" />, if the the root node is shown.
        /// The default is <see langword="true" />.
        /// </returns>
        public bool HideRoot
        {
            get
            {
                return Handler.HideRoot;
            }

            set
            {
                Handler.HideRoot = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether row heights become big enough
        /// to fit the content.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if row heights grow to fit the content;
        /// <see langword="false" />, if all rows use the largest row height.
        /// The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// This property doesn't work on all the platforms.
        /// </remarks>
        public bool VariableRowHeight
        {
            get
            {
                return Handler.VariableRowHeight;
            }

            set
            {
                Handler.VariableRowHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to select alternative style
        /// of +/- buttons and to show rotating ("twisting") arrows instead.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if twisted buttons are used;
        /// <see langword="false" />, if default buttons are used.
        /// The default is <see langword="true" />.
        /// </returns>
        /// <remarks>
        /// Currently this style is only implemented under Windows Vista and later
        /// versions and is ignored under the other platforms. Notice that under
        /// Vista this style results in the same appearance as used by the tree
        /// control in Explorer and other built-in programs and so using it
        /// may be preferable to the default style.
        /// </remarks>
        public bool TwistButtons
        {
            get
            {
                return Handler.TwistButtons;
            }

            set
            {
                Handler.TwistButtons = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of horizontal pixels between the buttons and
        /// the state images.
        /// </summary>
        public uint StateImageSpacing
        {
            get
            {
                return Handler.StateImageSpacing;
            }

            set
            {
                Handler.StateImageSpacing = value;
            }
        }

        /// <summary>
        /// Gets or sets the current control indentation.
        /// </summary>
        public uint Indentation
        {
            get
            {
                return Handler.Indentation;
            }

            set
            {
                Handler.Indentation = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a contrasting
        /// border between displayed rows.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if row lines are shown;
        /// <see langword="false" />, if row lines are hidden.
        /// The default is <see langword="false" />.
        /// </returns>
        public bool RowLines
        {
            get
            {
                return Handler.RowLines;
            }

            set
            {
                Handler.RowLines = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="TreeViewHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new TreeViewHandler Handler
        {
            get
            {
                CheckDisposed();
                return (TreeViewHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between tree
        /// items in the tree view control.
        /// </summary>
        /// <value><see langword="true"/> if lines are drawn between tree items in
        /// the tree view control; otherwise,
        /// <see langword="false"/>. The default is <see langword="true"/>.</value>
        public bool ShowLines
        {
            get => Handler.ShowLines;
            set => Handler.ShowLines = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between the
        /// tree items that are at the root of the tree view.
        /// </summary>
        /// <value><see langword="true"/> if lines are drawn between the tree
        /// items that are at the root of the tree
        /// view; otherwise, <see langword="false"/>. The default is
        /// <see langword="true"/>.</value>
        public bool ShowRootLines
        {
            get => Handler.ShowRootLines;
            set => Handler.ShowRootLines = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether expand buttons are displayed
        /// next to tree items that contain child
        /// tree items.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if expand buttons are displayed next to tree
        /// items that contain
        /// child tree items; otherwise, <see langword="false"/>. The default is
        /// <see langword="true"/>.
        /// </value>
        public bool ShowExpandButtons
        {
            get => Handler.ShowExpandButtons;
            set => Handler.ShowExpandButtons = value;
        }

        /// <summary>
        /// Gets or sets the first fully-visible tree item in the tree view control.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> that represents the first
        /// fully-visible tree item in the tree view control.</value>
        [Browsable(false)]
        public TreeViewItem? TopItem => Handler.TopItem;

        /// <summary>
        /// Gets a collection containing the currently selected items in the
        /// <see cref="TreeView"/>.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{TreeViewItem}"/> containing the
        /// currently selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="TreeView"/>, this property returns
        /// a collection containing all the items that are selected
        /// in the <see cref="TreeView"/>. For a single-selection
        /// <see cref="TreeView"/>, this property returns a collection containing a
        /// single element containing the only selected item in the
        /// <see cref="TreeView"/>.
        /// </remarks>
        [Browsable(false)]
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
        /// Gets or sets the method in which items are selected in the
        /// <see cref="TreeView"/>.
        /// </summary>
        /// <value>One of the <see cref="TreeViewSelectionMode"/> values. The
        /// default is <see cref="TreeViewSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine
        /// how many items in the <see cref="TreeView"/>
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
        /// <value>A <see cref="Collection{TreeViewItem}"/> that contains all
        /// the root items in the <see cref="TreeView"/> control.</value>
        /// <remarks>
        /// Using the <see cref="Collection{TreeViewItem}"/> returned by this
        /// property, you can add items, remove items, and obtain a count of items.
        /// The <see cref="Items"/> property holds a collection of
        /// <see cref="TreeViewItem"/> objects, each of which has a
        /// <see cref="Items"/> property
        /// that can contain its own child items collection.
        /// </remarks>
        public Collection<TreeViewItem> Items { get; } =
            new Collection<TreeViewItem> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Gets or sets the <see cref="ImageList"/> to use when displaying item
        /// images in the control.
        /// </summary>
        /// <value>An <see cref="ImageList"/> that contains the images to use for
        /// items in the <see cref="TreeView"/>. The default is <c>null</c>.</value>
        /// <remarks>
        /// If the <see cref="ImageList"/> property value is anything other than
        /// <c>null</c>, all the tree items display the
        /// first <see cref="Image"/> stored in the <see cref="ImageList"/>.
        /// You can specify which images from the list are displayed for items
        /// by default by setting the <see cref="ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image
        /// is displayed by setting the <see cref="TreeViewItem.ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override
        /// the settings in the corresponding <see cref="TreeView"/> properties.
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
        /// Gets or sets the index of the image that is displayed for the items
        /// in this <see cref="TreeView"/> by default.
        /// </summary>
        /// <value>
        /// The zero-based index of the image in the <see cref="ImageList"/> that
        /// is displayed for the items in this <see cref="TreeView"/> by default.
        /// The default is <c>null</c>.
        /// The <c>null</c> value means no image is displayed for the items
        /// by default.
        /// </value>
        /// <remarks>
        /// The effect of setting this property depends on the value of the
        /// <see cref="ImageList"/> property.
        /// You can specify which images from the list are displayed for items
        /// by default by setting the <see cref="ImageIndex"/> property.
        /// Individual <see cref="TreeViewItem"/> objects can specify which image
        /// is displayed by setting the
        /// <see cref="TreeViewItem.ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override
        /// the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public int? ImageIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight spans
        /// the width of the tree view control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the selection highlight spans the width of
        /// the tree view control; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public bool FullRowSelect
        {
            get => Handler.FullRowSelect;
            set => Handler.FullRowSelect = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the label text of the tree
        /// items can be edited.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the label text of the tree items can be
        /// edited; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public bool AllowLabelEdit
        {
            get => Handler.AllowLabelEdit;
            set => Handler.AllowLabelEdit = value;
        }

        /// <summary>
        /// Expands all child tree items.
        /// </summary>
        public void ExpandAll()
        {
            Handler.ExpandAll();
        }

        /// <summary>
        /// Collapses all child tree items.
        /// </summary>
        public void CollapseAll()
        {
            Handler.CollapseAll();
        }

        /// <summary>
        /// Provides tree view item information, at a given client point, in
        /// device-independent units (1/96th inch per
        /// unit).
        /// </summary>
        /// <param name="point">The <see cref="Point"/> at which to retrieve
        /// item information.</param>
        /// <returns>The hit test result information.</returns>
        /// <remarks>
        /// Use this method to determine whether a point is located in a
        /// <see cref="TreeViewItem"/> and where within the
        /// item the point is located, such as on the label or image area.
        /// </remarks>
        public TreeViewHitTestInfo HitTest(Point point)
        {
            return Handler.HitTest(point);
        }

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as
        /// necessary.
        /// </summary>
        /// <remarks>
        /// When the <see cref="EnsureVisible"/> method is called, the tree is
        /// expanded and scrolled to ensure that the current tree
        /// item is visible in the <see cref="TreeView"/>. This method is useful
        /// if you are selecting a tree item in code based on
        /// certain criteria. By calling this method after you select the item,
        /// the user can see and interact with the
        /// selected item.
        /// </remarks>
        public virtual void EnsureVisible(TreeViewItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.EnsureVisible();
        }

        /// <summary>
        /// Scrolls the specified item into view.
        /// </summary>
        public virtual void ScrollIntoView(TreeViewItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.ScrollIntoView();
        }

        /// <summary>
        /// Unselects all items in the <see cref="TreeView"/>.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the
        /// <see cref="SelectedItem"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the tree.
        /// </remarks>
        public void ClearSelected()
        {
            ClearSelectedCore();
            RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Selects or clears the selection for the specified item in a
        /// <see cref="TreeView"/>.
        /// </summary>
        /// <param name="item">The item in a <see cref="TreeView"/> to select or
        /// clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item;
        /// otherwise, <c>false</c>.</param>
        /// <remarks>
        /// You can use this method to set the selection of items in a
        /// multiple-selection <see cref="TreeView"/>.
        /// To select an item in a single-selection <see cref="TreeView"/>,
        /// use the <see cref="SelectedItem"/> property.
        /// </remarks>
        public void SetSelected(TreeViewItem item, bool value)
        {
            CheckDisposed();

            var changed = SetSelectedCore(item, value);

            if (changed)
                RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls
        /// <see cref="OnSelectionChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        public void RaiseSelectionChanged(EventArgs e)
        {
            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterCollapse"/> event and calls
        /// <see cref="OnAfterCollapse"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseAfterCollapse(TreeViewItemExpandedChangedEventArgs e)
        {
            e.Item.IsExpanded = false;
            OnAfterCollapse(e);
            AfterCollapse?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterExpand"/> event and calls
        /// <see cref="OnAfterExpand"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseAfterExpand(TreeViewItemExpandedChangedEventArgs e)
        {
            e.Item.IsExpanded = true;
            OnAfterExpand(e);
            AfterExpand?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeCollapse"/> event and calls
        /// <see cref="OnBeforeCollapse"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangingEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseBeforeCollapse(TreeViewItemExpandedChangingEventArgs e)
        {
            OnBeforeCollapse(e);
            BeforeCollapse?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeLabelEdit"/> event and calls
        /// <see cref="OnBeforeLabelEdit"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemLabelEditEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseBeforeLabelEdit(TreeViewItemLabelEditEventArgs e)
        {
            OnBeforeLabelEdit(e);
            BeforeLabelEdit?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterLabelEdit"/> event and calls
        /// <see cref="OnAfterLabelEdit"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemLabelEditEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseAfterLabelEdit(TreeViewItemLabelEditEventArgs e)
        {
            OnAfterLabelEdit(e);
            AfterLabelEdit?.Invoke(this, e);
        }

        /// <summary>
        /// Removes selected items from the control.
        /// </summary>
        public void RemoveSelected()
        {
            BeginUpdate();
            try
            {
                IReadOnlyList<TreeViewItem> items = SelectedItems;
                ClearSelected();
                foreach (var item in items)
                    item.Remove();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Removed all items from the control.
        /// </summary>
        public void RemoveAll()
        {
            BeginUpdate();
            try
            {
                ClearSelected();
                Items.Clear();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Removes item and selects its sibling (next or previous on the same level).
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItemAndSelectSibling(TreeViewItem? item)
        {
            if (item == null)
                return;
            var newItem = item?.NextOrPrevSibling;

            BeginUpdate();
            try
            {
                item!.Remove();
                SelectedItem = newItem;
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Changes visual style of the control to look like <see cref="ListBox"/>.
        /// </summary>
        public void MakeAsListBox()
        {
            BeginIgnoreRecreate();
            try
            {
                FullRowSelect = true;
                ShowRootLines = false;
                ShowLines = false;
                TwistButtons = false;
                StateImageSpacing = 0;
                if (Indentation > 3)
                    Indentation = 3;
            }
            finally
            {
                EndIgnoreRecreate();
            }
        }

        /// <summary>
        /// Raises the <see cref="BeforeExpand"/> event and calls
        /// <see cref="OnBeforeExpand"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangingEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseBeforeExpand(TreeViewItemExpandedChangingEventArgs e)
        {
            OnBeforeExpand(e);
            BeforeExpand?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ExpandedChanged"/> event and calls
        /// <see cref="OnExpandedChanged"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/>
        /// that contains the event data.</param>
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
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnAfterExpand(TreeViewItemExpandedChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called before a tree item is expanded.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangingEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnBeforeExpand(TreeViewItemExpandedChangingEventArgs e)
        {
        }

        /// <summary>
        /// Called before a tree item label is edited.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemLabelEditEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnBeforeLabelEdit(TreeViewItemLabelEditEventArgs e)
        {
        }

        /// <summary>
        /// Called after a tree item label is edited.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemLabelEditEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnAfterLabelEdit(TreeViewItemLabelEditEventArgs e)
        {
        }

        /// <summary>
        /// Called before a tree item is collapsed.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangingEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnBeforeCollapse(
            TreeViewItemExpandedChangingEventArgs e)
        {
        }

        /// <summary>
        /// Called after a tree item is collapsed.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnAfterCollapse(TreeViewItemExpandedChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called after <see cref="TreeViewItem.IsExpanded"/> property value of
        /// a tree item belonging to this <see cref="TreeView"/> changes.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemExpandedChangedEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnExpandedChanged(
            TreeViewItemExpandedChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when an item is added to this <see cref="TreeView"/> control,
        /// at any nesting level.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemContainmentEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnItemAdded(TreeViewItemContainmentEventArgs e)
        {
        }

        /// <summary>
        /// Called when an item is removed from this <see cref="TreeView"/> control,
        /// at any nesting level.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewItemContainmentEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnItemRemoved(TreeViewItemContainmentEventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="SelectedItem"/> property or the
        /// <see cref="SelectedItems"/> collection has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        /// <remarks>See <see cref="SelectionChanged"/> for details.</remarks>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateTreeViewHandler(this);
        }

        private void Items_ItemRemovedFast(object? sender, int index, TreeViewItem item)
        {
            TreeViewItem.OnChildItemRemoved(item);
        }

        private void Items_ItemInsertedFast(object? sender, int index, TreeViewItem item)
        {
            TreeViewItem.OnChildItemAdded(item, null, this, index);
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