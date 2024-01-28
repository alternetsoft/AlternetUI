﻿using System;
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
    internal class UIDialogListEditWindow : DialogWindow
    {
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly AuiToolbar toolbar = new();
        private readonly StatusBar statusbar = new();
        private readonly int buttonIdAdd;
        private readonly int buttonIdRemove;
        private readonly int buttonIdRemoveAll;
        private readonly int buttonIdAddChild;
        private readonly int buttonIdOk;
        private readonly int buttonIdCancel;

        private readonly TreeViewPlus treeView = new()
        {
            HasBorder = false,
        };

        private readonly PropertyGrid propertyGrid = new()
        {
            HasBorder = false,
        };

        private IListEditSource? dataSource;
        private object? lastPropInstance;

        public UIDialogListEditWindow(IListEditSource source)
        {
            treeView.FullRowSelect = true;
            treeView.ShowLines = false;

            propertyGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propertyGrid.Features = PropertyGridFeature.QuestionCharInNullable;

            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Title = CommonStrings.Default.WindowTitleListEdit;
            StartLocation = WindowStartLocation.CenterScreen;

            this.StatusBar = statusbar;

            treeView.SelectionChanged += TreeView_SelectionChanged;

            panel.Layout = GenericLayoutStyle.Native;
            Children.Add(panel);
            manager.SetManagedWindow(panel);
            manager.SetDefaultSplitterSashProps();

            const int defaultWidth = 300;

            // Right Pane
            var rightPane = manager.CreatePaneInfo();
            rightPane.Name(nameof(rightPane)).Caption("Properties").Right().PaneBorder(false)
                .TopDockable(false).BottomDockable(false)
                .BestSizeDip(defaultWidth, 300).MinSizeDip(defaultWidth, 300)
                .CaptionVisible(false);
            propertyGrid.SuggestedWidth = defaultWidth;
            manager.AddPane(propertyGrid, rightPane);

            // Toolbar pane
            var toolbarPane = manager.CreatePaneInfo();
            toolbarPane.Name(nameof(toolbarPane)).Top().ToolbarPane().PaneBorder(false)
                .Movable(false).Floatable(false).Resizable(false).Gripper(false).Fixed().DockFixed();

            var toolbarStyle =
                AuiToolbarCreateStyle.PlainBackground |
                AuiToolbarCreateStyle.HorzLayout |
                AuiToolbarCreateStyle.Text |
                AuiToolbarCreateStyle.NoTooltips |
                AuiToolbarCreateStyle.DefaultStyle;

            toolbarStyle &= ~AuiToolbarCreateStyle.Gripper;

            toolbar.CreateStyle = toolbarStyle;
            var imageSize = PanelAuiManager.GetBaseToolSvgSize();
            toolbar.ToolBitmapSizeInPixels = imageSize;

            var images = KnownSvgImages.GetForSize(
                toolbar.GetSvgColor(KnownSvgColor.Normal),
                imageSize);

            if (source.AllowAdd)
            {
                buttonIdAdd = toolbar.AddTool(
                    CommonStrings.Default.ButtonAdd,
                    images.ImgAdd,
                    CommonStrings.Default.ButtonAdd);
            }

            if(source.AllowSubItems && source.AllowAdd)
            {
                buttonIdAddChild = toolbar.AddTool(
                    CommonStrings.Default.ButtonAddChild,
                    images.ImgAddChild,
                    CommonStrings.Default.ButtonAddChild);
            }

            if (source.AllowDelete)
            {
                buttonIdRemove = toolbar.AddTool(
                    CommonStrings.Default.ButtonRemove,
                    images.ImgRemove,
                    CommonStrings.Default.ButtonRemove);
                buttonIdRemoveAll = toolbar.AddTool(
                    CommonStrings.Default.ButtonClear,
                    images.ImgRemoveAll,
                    CommonStrings.Default.ButtonClear);
            }

            if (source.AllowApplyData)
            {
                buttonIdOk = toolbar.AddTool(
                    CommonStrings.Default.ButtonOk,
                    images.ImgOk,
                    CommonStrings.Default.ButtonOk);
            }

            buttonIdCancel = toolbar.AddTool(
                CommonStrings.Default.ButtonCancel,
                images.ImgCancel,
                CommonStrings.Default.ButtonCancel);

            toolbar.Realize();
            manager.AddPane(toolbar, toolbarPane);

            // Center pane
            var centerPane = manager.CreatePaneInfo();
            centerPane.Name(nameof(centerPane)).CenterPane().PaneBorder(false);
            manager.AddPane(treeView, centerPane);

            manager.Update();

            toolbar.AddToolOnClick(buttonIdAdd, AddButton_Click);
            toolbar.AddToolOnClick(buttonIdRemove, RemoveButton_Click);
            toolbar.AddToolOnClick(buttonIdRemoveAll, RemoveAllButton_Click);
            toolbar.AddToolOnClick(buttonIdAddChild, AddChildButton_Click);
            toolbar.AddToolOnClick(buttonIdOk, OkButton_Click);
            toolbar.AddToolOnClick(buttonIdCancel, CancelButton_Click);

            propertyGrid.SuggestedInitDefaults();

            this.Disposed += UIDialogCollectionEdit_Disposed;
            propertyGrid.PropertyChanged += PropertyGrid_PropertyChanged;

            ComponentDesigner.InitDefault();
            ComponentDesigner.Default!.PropertyChanged += OnDesignerPropertyChanged;
            Size = new(750, 600);
            MinimumSize = new(550, 350);
            rightPane.BestSizeDip(defaultWidth + 1, 300).MinSizeDip(defaultWidth + 1, 300);
            propertyGrid.SuggestedWidth = defaultWidth + 1;
            manager.Update();
            PerformLayout();
            Closing += UIDialogListEditWindow_Closing;
            Closed += UIDialogListEditWindow_Closed;

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
                Load();
                Bind();
                UpdateButtons();
            }
        }

        internal void Save()
        {
            if (dataSource is null)
                return;
            dataSource.ApplyData(treeView);
        }

        internal void Clear()
        {
            propertyGrid.Clear();
            treeView.RemoveAll();
        }

        private void UIDialogListEditWindow_Closed(object? sender, EventArgs e)
        {
        }

        private void UIDialogListEditWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            ComponentDesigner.Default!.PropertyChanged -= OnDesignerPropertyChanged;
            if (ModalResult == ModalResult.Accepted)
            {
                Save();
                Designer?.RaisePropertyChanged(DataSource?.Instance, DataSource?.PropInfo?.Name);
            }
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

        private void UIDialogCollectionEdit_Disposed(object? sender, EventArgs e)
        {
            ComponentDesigner.Default!.PropertyChanged -= OnDesignerPropertyChanged;
            manager.UnInit();
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            toolbar.SetMouseCapture(false);
            SetFocusIfPossible();
            Application.DoEvents();
            if (Modal)
                ModalResult = ModalResult.Canceled;
            Close();
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            toolbar.SetMouseCapture(false);
            if (!propertyGrid.ClearSelection(true))
                return;
            SetFocusIfPossible();
            Application.DoEvents();
            if (Modal)
                ModalResult = ModalResult.Accepted;
            Close();
        }

        private void RemoveAllButton_Click(object? sender, EventArgs e)
        {
            Clear();
            UpdateButtons();
        }

        private void AddChildButton_Click(object? sender, EventArgs e)
        {
            AddItem(treeView.SelectedItem?.Items);
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            AddItem(treeView.SelectedItem?.ParentItems);
        }

        private void AddItem(Collection<TreeViewItem>? items)
        {
            items ??= treeView.Items;

            if (dataSource == null)
                return;

            var item = dataSource.CreateNewItem();

            if (item == null)
                return;

            var itemInfo = GetItemInfo(item);

            var treeItem = new TreeViewItem(itemInfo!.Value.Title!, itemInfo.Value.ImageIndex)
            {
                Tag = item,
            };
            items.Add(treeItem);
            treeView.SelectedItem = treeItem;
            treeItem.IsFocused = true;
            UpdateButtons();
        }

        private void RemoveButton_Click(object? sender, EventArgs e)
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
                canRemoveAll = dataSource.AllowDelete && treeView.Items.Count > 0;
                canApply = dataSource.AllowApplyData;
            }

            toolbar.EnableTool(buttonIdAdd, canAdd);
            toolbar.EnableTool(buttonIdRemove, canRemove);
            toolbar.EnableTool(buttonIdRemoveAll, canRemoveAll);
            toolbar.EnableTool(buttonIdAddChild, canAddChild);
            toolbar.EnableTool(buttonIdOk, canApply);
            toolbar.Refresh();
        }

        private void TreeView_SelectionChanged(object? sender, EventArgs e)
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
            propertyGrid.SetProps(propInstance);
        }

        private void Load()
        {
            Clear();
            if (dataSource == null)
                return;
            AddItems(treeView.Items, dataSource.RootItems);
        }

        private void PropertyGrid_PropertyChanged(object? sender, EventArgs e)
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

        private void AddItems(Collection<TreeViewItem> treeItems, IEnumerable? data)
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
                    AddItems(treeItem.Items, subItems);

                treeItems.Add(treeItem);
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

        public class TreeViewPlus : TreeView, IEnumerableTree<TreeViewItem>
        {
            public TreeViewPlus()
            {
                AllowDrop = true;

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
                    return Array.Empty<TreeViewItem>();
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
                return Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Items.GetEnumerator();
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
                Application.DebugLog("Click");
            }

            internal void TreeViewPlus_MouseUp(object sender, MouseEventArgs e)
            {
                Application.DebugLog("MouseUp");
            }

            internal void TreeViewPlus_MouseDown(object sender, MouseEventArgs e)
            {
                Application.DebugLog("MouseDown");
            }

            internal void OnDragDropEvent(object? sender, DragEventArgs e)
            {
                Application.DebugLog($"DragDrop: {e.MouseClientLocation}, {e.Effect}");
                Application.DebugLog($"Dropped Data: {DataObject.ToDebugString(e.Data)}");
            }

            internal void OnDragOverEvent(object? sender, DragEventArgs e)
            {
                Application.DebugLogReplace(
                    $"DragOver: {e.MouseClientLocation}, {e.Effect}", "DragOver");
            }

            internal void TreeViewPlus_MouseLeftButtonUp(object sender, MouseEventArgs e)
            {
                Application.DebugLog("MouseLeftButtonUp");
            }

            internal void TreeViewPlus_PreviewMouseUp(object sender, MouseEventArgs e)
            {
                Application.DebugLog("PreviewMouseUp");
            }

            internal void OnDragEnterEvent(object? sender, DragEventArgs e)
            {
                Application.DebugLog($"DragEnter: {e.MouseClientLocation}, {e.Effect}");
            }

            internal void OnDragLeaveEvent(object? sender, EventArgs e)
            {
                Application.DebugLog("DragLeave");
            }
        }
    }
}