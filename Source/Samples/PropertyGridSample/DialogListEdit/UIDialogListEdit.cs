using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class UIDialogListEdit : UIDialogWindow
    {
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly AuiToolbar toolbar = new();
        private readonly StatusBar statusbar = new();
        
        private readonly TreeView treeView = new()
        {
            HasBorder = false,
        };

        private readonly PropertyGrid propertyGrid = new()
        {
            HasBorder = false,
        };

        private object? dataSource;

        public UIDialogListEdit()
        {
            propertyGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propertyGrid.Features = PropertyGridFeature.QuestionCharInNullable;

            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            AlwaysOnTop = true;
            Size = new(600, 400);
            Title = "Collection Editor";
            StartLocation = WindowStartLocation.CenterScreen;

            this.StatusBar = statusbar;

            treeView.MakeAsListBox();
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
                /*AuiToolbarCreateStyle.HorzLayout |*/
                /*AuiToolbarCreateStyle.Text | */
                AuiToolbarCreateStyle.NoTooltips |
                AuiToolbarCreateStyle.DefaultStyle;

            toolbarStyle &= ~AuiToolbarCreateStyle.Gripper;

            toolbar.CreateStyle = toolbarStyle;

            var addImage = ImageSet.FromSvgUrlForToolbar(SvgUtils.UrlImagePlus, this);
            var removeImage = ImageSet.FromSvgUrlForToolbar(SvgUtils.UrlImageMinus, this);

            var addButtonId = toolbar.AddTool(
                CommonStrings.Default.ButtonAdd,
                addImage,
                CommonStrings.Default.ButtonAdd);
            var removeButtonId = toolbar.AddTool(
                CommonStrings.Default.ButtonRemove,
                removeImage,
                CommonStrings.Default.ButtonRemove);

            toolbar.Realize();
            manager.AddPane(toolbar, toolbarPane);

            // Center pane
            var centerPane = manager.CreatePaneInfo();
            centerPane.Name(nameof(centerPane)).CenterPane().PaneBorder(false);

            manager.AddPane(treeView, centerPane);

            manager.Update();

            toolbar.AddToolOnClick(addButtonId, AddButton_Click);
            toolbar.AddToolOnClick(removeButtonId, RemoveButton_Click);

            propertyGrid.ApplyKnownColors(PropertyGridKnownColors.White);
            propertyGrid.CenterSplitter();
            propertyGrid.SetVerticalSpacing();

            this.Disposed += UIDialogCollectionEdit_Disposed;
        }

        private void UIDialogCollectionEdit_Disposed(object? sender, EventArgs e)
        {
            manager.UnInit();
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            var item = new TreeViewItem
            {
                Text = "item"
            };
            treeView.Items.Add(item);
            treeView.SelectedItem = item;
            item.IsFocused = true;
        }

        private void RemoveButton_Click(object? sender, EventArgs e)
        {
            treeView.RemoveItemAndSelectSibling(treeView.SelectedItem);
        }

        private void TreeView_SelectionChanged(object? sender, EventArgs e)
        {
            var item = treeView.SelectedItem;
            if (item == null)
            {
                propertyGrid.Clear();
                return;
            }

            propertyGrid.SetProps(item);
        }

        public object? DataSource {
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
            }
        }

        private void Load()
        {
            Clear();
            var collection = AsList;
            if (collection == null)
                return;
            foreach(var item in collection)
            {
                var s = item.ToString();
                if (string.IsNullOrWhiteSpace(s))
                    s = "item";

                var treeItem = new TreeViewItem(s)
                {
                    Tag = item
                };

                treeView.Items.Add(treeItem);
            }
        }

        private void Clear()
        {
            propertyGrid.Clear();
            treeView.RemoveAll();
        }

        private IList? AsList => dataSource as IList;

        private INotifyPropertyChanged? AsNotifyProperty => dataSource as INotifyPropertyChanged;

        private INotifyCollectionChanged? AsNotifyCollection =>
            dataSource as INotifyCollectionChanged;

        private void Bind()
        {
            if (dataSource == null)
                return;
            var notifyProp = AsNotifyProperty;
            var notifyCollection = AsNotifyCollection;

            if(notifyProp!=null)
                notifyProp.PropertyChanged += NotifyProp_PropertyChanged;
            if(notifyCollection != null)
                notifyCollection.CollectionChanged += NotifyCollection_CollectionChanged;
        }

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

        private void NotifyProp_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

        private void Unbind()
        {
            if (dataSource == null)
                return;
            var notifyProp = AsNotifyProperty;
            var notifyCollection = AsNotifyCollection;

            if (notifyProp != null)
                notifyProp.PropertyChanged -= NotifyProp_PropertyChanged;
            if (notifyCollection != null)
                notifyCollection.CollectionChanged -= NotifyCollection_CollectionChanged;
        }
    }
}
