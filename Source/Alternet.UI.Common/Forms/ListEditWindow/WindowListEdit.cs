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
    [ControlCategory("Hidden")]
    internal class WindowListEdit : Window
    {
        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            LeftVisible = false,
            BottomVisible = false,
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

        private readonly PropertyGrid propertyGrid = new()
        {
            HasBorder = false,
            Features = PropertyGridFeature.QuestionCharInNullable,
        };

        private IListEditSource? dataSource;
        private object? lastPropInstance;

        public WindowListEdit(IListEditSource source)
        {
            Layout = LayoutStyle.Vertical;
            Size = new(750, 600);
            MinimumSize = new(550, 350);
            toolbar.Parent = this;

            panel.RightPanelWidth = Size.Width / 2;
            panel.VerticalAlignment = VerticalAlignment.Fill;
            panel.Parent = this;

            treeView.SelectionChanged += OnTreeViewSelectionChanged;
            treeView.Parent = panel.FillPanel;

            propertyGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAllAfterSetValue;
            propertyGrid.Parent = panel.RightPanel;

            var s = CommonStrings.Default.WindowTitleListEdit;

            string? elementName = null;

            if (source.Instance is FrameworkElement element)
            {
                elementName = element.Name ?? element.GetType().FullName;
            }
            else
                elementName = source.Instance?.GetType().FullName;

            var propName = source.PropInfo?.Name;

            if(propName is not null && elementName is not null)
            {
                s += " - " + elementName + " - " + propName;
            }

            Title = s;
            StartLocation = WindowStartLocation.CenterScreen;

            if (source.AllowAdd)
            {
                buttonIdAdd = toolbar.AddSpeedBtn(KnownButton.Add, OnAddButtonClick);
            }

            if(source.AllowSubItems && source.AllowAdd)
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

            propertyGrid.SuggestedInitDefaults();
            propertyGrid.PropertyChanged += OnPropertyGridPropertyChanged;

            toolbar.Margin = (0, 0, 0, ToolBar.DefaultDistanceToContent);
            toolbar.SetVisibleBorders(false, false, false, true);
            BackgroundColor = SystemColors.Window;

            ComponentDesigner.SafeDefault.ObjectPropertyChanged += OnDesignerPropertyChanged;

            Closing += Window_Closing;
            Closed += Window_Closed;
            Disposed += Window_Disposed;

            DataSource = source;
        }

        public IListEditSource? DataSource
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

        internal void SaveData()
        {
            if (dataSource is null || dataSource.Instance is null)
                return;
            dataSource.ApplyData(treeView);
        }

        internal void Clear()
        {
            propertyGrid.Clear();
            treeView.RemoveAll();
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
        }

        private void Window_Closing(object? sender, WindowClosingEventArgs e)
        {
            ComponentDesigner.SafeDefault.ObjectPropertyChanged -= OnDesignerPropertyChanged;
        }

        private void OnDesignerPropertyChanged(object? sender, ObjectPropertyChangedEventArgs e)
        {
            if (IsDisposed)
                return;
            if (lastPropInstance == null)
                return;

            if (e.Instance == lastPropInstance)
                UpdatePropertyGrid(lastPropInstance);

            var treeItem = treeView.SelectedItem;

            if (treeItem != null && treeItem?.Tag == lastPropInstance)
            {
                var itemInfo = GetItemInfo(lastPropInstance);

                treeItem.Text = itemInfo!.Value.Title
                    ?? CommonStrings.Default.ListEditDefaultItemTitle;
                treeItem.ImageIndex = itemInfo.Value.ImageIndex;
            }
        }

        private void Window_Disposed(object? sender, EventArgs e)
        {
            ComponentDesigner.SafeDefault.ObjectPropertyChanged -= OnDesignerPropertyChanged;
        }

        private void OnCancelButtonClick(object? sender, EventArgs e)
        {
            Close();
        }

        private void OnOkButtonClick(object? sender, EventArgs e)
        {
            if (!propertyGrid.ClearSelection(true))
                return;
            SetFocusIfPossible();

            if(DataSource?.Instance is not null)
            {
                SaveData();
                Designer?.RaisePropertyChanged(DataSource?.Instance, DataSource?.PropInfo?.Name);
            }

            Close();
        }

        private void OnRemoveAllButtonClick(object? sender, EventArgs e)
        {
            Clear();
            UpdateButtons();
        }

        private void OnAddChildButtonClick(object? sender, EventArgs e)
        {
            AddItem(treeView.SelectedItem);
        }

        private void OnAddButtonClick(object? sender, EventArgs e)
        {
            AddItem(treeView.SelectedItem?.Parent);
        }

        private void AddItem(TreeViewItem? parentItem)
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
                propertyGrid.Clear();
                return;
            }

            UpdatePropertyGrid(item.Tag);
        }

        private void UpdatePropertyGrid(object? instance)
        {
            lastPropInstance = instance;
            if (instance == null)
            {
                propertyGrid.Clear();
                return;
            }

            var propInstance = dataSource?.GetProperties(instance);
            propertyGrid.SetProps(propInstance, sort: true);
        }

        private void LoadData()
        {
            Clear();
            if (dataSource == null)
                return;
            AddItems(treeView.RootItem, dataSource.RootItems);
        }

        private void OnPropertyGridPropertyChanged(object? sender, EventArgs e)
        {
            var eventProp = propertyGrid.EventProperty;
            var eventInstance = eventProp?.Instance;
            var itemInfo = GetItemInfo(eventInstance);
            var selectedItem = treeView.SelectedItem;

            if (selectedItem == null || selectedItem.Tag != eventInstance)
                return;
            if (itemInfo == null)
                return;
            selectedItem.ImageIndex = itemInfo.Value.ImageIndex;
            selectedItem.Text = itemInfo.Value.Title ?? CommonStrings.Default.ListEditDefaultItemTitle;
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

        private class TreeViewPlus : StdTreeView, IEnumerableTree<TreeViewItem>
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