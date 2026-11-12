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

        private readonly ObjectUniqueId? buttonIdAdd;
        private readonly ObjectUniqueId? buttonIdRemove;
        private readonly ObjectUniqueId? buttonIdRemoveAll;
        private readonly ObjectUniqueId? buttonIdAddChild;
        private readonly ObjectUniqueId? buttonIdOk;

        private readonly TreeViewPlus treeView = new()
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
        /// </summary>
        /// <param name="source">The list edit source to be edited.</param>
        /// <param name="callback">The callback action to be invoked with the created WindowCollectionEditor.</param>
        public static void ShowCollectionEditor(IListEditSource? source, Action<WindowCollectionEditor>? callback = null)
        {
            var existing = App.FindVisibleWindow<WindowCollectionEditor>();

            if (existing is not null)
            {
                existing.ShowAndFocus();
                App.LogWarning("Collection Editor is already shown. Please close it first.");
            }
            else
            {
                WindowCollectionEditor dialog = new(source ?? new DummyListEditSource());
                callback?.Invoke(dialog);
                dialog.Show();
            }
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

        private void OnRemoveButtonClick(object? sender, EventArgs e)
        {
            treeView.RemoveItemAndSelectSibling(treeView.SelectedItem);
            UpdateButtons();
        }

        private void UpdateButtons()
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

        private void OnTreeViewSelectionChanged(object? sender, EventArgs e)
        {
            UpdateButtons();
            var item = treeView.SelectedItem;
            if (item == null)
            {
                return;
            }
        }

        private void LoadData()
        {
            Clear();
            if (dataSource == null)
                return;
            AddItems(treeView.RootItem, dataSource.RootItems);
        }

        private (string? Title, int? ImageIndex)? GetItemInfo(object? item)
        {
            if (item == null)
                return null;
            var s = dataSource?.GetItemTitle(item);
            int? imageIndex = dataSource?.GetItemImageIndex(item);
            if (string.IsNullOrWhiteSpace(s))
                s = CommonStrings.Default.ListEditDefaultItemTitle;

            return (s, imageIndex);
        }

        private void AddItems(TreeViewItem parentItem, IEnumerable? data)
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

        private void Bind()
        {
            treeView.ImageList = dataSource?.ImageList;
        }

        private void Unbind()
        {
            treeView.ImageList = null;
        }

        private class TreeViewPlus : XTreeView, IEnumerableTree<TreeViewItem>
        {
            public TreeViewPlus()
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
}