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
    /// each represented by a <see cref="TreeViewItem"/>. We suggest to use
    /// <see cref="StdTreeView"/> instead of this control. <see cref="StdTreeView"/>
    /// works faster, especially with large number of items, and has much more features.
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
    /// <see cref="Alternet.Drawing.ImageList"/> to the <see cref="ImageList"/> property and
    /// referencing the index value of an <see cref="Image"/> in the
    /// <see cref="ImageList"/> to assign that <see cref="Image"/>.
    /// Set the <see cref="ImageIndex"/> property to the index value of
    /// the <see cref="Alternet.Drawing.Image"/> that you want to display for all items by default.
    /// Individual items can override the default images by setting the
    /// <see cref="ListControlItem.ImageIndex"/> property.
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
    [DefaultProperty("Items")]
    [DefaultEvent("SelectionChanged")]
    [ControlCategory("Common")]
    public partial class TreeView : Control, ITreeViewItemContainer, IListControlItemContainer
    {
        /// <summary>
        /// The set of flags that are closest to the defaults for the native control under Linux.
        /// </summary>
        public const TreeViewCreateStyle DefaultCreateStyleLinux =
            TreeViewCreateStyle.HasButtons | TreeViewCreateStyle.NoLines;

        /// <summary>
        /// The set of flags that are closest to the defaults for the native control under MacOs.
        /// </summary>
        public const TreeViewCreateStyle DefaultCreateStyleMacOs =
            TreeViewCreateStyle.HasButtons | TreeViewCreateStyle.NoLines
            | TreeViewCreateStyle.FullRowHighlight;

        /// <summary>
        /// The set of flags that are closest to the defaults for the native control under Windows.
        /// </summary>
        public const TreeViewCreateStyle DefaultCreateStyleWin =
            TreeViewCreateStyle.HasButtons | TreeViewCreateStyle.LinesAtRoot;

        private readonly HashSet<TreeViewItem> selectedItems = new();
        private readonly ListControlItemDefaults itemDefaults = new();
        private readonly TreeViewRootItem rootItem;

        private ImageList? imageList = null;
        private TreeViewSelectionMode selectionMode = TreeViewSelectionMode.Single;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public TreeView(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView()
        {
            rootItem = new(this);
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
        public event EventHandler<TreeViewEventArgs>? ItemAdded;

        /// <summary>
        /// Occurs when an item is removed from this <see cref="TreeView"/> control,
        /// at any nesting level.
        /// </summary>
        public event EventHandler<TreeViewEventArgs>? ItemRemoved;

        /// <summary>
        /// Occurs after the tree item is expanded.
        /// </summary>
        public event EventHandler<TreeViewEventArgs>? AfterExpand;

        /// <summary>
        /// Occurs after the tree item is collapsed.
        /// </summary>
        public event EventHandler<TreeViewEventArgs>? AfterCollapse;

        /// <summary>
        /// Occurs before the tree item is expanded. This event can be canceled.
        /// </summary>
        public event EventHandler<TreeViewCancelEventArgs>? BeforeExpand;

        /// <summary>
        /// Occurs before the tree item is collapsed. This event can be canceled.
        /// </summary>
        public event EventHandler<TreeViewCancelEventArgs>? BeforeCollapse;

        /// <summary>
        /// Occurs before the tree item label text is edited. This event can
        /// be canceled.
        /// </summary>
        public event EventHandler<TreeViewEditEventArgs>? BeforeLabelEdit;

        /// <summary>
        /// Occurs after the tree item label text is edited. This event
        /// can be canceled.
        /// </summary>
        public event EventHandler<TreeViewEditEventArgs>? AfterLabelEdit;

        /// <summary>
        /// Occurs after <see cref="TreeViewItem.IsExpanded"/> property
        /// value of a tree item belonging to this <see cref="TreeView"/> changes.
        /// </summary>
        public event EventHandler<TreeViewEventArgs>? ExpandedChanged;

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
        public virtual TreeViewItem? SelectedItem
        {
            get
            {
                return selectedItems.FirstOrDefault();
            }

            set
            {
                CheckDisposed();

                ClearSelectedCore();
                if (value != null)
                    selectedItems.Add(value);
                RaiseSelectionChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets first root item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem? FirstItem
        {
            get
            {
                if (Items.Count == 0)
                    return null;
                return Items[0];
            }
        }

        /// <summary>
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem? LastRootItem
        {
            get
            {
                if (Items.Count == 0)
                    return null;
                return Items[Items.Count - 1];
            }
        }

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual TreeViewItem? LastItem
        {
            get
            {
                static TreeViewItem? GetLastItem(TreeViewItem? item)
                {
                    if (item is null)
                        return null;
                    if (!item.HasItems)
                        return item;
                    var child = item.Items.Last();
                    return GetLastItem(child);
                }

                return GetLastItem(LastRootItem);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TreeView;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return base.Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                base.Handler.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the display of the root node
        /// is suppressed. This effectively causing the first-level nodes to
        /// appear as a series of root nodes.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the root node is hidden;
        /// <see langword="false" />, if the the root node is shown.
        /// The default is <see langword="true" />.
        /// </returns>
        public virtual bool HideRoot
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.HideRoot;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
        public virtual bool VariableRowHeight
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.VariableRowHeight;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
        public virtual bool TwistButtons
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.TwistButtons;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.TwistButtons = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of horizontal pixels between the buttons and
        /// the state images.
        /// </summary>
        [Browsable(false)]
        public virtual uint StateImageSpacing
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.StateImageSpacing;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.StateImageSpacing = value;
            }
        }

        /// <summary>
        /// Gets or sets the current control indentation.
        /// </summary>
        [Browsable(false)]
        public virtual uint Indentation
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.Indentation;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
        public virtual bool RowLines
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.RowLines;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.RowLines = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between tree
        /// items in the tree view control.
        /// </summary>
        /// <value><see langword="true"/> if lines are drawn between tree items in
        /// the tree view control; otherwise,
        /// <see langword="false"/>. The default is <see langword="true"/>.</value>
        public virtual bool ShowLines
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ShowLines;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ShowLines = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between the
        /// tree items that are at the root of the tree view.
        /// </summary>
        /// <value><see langword="true"/> if lines are drawn between the tree
        /// items that are at the root of the tree
        /// view; otherwise, <see langword="false"/>. The default is
        /// <see langword="true"/>.</value>
        public virtual bool ShowRootLines
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ShowRootLines;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ShowRootLines = value;
            }
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
        public virtual bool ShowExpandButtons
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.ShowExpandButtons;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ShowExpandButtons = value;
            }
        }

        /// <summary>
        /// Gets or sets the first fully-visible tree item in the tree view control.
        /// </summary>
        /// <value>A <see cref="TreeViewItem"/> that represents the first
        /// fully-visible tree item in the tree view control.</value>
        [Browsable(false)]
        public virtual TreeViewItem? TopItem
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.TopItem;
            }
        }

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
        public virtual IReadOnlyList<TreeViewItem> SelectedItems
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
                {
                    if (item is null)
                        continue;
                    selectedItems.Add(item);
                }

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
        public virtual TreeViewSelectionMode SelectionMode
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
        /// Gets the root item of the tree view.
        /// </summary>
        [Browsable(false)]
        public TreeViewItem RootItem
        {
            get
            {
                return rootItem;
            }
        }

        /// <summary>
        /// Gets a collection containing all the root items in the control.
        /// </summary>
        public IReadOnlyList<TreeViewItem> Items
        {
            get
            {
                return rootItem.Items;
            }
        }

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
        /// is displayed by setting the <see cref="ListControlItem.ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override
        /// the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public virtual ImageList? ImageList
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
        /// <see cref="ListControlItem.ImageIndex"/> property.
        /// These individual <see cref="TreeViewItem"/> settings will override
        /// the settings in the corresponding <see cref="TreeView"/> properties.
        /// </remarks>
        public virtual int? ImageIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight spans
        /// the width of the tree view control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the selection highlight spans the width of
        /// the tree view control; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public virtual bool FullRowSelect
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.FullRowSelect;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.FullRowSelect = value;
            }
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
        public virtual bool AllowLabelEdit
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.AllowLabelEdit;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.AllowLabelEdit = value;
            }
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Gets a <see cref="ITreeViewHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new ITreeViewHandler Handler
        {
            get
            {
                return (ITreeViewHandler)base.Handler;
            }
        }

        IListControlItemContainer ITreeViewItemContainer.ListContainer => this;

        TreeViewButtonsKind ITreeViewItemContainer.TreeButtons { get; }

        SvgImage? IListControlItemContainer.CheckImageUnchecked { get; }

        SvgImage? IListControlItemContainer.CheckImageChecked { get; }

        SvgImage? IListControlItemContainer.CheckImageIndeterminate { get; }

        AbstractControl? IListControlItemContainer.Control => this;

        IListControlItemDefaults IListControlItemContainer.Defaults => itemDefaults;

        /// <summary>
        /// Expands all child tree items.
        /// </summary>
        public virtual void ExpandAll()
        {
            if (DisposingOrDisposed)
                return;
            Handler.ExpandAll();
        }

        /// <summary>
        /// Sets <see cref="SelectedItem"/> to the item on the root level with the specified index.
        /// </summary>
        /// <param name="index"></param>
        public virtual void SetSelectedIndex(int? index)
        {
            if (index is null)
                SelectedItem = null;
            else
                SelectedItem = Items[index.Value];
        }

        /// <summary>
        /// Collapses all child tree items.
        /// </summary>
        public virtual void CollapseAll()
        {
            if (DisposingOrDisposed)
                return;
            Handler.CollapseAll();
        }

        /// <summary>
        /// Provides tree view item information, at a given client point, in
        /// device-independent units.
        /// </summary>
        /// <param name="point">The <see cref="PointD"/> at which to retrieve
        /// item information.</param>
        /// <returns>The hit test result information.</returns>
        /// <remarks>
        /// Use this method to determine whether a point is located in a
        /// <see cref="TreeViewItem"/> and where within the
        /// item the point is located, such as on the label or image area.
        /// </remarks>
        public virtual TreeViewHitTestInfo HitTest(PointD point)
        {
            if (!DisposingOrDisposed)
            {
                var htResult = Handler.HitTest(point, out var item, out var locations);
                if (htResult)
                {
                    return new TreeViewHitTestInfo(locations, item);
                }
            }

            return TreeViewHitTestInfo.Empty;
        }

        /// <summary>
        /// Gets <see cref="TreeViewHitTestLocations"/> at a given
        /// client point, in device-independent units.
        /// </summary>
        /// <param name="point">The <see cref="PointD"/> at which to retrieve
        /// item information.</param>
        /// <returns>The hit test result.</returns>
        /// <remarks>
        /// Use this method to determine whether a point is located in a
        /// <see cref="TreeViewItem"/> and where within the
        /// item the point is located, such as on the label or image area.
        /// </remarks>
        public virtual TreeViewHitTestLocations HitTestLocation(PointD point)
        {
            if (DisposingOrDisposed)
                return default;
            Handler.HitTest(point, out _, out var locations, false);
            return locations;
        }

        /// <summary>
        /// Gets tree view item at the current cursor position.
        /// </summary>
        /// <returns>Tree view item if success; <c>null</c> otherwise.</returns>
        public virtual TreeViewItem? GetNodeAtMouseCursor()
        {
            return GetNodeAt(Mouse.GetPosition(this));
        }

        /// <summary>
        /// Retrieves the tree view item that is at the specified point.
        /// </summary>
        /// <param name="pt">
        /// The <see cref="PointD" /> to evaluate and retrieve the node from.
        /// </param>
        /// <returns>
        /// The <see cref="TreeViewItem" /> at the specified point, in tree view
        /// (client) coordinates,
        /// or <see langword="null" /> if there is no item at that location.
        /// </returns>
        public virtual TreeViewItem? GetNodeAt(PointD pt)
        {
            if (DisposingOrDisposed)
                return null;
            Handler.HitTest(pt, out var item, out _);
            return item;
        }

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as necessary.
        /// </summary>
        /// <remarks>
        /// When the <see cref="EnsureVisible"/> method is called, the tree is
        /// expanded and scrolled to ensure that the current tree
        /// item is visible in the <see cref="TreeView"/>. This method is useful
        /// if you are selecting a tree item in code based on
        /// certain criteria. By calling this method after you select the item,
        /// the user can see and interact with the selected item.
        /// </remarks>
        public virtual void EnsureVisible(TreeViewItem? item)
        {
            if (item is null)
                return;

            item.EnsureVisible();
        }

        /// <summary>
        /// Scrolls the specified item into view.
        /// </summary>
        public virtual void ScrollIntoView(TreeViewItem? item)
        {
            if (item is null)
                return;

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
        public virtual void ClearSelected()
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
        public virtual void SetSelected(TreeViewItem item, bool value)
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
            if (DisposingOrDisposed)
                return;
            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="item">The object to be added to the end of the
        /// <see cref="Items"/> collection.</param>
        public virtual void Add(TreeViewItem item)
        {
            rootItem.Add(item);
        }

        /// <summary>
        /// Selects and shows specified item in the control.
        /// </summary>
        public virtual void SelectAndShowItem(TreeViewItem? item)
        {
            if (DisposingOrDisposed)
                return;
            if (item != null)
            {
                DoInsideUpdate(() =>
                {
                    ClearSelectedCore();
                    item.IsSelected = true;
                    ScrollIntoView(item);
                    EnsureVisible(item);
                });
            }
        }

        /// <summary>
        /// Adds <see cref="string"/> to the end of the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to be added to the end of the
        /// <see cref="Items"/> collection.</param>
        public virtual TreeViewItem Add(string s)
        {
            var item = new TreeViewItem(s);
            rootItem.Add(item);
            return item;
        }

        /// <summary>
        /// Raises the <see cref="AfterCollapse"/> event and calls
        /// <see cref="OnAfterCollapse"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseAfterCollapse(TreeViewEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            e.Item.IsExpanded = false;
            OnAfterCollapse(e);
            AfterCollapse?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterExpand"/> event and calls
        /// <see cref="OnAfterExpand"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseAfterExpand(TreeViewEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            e.Item.IsExpanded = true;
            OnAfterExpand(e);
            AfterExpand?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeCollapse"/> event and calls
        /// <see cref="OnBeforeCollapse"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewCancelEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseBeforeCollapse(TreeViewCancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnBeforeCollapse(e);
            BeforeCollapse?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="BeforeLabelEdit"/> event and calls
        /// <see cref="OnBeforeLabelEdit"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEditEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseBeforeLabelEdit(TreeViewEditEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnBeforeLabelEdit(e);
            BeforeLabelEdit?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AfterLabelEdit"/> event and calls
        /// <see cref="OnAfterLabelEdit"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEditEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseAfterLabelEdit(TreeViewEditEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnAfterLabelEdit(e);
            AfterLabelEdit?.Invoke(this, e);
        }

        /// <summary>
        /// Removes selected items from the control.
        /// </summary>
        public virtual void RemoveSelected()
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
        public virtual void RemoveAll()
        {
            if (Items.Count == 0)
                return;
            BeginUpdate();
            try
            {
                ClearSelected();
                rootItem.Clear();
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
        public virtual void RemoveItemAndSelectSibling(TreeViewItem? item)
        {
            if (DisposingOrDisposed)
                return;
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
        /// Selects first item in the control.
        /// </summary>
        public virtual void SelectFirstItem()
        {
            SelectedItem = FirstItem;
        }

        /// <summary>
        /// Changes visual style of the control to look like <see cref="StdListBox"/>.
        /// </summary>
        public virtual void MakeAsListBox()
        {
            if (DisposingOrDisposed)
                return;
            Handler.MakeAsListBox();
        }

        /// <summary>
        /// Raises the <see cref="BeforeExpand"/> event and calls
        /// <see cref="OnBeforeExpand"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewCancelEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseBeforeExpand(TreeViewCancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnBeforeExpand(e);
            BeforeExpand?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ExpandedChanged"/> event and calls
        /// <see cref="OnExpandedChanged"/>.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        public void RaiseExpandedChanged(TreeViewEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnExpandedChanged(e);
            ExpandedChanged?.Invoke(this, e);
        }

        internal void RaiseItemAdded(TreeViewEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (e == null)
                return;

            OnItemAdded(e);
            ItemAdded?.Invoke(this, e);
        }

        internal void RaiseItemRemoved(TreeViewEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            if (e == null)
                return;

            OnItemRemoved(e);
            ItemRemoved?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return (ControlFactory.Handler as IWxControlFactoryHandler)?.CreateTreeViewHandler(this)
                ?? throw new Exception("Failed to create TreeView handler.");
        }

        /// <summary>
        /// Called after a tree item is expanded.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnAfterExpand(TreeViewEventArgs e)
        {
        }

        /// <summary>
        /// Called before a tree item is expanded.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewCancelEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
        }

        /// <summary>
        /// Called before a tree item label is edited.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEditEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnBeforeLabelEdit(TreeViewEditEventArgs e)
        {
        }

        /// <summary>
        /// Called after a tree item label is edited.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEditEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnAfterLabelEdit(TreeViewEditEventArgs e)
        {
        }

        /// <summary>
        /// Called before a tree item is collapsed.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewCancelEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnBeforeCollapse(TreeViewCancelEventArgs e)
        {
        }

        /// <summary>
        /// Called after a tree item is collapsed.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnAfterCollapse(TreeViewEventArgs e)
        {
        }

        /// <summary>
        /// Called after <see cref="TreeViewItem.IsExpanded"/> property value of
        /// a tree item belonging to this <see cref="TreeView"/> changes.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnExpandedChanged(TreeViewEventArgs e)
        {
        }

        /// <summary>
        /// Called when an item is added to this <see cref="TreeView"/> control,
        /// at any nesting level.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnItemAdded(TreeViewEventArgs e)
        {
        }

        /// <summary>
        /// Called when an item is removed from this <see cref="TreeView"/> control,
        /// at any nesting level.
        /// </summary>
        /// <param name="e">An <see cref="TreeViewEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnItemRemoved(TreeViewEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
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

        double ITreeViewItemContainer.GetLevelMargin()
        {
            return StdTreeView.DefaultLevelMargin;
        }

        void ITreeViewItemContainer.BeginUpdate()
        {
        }

        void ITreeViewItemContainer.EndUpdate()
        {
        }

        void ITreeViewItemContainer.TreeChanged()
        {
        }

        void ITreeViewItemContainer.RaiseItemAdded(TreeViewItem item)
        {
            var e = new TreeViewEventArgs(item);
            RaiseItemAdded(e);
        }

        void ITreeViewItemContainer.RaiseItemRemoved(TreeViewItem item)
        {
            var e = new TreeViewEventArgs(item);
            RaiseItemRemoved(e);
        }

        void ITreeViewItemContainer.RaiseAfterExpand(TreeViewItem item)
        {
        }

        void ITreeViewItemContainer.RaiseAfterCollapse(TreeViewItem item)
        {
        }

        void ITreeViewItemContainer.RaiseBeforeExpand(TreeViewItem item, ref bool cancel)
        {
        }

        void ITreeViewItemContainer.RaiseBeforeCollapse(TreeViewItem item, ref bool cancel)
        {
        }

        void ITreeViewItemContainer.RaiseExpandedChanged(TreeViewItem item)
        {
        }

        ListControlItem? IListControlItemContainer.SafeItem(int index)
        {
            return Items[index] as ListControlItem;
        }

        string IListControlItemContainer.GetItemText(int index, bool forDisplay)
        {
            return Items[index].Text;
        }

        int IListControlItemContainer.GetItemCount()
        {
            return Items.Count;
        }
    }
}