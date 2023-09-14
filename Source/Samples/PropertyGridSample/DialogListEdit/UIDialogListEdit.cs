using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class UIDialogListEdit : UIDialogWindow
    {
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly AuiToolbar toolbar = new();
        private readonly StatusBar statusbar = new();
        private readonly int buttonIdAdd;
        private readonly int buttonIdRemove;

        private readonly TreeView treeView = new()
        {
            HasBorder = false,
        };

        private readonly PropertyGrid propertyGrid = new()
        {
            HasBorder = false,
        };

        private IListEditSource? dataSource;

        public UIDialogListEdit()
        {
            treeView.FullRowSelect = true;
            treeView.ShowLines = false;

            propertyGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propertyGrid.Features = PropertyGridFeature.QuestionCharInNullable;

            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Size = new(600, 400);
            Title = CommonStrings.Default.WindowTitleListEditor;
            StartLocation = WindowStartLocation.CenterScreen;

            this.StatusBar = statusbar;

            treeView.SelectionChanged += TreeView_SelectionChanged;

            panel.Layout = LayoutPanelKind.Native;
            Children.Add(panel);
            manager.SetManagedWindow(panel);

            // Right Pane
            var rightPane = manager.CreatePaneInfo();
            rightPane.Name(nameof(rightPane)).Caption("Properties").Right().PaneBorder(false)
                .TopDockable(false).BottomDockable(false).BestSize(300,300).MinSize(300, 300)
                .CaptionVisible(false);
            propertyGrid.Width = 300;
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
            var imageSize = Toolbar.GetDefaultImageSize(this);
            toolbar.ToolBitmapSize = imageSize;

            var imageAdd = toolbar.LoadSvgImage(SvgUtils.UrlImagePlus, imageSize);
            var imageRemove = toolbar.LoadSvgImage(SvgUtils.UrlImageMinus, imageSize);
            var imageOk = toolbar.LoadSvgImage(SvgUtils.UrlImageOk, imageSize);
            var imageCancel = toolbar.LoadSvgImage(SvgUtils.UrlImageCancel, imageSize);
            var imageAddChild = toolbar.LoadSvgImage(SvgUtils.UrlImageAddChild, imageSize);
            var imageRemoveAll = toolbar.LoadSvgImage(SvgUtils.UrlImageRemoveAll, imageSize);

            buttonIdAdd = toolbar.AddTool(
                CommonStrings.Default.ButtonAdd,
                imageAdd,
                CommonStrings.Default.ButtonAdd);
            var buttonIdAddChild = toolbar.AddTool(
                CommonStrings.Default.ButtonAddChild,
                imageAddChild,
                CommonStrings.Default.ButtonAddChild);
            buttonIdRemove = toolbar.AddTool(
                CommonStrings.Default.ButtonRemove,
                imageRemove,
                CommonStrings.Default.ButtonRemove);
            var buttonIdRemoveAll = toolbar.AddTool(
                CommonStrings.Default.ButtonClear,
                imageRemoveAll,
                CommonStrings.Default.ButtonClear);
            var buttonIdOk = toolbar.AddTool(
                CommonStrings.Default.ButtonOk,
                imageOk,
                CommonStrings.Default.ButtonOk);
            var buttonIdCancel = toolbar.AddTool(
                CommonStrings.Default.ButtonCancel,
                imageCancel,
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

            propertyGrid.ApplyKnownColors(PropertyGridKnownColors.White);
            propertyGrid.CenterSplitter();
            propertyGrid.SetVerticalSpacing();

            this.Disposed += UIDialogCollectionEdit_Disposed;
            propertyGrid.PropertyChanged += PropertyGrid_PropertyChanged;
        }

        private void UIDialogCollectionEdit_Disposed(object? sender, EventArgs e)
        {
            manager.UnInit();
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (dataSource == null)
                return;
            
            var item = dataSource.CreateNewItem();

            if (item == null)
                return;

            var itemInfo = GetItemInfo(item);

            var treeItem = new TreeViewItem(itemInfo!.Value.Title!, itemInfo.Value.ImageIndex)
            {
                Tag = item
            };
            treeView.Items.Add(treeItem);
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
            var canRemove = false;
            if (dataSource != null)
            {
                canAdd = dataSource.AllowAdd;
                canRemove = (treeView.SelectedItem != null) && dataSource.AllowDelete;
            }
            toolbar.EnableTool(buttonIdAdd, canAdd);
            toolbar.EnableTool(buttonIdRemove, canRemove);
            toolbar.Invalidate();
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

            var tag = item.Tag;

            if(tag == null)
            {
                propertyGrid.Clear();
                return;
            }

            var propInstance = dataSource?.GetProperties(tag);

            propertyGrid.SetProps(propInstance);
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
            selectedItem.Text = itemInfo.Value.Title!;
        }

        private (string? Title, int? ImageIndex)? GetItemInfo(object? item)
        {
            if (item == null)
                return null;
            var s = dataSource?.GetItemTitle(item);
            int? imageIndex = dataSource?.GetItemImageIndex(item);
            if (string.IsNullOrWhiteSpace(s))
                s = "item";

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
                    Tag = item,
                };

                var subItems = dataSource?.GetChildren(item);
                if (subItems != null)
                    AddItems(treeItem.Items, subItems);

                treeItems.Add(treeItem);
            }
        }

        private void Clear()
        {
            propertyGrid.Clear();
            treeView.RemoveAll();
        }

/*
        private IList? AsList => dataSource as IList;

        private INotifyPropertyChanged? AsNotifyProperty => dataSource as INotifyPropertyChanged;

        private INotifyCollectionChanged? AsNotifyCollection =>
            dataSource as INotifyCollectionChanged;
*/
        private void Bind()
        {
            treeView.ImageList = dataSource?.ImageList;

            if (dataSource == null)
                return;

            /*
                        if (dataSource == null)
                            return;
                        var notifyProp = AsNotifyProperty;
                        var notifyCollection = AsNotifyCollection;

                        if(notifyProp!=null)
                            notifyProp.PropertyChanged += NotifyProp_PropertyChanged;
                        if(notifyCollection != null)
                            notifyCollection.CollectionChanged += NotifyCollection_CollectionChanged;
            */
        }

        /*
                private void NotifyCollection_CollectionChanged(
                    object? sender,
                    NotifyCollectionChangedEventArgs e)
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            break;
                        case NotifyCollectionChangedAction.Move:
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            Clear();
                            break;
                        default:
                            break;
                    }
                }
        */
        /*
                private void NotifyProp_PropertyChanged(object? sender, PropertyChangedEventArgs e)
                {
                }
        */
        private void Unbind()
        {
            treeView.ImageList = null;
            
/*
                        if (dataSource == null)
                            return;
                        var notifyProp = AsNotifyProperty;
                        var notifyCollection = AsNotifyCollection;

                        if (notifyProp != null)
                            notifyProp.PropertyChanged -= NotifyProp_PropertyChanged;
                        if (notifyCollection != null)
                            notifyCollection.CollectionChanged -= NotifyCollection_CollectionChanged;
            */
        }
    }
}
