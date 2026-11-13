using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that allows editing a collection of items in a list or tree structure.
    /// This window provides a user interface for adding, removing, and modifying items in the collection,
    /// as well as applying changes to the underlying data source.
    /// </summary>
    [ControlCategory(KnownControlCategory.Hidden)]
    public class WindowCollectionEditor : Window
    {
        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            LeftVisible = false,
            BottomVisible = false,
            RightVisible = false,
        };

        private readonly ToolBar toolbar = new()
        {
            TextVisible = true,
            ItemSize = 32,
        };

        private readonly ObjectUniqueId buttonIdAdd;
        private readonly ObjectUniqueId buttonIdRemove;
        private readonly ObjectUniqueId buttonIdRemoveAll;
        private readonly ObjectUniqueId buttonIdAddChild;
        private readonly ObjectUniqueId buttonIdOk;

        private readonly XEnumerableTreeView treeView = new()
        {
            HasBorder = false,
        };

        private IListEditSource? dataSource;
        private bool autoSuffixNewTitle = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowCollectionEditor"/> class.
        /// </summary>
        /// <param name="source">The list edit source.</param>
        public WindowCollectionEditor(IListEditSource source)
        {
            Icon = KnownIcons.Default;
            Layout = LayoutStyle.Vertical;
            Size = new(750, 600);
            MinimumSize = new(550, 350);
            toolbar.Parent = this;

            panel.RightPanelWidth = Size.Width / 2;
            panel.VerticalAlignment = VerticalAlignment.Fill;
            panel.Parent = this;

            treeView.SelectionChanged += OnTreeViewSelectionChanged;
            treeView.Parent = panel.FillPanel;

            var s = CommonStrings.Default.WindowTitleCollectionEditor;

            string? elementName = null;

            if (source.Instance is FrameworkElement element)
            {
                elementName = element.Name ?? element.GetType().FullName;
            }
            else
                elementName = source.Instance?.GetType().FullName;

            var propName = source.PropInfo?.Name;

            if (propName is not null && elementName is not null)
            {
                s += " - " + elementName + " - " + propName;
            }

            Title = s;
            StartLocation = WindowStartLocation.CenterScreen;

            if (source.AllowAdd)
            {
                buttonIdAdd = toolbar.AddSpeedBtn(KnownButton.Add, OnAddButtonClick);
            }

            if (source.AllowSubItems && source.AllowAdd)
            {
                buttonIdAddChild = toolbar.AddSpeedBtn(KnownButton.AddChild, OnAddChildButtonClick);
            }

            if (source.AllowDelete)
            {
                buttonIdRemove = toolbar.AddSpeedBtn(KnownButton.Remove, OnRemoveButtonClick);
                buttonIdRemoveAll = toolbar.AddSpeedBtn(KnownButton.Clear, OnRemoveAllButtonClick);
            }

            if (source.AllowApplyData)
            {
                buttonIdOk = toolbar.AddSpeedBtn(KnownButton.OK, OnOkButtonClick);
            }

            var buttonIdCancel = toolbar.AddSpeedBtn(KnownButton.Cancel, OnCancelButtonClick);

            toolbar.Margin = (0, 0, 0, ToolBar.DefaultDistanceToContent);
            toolbar.SetVisibleBorders(false, false, false, true);
            BackgroundColor = ThemedColors.Window;

            DataSource = source;
        }

        /// <summary>
        /// Gets the main panel of the collection editor window.
        /// </summary>
        public SplittedPanel MainPanel => panel;

        /// <summary>
        /// Gets the toolbar of the collection editor window.
        /// </summary>
        public ToolBar Toolbar => toolbar;

        /// <summary>
        /// Gets the tree view control that displays the collection items in the collection editor window.
        /// </summary>
        public XTreeView MainView => treeView;

        /// <summary>
        /// Gets the unique identifier of the "Add" button in the collection editor window.
        /// </summary>
        public ObjectUniqueId ButtonIdAdd => buttonIdAdd;

        /// <summary>
        /// Gets the unique identifier of the "Remove" button in the collection editor window.
        /// </summary>
        public ObjectUniqueId ButtonIdRemove => buttonIdRemove;

        /// <summary>
        /// Gets the unique identifier of the "Remove All" button in the collection editor window.
        /// </summary>
        public ObjectUniqueId ButtonIdRemoveAll => buttonIdRemoveAll;

        /// <summary>
        /// Gets the unique identifier of the "Add Child" button in the collection editor window.
        /// </summary>
        public ObjectUniqueId ButtonIdAddChild => buttonIdAddChild;
        
        /// <summary>
        /// Gets the unique identifier of the "OK" button in the collection editor window.
        /// </summary>
        public ObjectUniqueId ButtonIdOk => buttonIdOk;

        /// <summary>
        /// Gets or sets the data source for the list editor.
        /// </summary>
        public virtual IListEditSource? DataSource
        {
            get
            {
                return dataSource;
            }

            set
            {
                if (dataSource == value)
                    return;

                Unbind();
                dataSource = value;
                LoadData();
                Bind();
                UpdateButtons();
            }
        }

        /// <summary>
        /// Shows the collection editor window for the specified list edit source.
        /// This method creates an instance of the specified <typeparamref name="TWindow"/> type,
        /// which must be a subclass of <see cref="WindowCollectionEditor"/>.
        /// </summary>
        /// <typeparam name="TWindow">The type of the window to be created.</typeparam>
        /// <param name="source">The list edit source to be edited.</param>
        /// <param name="callback">The callback action to be invoked with the created window.</param>
        public static void ShowCollectionEditor<TWindow>(IListEditSource? source, Action<TWindow>? callback = null)
            where TWindow : WindowCollectionEditor
        {
            var existing = App.FindVisibleWindow<TWindow>();

            if (existing is not null)
            {
                existing.ShowAndFocus();
                App.LogWarning("Collection Editor is already shown. Please close it first.");
            }
            else
            {
                TWindow dialog = (TWindow)Activator.CreateInstance(typeof(TWindow), source ?? new DummyListEditSource())!;
                callback?.Invoke(dialog);
                dialog.Show();
            }
        }


        /// <summary>
        /// Shows the collection editor window for the specified list edit source.
        /// </summary>
        /// <param name="source">The list edit source to be edited.</param>
        /// <param name="callback">The callback action to be invoked with the created WindowCollectionEditor.</param>
        public static void ShowCollectionEditor(IListEditSource? source, Action<WindowCollectionEditor>? callback = null)
        {
            ShowCollectionEditor<WindowCollectionEditor>(source, callback);
        }

        /// <summary>
        /// Handles the click event of the "Cancel" button in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCancelButtonClick(object? sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the "OK" button in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnOkButtonClick(object? sender, EventArgs e)
        {
            SetFocusIfPossible();

            if (DataSource?.Instance is not null)
            {
                SaveData();
                Designer?.RaisePropertyChanged(DataSource?.Instance, DataSource?.PropInfo?.Name);
            }

            Close();
        }

        /// <summary>
        /// Handles the click event of the "Remove All" button in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnRemoveAllButtonClick(object? sender, EventArgs e)
        {
            Clear();
            UpdateButtons();
        }

        /// <summary>
        /// Handles the click event of the "Add Child" button in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnAddChildButtonClick(object? sender, EventArgs e)
        {
            AddItem(treeView.SelectedItem);
        }

        /// <summary>
        /// Handles the click event of the "Add" button in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnAddButtonClick(object? sender, EventArgs e)
        {
            AddItem(treeView.SelectedItem?.Parent);
        }

        /// <summary>
        /// Saves the data from the collection editor back to the underlying data source.
        /// </summary>
        protected virtual void SaveData()
        {
            if (dataSource is null || dataSource.Instance is null)
                return;
            dataSource.ApplyData(treeView);
        }

        /// <summary>
        /// Clears all items from the collection editor window.
        /// </summary>
        protected virtual void Clear()
        {
            treeView.RemoveAll();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically suffix
        /// new item titles with a unique identifier.
        /// </summary>
        public virtual bool AutoSuffixNewTitle
        {
            get
            {
                return autoSuffixNewTitle;
            }

            set
            {
                if (autoSuffixNewTitle == value)
                    return;
                autoSuffixNewTitle = value;
            }
        }

        /// <summary>
        /// Adds a new item to the collection editor window.
        /// </summary>
        /// <param name="parentItem">The parent item to which the new item will be added.</param>
        protected virtual void AddItem(TreeViewItem? parentItem)
        {
            parentItem ??= treeView.RootItem;

            if (dataSource == null)
                return;

            var item = dataSource.CreateNewItem();

            if (item == null)
                return;

            var itemInfo = GetItemInfo(item);

            var imageIndex = itemInfo?.ImageIndex ?? -1;
            var newTitle = itemInfo?.Title ?? CommonStrings.Default.ListEditDefaultItemTitle;

            if (AutoSuffixNewTitle)
                newTitle += " " + LogUtils.GenNewId();

            AssemblyUtils.TrySetMemberValue(item, "Text", newTitle);

            var treeItem = new TreeViewItem(newTitle, imageIndex)
            {
                Tag = item,
            };

            parentItem.IsExpanded = true;
            parentItem.Add(treeItem);
            treeView.SelectItem(treeItem);
            UpdateButtons();
        }

        /// <summary>
        /// Handles the click event of the "Remove" button in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnRemoveButtonClick(object? sender, EventArgs e)
        {
            treeView.RemoveItemAndSelectSibling(treeView.SelectedItem);
            UpdateButtons();
        }
        
        /// <summary>
        /// Updates the enabled state of the toolbar buttons in the collection editor window.
        /// </summary>
        protected virtual void UpdateButtons()
        {
            var canAdd = false;
            var canAddChild = false;
            var canRemove = false;
            var canRemoveAll = false;
            var canApply = false;
            if (dataSource != null)
            {
                var itemSelected = treeView.SelectedItem != null;
                canAdd = dataSource.AllowAdd;
                canAddChild = itemSelected && canAdd && dataSource.AllowSubItems;
                canRemove = itemSelected && dataSource.AllowDelete;
                canRemoveAll = dataSource.AllowDelete && treeView.RootItem.ItemCount > 0;
                canApply = dataSource.AllowApplyData;
            }

            toolbar.SetToolEnabled(buttonIdAdd, canAdd);
            toolbar.SetToolEnabled(buttonIdRemove, canRemove);
            toolbar.SetToolEnabled(buttonIdRemoveAll, canRemoveAll);
            toolbar.SetToolEnabled(buttonIdAddChild, canAddChild);
            toolbar.SetToolEnabled(buttonIdOk, canApply);
        }

        /// <summary>
        /// Handles the selection changed event of the tree view in the collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnTreeViewSelectionChanged(object? sender, EventArgs e)
        {
            UpdateButtons();
            var item = treeView.SelectedItem;
            if (item == null)
            {
                return;
            }
        }

        /// <summary>
        /// Loads the data from the data source into the tree view.
        /// </summary>
        protected virtual void LoadData()
        {
            Clear();
            if (dataSource == null)
                return;
            AddItems(treeView.RootItem, dataSource.RootItems);
        }
        
        /// <summary>
        /// Gets the title and image index for the specified item.
        /// </summary>
        /// <param name="item">The item for which to get the information.</param>
        /// <returns>A tuple containing the title and image index, or null if the item is null.</returns>   
        protected virtual (string? Title, int? ImageIndex)? GetItemInfo(object? item)
        {
            if (item == null)
                return null;
            var s = dataSource?.GetItemTitle(item);
            int? imageIndex = dataSource?.GetItemImageIndex(item);
            if (string.IsNullOrWhiteSpace(s))
                s = CommonStrings.Default.ListEditDefaultItemTitle;

            return (s, imageIndex);
        }

        /// <summary>
        /// Adds the specified data items as child nodes to the given parent tree view item.
        /// </summary>
        /// <param name="parentItem">The parent tree view item.</param>
        /// <param name="data">The data items to add.</param>
        protected virtual void AddItems(TreeViewItem parentItem, IEnumerable? data)
        {
            if (data == null)
                return;
            foreach (var item in data)
            {
                var itemInfo = GetItemInfo(item);
                var treeItem = new TreeViewItem(itemInfo!.Value.Title!, itemInfo!.Value.ImageIndex)
                {
                    Tag = dataSource?.CloneItem(item),
                };

                var subItems = dataSource?.GetChildren(item);
                if (subItems != null)
                    AddItems(treeItem, subItems);

                parentItem.Add(treeItem);
            }
        }

        /// <summary>
        /// Binds the data source to the tree view.
        /// </summary>
        protected virtual void Bind()
        {
            treeView.ImageList = dataSource?.ImageList;
        }

        /// <summary>
        /// Unbinds the data source from the tree view.
        /// </summary>
        protected virtual void Unbind()
        {
            treeView.ImageList = null;
        }

        private class XEnumerableTreeView : XTreeView, IEnumerableTree<TreeViewItem>
        {
            public XEnumerableTreeView()
            {
                // AllowDrop = true;
                // DragDrop += OnDragDropEvent;
                // DragOver += OnDragOverEvent;
                // DragEnter += OnDragEnterEvent;
                // DragLeave += OnDragLeaveEvent;
                // DragStart += OnDragStartEvent;
                // MouseDown += TreeViewPlus_MouseDown;
                // MouseUp += TreeViewPlus_MouseUp;
                // Click += TreeViewPlus_Click;
                // PreviewMouseUp += TreeViewPlus_PreviewMouseUp;
                // MouseLeftButtonUp += TreeViewPlus_MouseLeftButtonUp;
            }

            IEnumerable<TreeViewItem>? IEnumerableTree<TreeViewItem>.GetChildren(TreeViewItem item)
            {
                if (item == null || !item.HasItems)
                    return [];
                return item.Items;
            }

            IEnumerable? IEnumerableTree.GetChildren(object item)
            {
                if (item is not TreeViewItem treeItem || !treeItem.HasItems)
                    return Array.Empty<TreeViewItem>();
                return treeItem.Items;
            }

            object? IEnumerableTree.GetData(object item)
            {
                if (item is not TreeViewItem treeItem)
                    return null;
                return treeItem.Tag;
            }

            IEnumerator<TreeViewItem> IEnumerable<TreeViewItem>.GetEnumerator()
            {
                return RootItem.Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return RootItem.Items.GetEnumerator();
            }

            internal static IDataObject GetDataObject()
            {
                var result = new DataObject();
                result.SetData(DataFormats.Text, "Test data string.");
                return result;
            }

            internal void OnDragStartEvent(object? sender, DragStartEventArgs e)
            {
                if (e.DistanceIsLess)
                    return;
                if (e.TimeIsGreater)
                {
                    e.Cancel = true;
                    return;
                }

                e.DragStarted = true;
                var result = DoDragDrop(GetDataObject(), DragDropEffects.Copy | DragDropEffects.Move);
                if (result == DragDropEffects.None)
                    return;
            }

            internal void TreeViewPlus_Click(object sender, EventArgs e)
            {
                App.DebugLog("Click");
            }

            internal void TreeViewPlus_MouseUp(object sender, MouseEventArgs e)
            {
                App.DebugLog("MouseUp");
            }

            internal void TreeViewPlus_MouseDown(object sender, MouseEventArgs e)
            {
                App.DebugLog("MouseDown");
            }

            internal void OnDragDropEvent(object? sender, DragEventArgs e)
            {
                App.DebugLog($"DragDrop: {e.MouseClientLocation}, {e.Effect}");
                App.DebugLog($"Dropped Data: {DataObject.ToDebugString(e.Data)}");
            }

            internal void OnDragOverEvent(object? sender, DragEventArgs e)
            {
                App.DebugLogReplace(
                    $"DragOver: {e.MouseClientLocation}, {e.Effect}", "DragOver");
            }

            internal void TreeViewPlus_MouseLeftButtonUp(object sender, MouseEventArgs e)
            {
                App.DebugLog("MouseLeftButtonUp");
            }

            internal void TreeViewPlus_PreviewMouseUp(object sender, MouseEventArgs e)
            {
                App.DebugLog("PreviewMouseUp");
            }

            internal void OnDragEnterEvent(object? sender, DragEventArgs e)
            {
                App.DebugLog($"DragEnter: {e.MouseClientLocation}, {e.Effect}");
            }

            internal void OnDragLeaveEvent(object? sender, EventArgs e)
            {
                App.DebugLog("DragLeave");
            }
        }
    }

    /// <summary>
    /// Represents a generic collection editor window that allows editing a collection of items
    /// using a specified item editor control.
    /// </summary>
    /// <typeparam name="TItemEditor">The type of the item editor control.</typeparam>
    public partial class WindowCollectionEditor<TItemEditor> : WindowCollectionEditor
        where TItemEditor : AbstractControl, new()
    {
        private readonly TItemEditor itemEditor = new()
        {
            VerticalAlignment = VerticalAlignment.Fill,
            HorizontalAlignment = HorizontalAlignment.Fill,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowCollectionEditor{TItemEditor}"/> class.
        /// </summary>
        /// <param name="source">The list edit source.</param>
        public WindowCollectionEditor(IListEditSource source) : base(source)
        {
            MainPanel.RightVisible = true;
            itemEditor.Parent = MainPanel.RightPanel;
            itemEditor.Enabled = false;
            MainView.RootItem.ItemSource.ItemInserted += OnItemInserted;
            MainView.RootItem.ItemSource.ItemRemoved += OnItemRemoved;
        }

        /// <summary>
        /// Gets the item editor control used for editing individual items in the collection.
        /// </summary>
        public TItemEditor ItemEditor => itemEditor;

        /// <summary>
        /// Called when an item is inserted into the collection editor's tree view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index at which the item was inserted.</param>
        /// <param name="item">The item that was inserted.</param>
        protected virtual void OnItemInserted(object? sender, int index, TreeViewItem item)
        {
            UpdateItemEditor();
        }
        
        /// <summary>
        /// Called when an item is removed from the collection editor's tree view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index at which the item was removed.</param>
        /// <param name="item">The item that was removed.</param>
        protected virtual void OnItemRemoved(object? sender, int index, TreeViewItem item)
        {
            UpdateItemEditor();
        }

        /// <inheritdoc/>
        protected override void Clear()
        {
            base.Clear();
            UpdateItemEditor();
        }

        /// <inheritdoc/>
        protected override void OnTreeViewSelectionChanged(object? sender, EventArgs e)
        {
            base.OnTreeViewSelectionChanged(sender, e);
            UpdateItemEditor();
        }

        /// <summary>
        /// Updates the item editor control based on the currently selected item
        /// in the collection editor's tree view.
        /// </summary>
        protected virtual void UpdateItemEditor()
        {
        }
    }

    /// <summary>
    /// Represents a specialized collection editor window for editing collections of <see cref="CalendarItem"/>
    /// in a <see cref="SchedulerDocument"/>.
    /// </summary>
    public partial class WindowSchedulerDocumentBrowser : WindowCollectionEditor<ScrollableCalendarItemPicker>
    {
        private bool suspendValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSchedulerDocumentBrowser"/> class
        /// with the specified list edit source.
        /// </summary>
        /// <param name="source">The list edit source.</param>
        public WindowSchedulerDocumentBrowser(IListEditSource source) : base(source)
        {
            AutoSuffixNewTitle = false;
            ItemEditor.ValueChanged += OnValueChanged;
            ItemEditor.ScrolledControl.MinWidth = 350;
            ItemEditor.ScrolledControl.MaxWidth = 350;
        }

        /// <summary>
        /// Shows the scheduler document browser window for the specified <see cref="SchedulerDocument"/>.
        /// </summary>
        /// <param name="document">The scheduler document to browse.</param>
        public static void ShowDocumentBrowser(SchedulerDocument document)
        {
            IListEditSource source = new SchedulerDocumentListEditSource(document);

            WindowCollectionEditor.ShowCollectionEditor<WindowSchedulerDocumentBrowser>(source, OnCreate);

            void OnCreate(WindowSchedulerDocumentBrowser e)
            {
                e.Title = CommonStrings.Default.WindowTitleSchedulerDocumentBrowser;
            }
        }

        /// <summary>
        /// Updates the enabled state of the item editor control based on the
        /// specified <paramref name="enabled"/> value. Also update the visibility of the text
        /// in the item editor control based on the enabled state.
        /// </summary>
        /// <param name="enabled">A value indicating whether the item editor should be enabled.</param>
        protected virtual void UpdateItemEditorState(bool enabled)
        {
            ItemEditor.Enabled = enabled;
            ItemEditor.ScrolledControl.ValuesTransparent = !enabled;
        }

        /// <inheritdoc/>
        protected override void UpdateItemEditor()
        {
            var item = MainView.SelectedItem;
            if (item == null)
            {
                UpdateItemEditorState(false);
                return;
            }

            UpdateItemEditorState(true);

            if (item.Tag is CalendarItem calendarItem)
            {
                suspendValueChanged = true;
                try
                {
                    ItemEditor.Value = calendarItem;
                }
                finally
                {
                    suspendValueChanged = false;
                }
            }
        }

        /// <summary>
        /// Handles the value changed event of the calendar control in the date collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnValueChanged(object? sender, EventArgs e)
        {
            if (suspendValueChanged)
                return;
            var item = MainView.SelectedItem;
            if (item is not null)
            {
                var value = ItemEditor.Value;
                (item.Tag as CalendarItem)?.Assign(value);
                item.Text = value.Text ?? CommonStrings.Default.UntitledItemText;
            }

            UpdateItemEditorState(item is not null);
        }
    }

    /// <summary>
    /// Represents a specialized collection editor window for editing collections of <see cref="DateOnly"/>.
    /// </summary>
    public partial class WindowDateCollectionEditor : WindowCollectionEditor<XCalendar>
    {
        private bool suspendValueChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDateCollectionEditor"/>
        /// class with the specified list edit source.
        /// </summary>
        /// <param name="source">The list edit source.</param>
        public WindowDateCollectionEditor(IListEditSource source) : base(source)
        {
            AutoSuffixNewTitle = false;
            ItemEditor.ValueChanged += OnValueChanged;
        }

        /// <inheritdoc/>
        protected override void UpdateItemEditor()
        {
            var item = MainView.SelectedItem;
            if (item == null)
            {
                ItemEditor.Enabled = false;
                return;
            }

            ItemEditor.Enabled = true;

            if (item.Tag is DateOnly date)
            {
                suspendValueChanged = true;
                try
                {
                    ItemEditor.Value = date;
                }
                finally
                {
                    suspendValueChanged = false;
                }
            }
        }

        /// <summary>
        /// Handles the value changed event of the calendar control in the date collection editor window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnValueChanged(object? sender, EventArgs e)
        {
            if (suspendValueChanged)
                return;
            var item = MainView.SelectedItem;
            if (item is not null)
            {
                var value = ItemEditor.Value;
                item.Tag = value;
                item.Text = value.ToString();
            }

            ItemEditor.Enabled = item is not null;
        }
    }
}