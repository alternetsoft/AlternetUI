using Alternet.Maui;

using Microsoft.Maui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public partial class VirtualTreeControlExamplePage : ContentPage
    {
        private readonly AbsoluteLayout layout = new();

        private readonly VirtualTreeControlView treeView = new ();

        public VirtualTreeControlExamplePage()
        {
            Alternet.UI.ListControlUtils.AddTestItems(treeView.Control.RootItem, 10, ItemInitialize);

            void ItemInitialize(Alternet.UI.TreeViewItem item)
            {
            }

            var menuFlyout = new MenuFlyout();
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Add",
                Command = new Command(() =>
                {
                    AddNewItem();
                }),
            });
            menuFlyout = new MenuFlyout();
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Add Child",
                Command = new Command(() =>
                {
                    AddNewChildItem();
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Remove",
                Command = new Command(() =>
                {
                    treeView.Control.RemoveSelectedItem(true);
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Clear",
                Command = new Command(() =>
                {
                    treeView.Control.Clear();
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Rename",
                Command = new Command(() =>
                {
                    RenameSelectedItem();
                }),
            });

            FlyoutBase.SetContextFlyout(treeView, menuFlyout);

            var toolbar = new SimpleToolBarView();

            toolbar.AddButton(
                "Add",
                "Add new item",
                Alternet.UI.KnownSvgImages.ImgAdd,
                () =>
                {
                    AddNewItem();
                });

            toolbar.AddButton(
                "Add child",
                "Add new child item",
                Alternet.UI.KnownSvgImages.ImgAdd,
                () =>
                {
                    AddNewChildItem();
                });

            toolbar.AddSeparator();

            toolbar.AddButton(
                "Remove",
                "Remove current item",
                Alternet.UI.KnownSvgImages.ImgRemove,
                () =>
                {
                    treeView.Control.RemoveSelectedItem(true);
                });

            toolbar.AddButton(
                "Clear",
                "Clear items",
                Alternet.UI.KnownSvgImages.ImgRemoveAll,
                () =>
                {
                    treeView.Control.Clear();
                });

            toolbar.AddButton(
                "Rename",
                "Rename current item",
                Alternet.UI.KnownSvgImages.ImgGear,
                () =>
                {
                    RenameSelectedItem();
                });

            toolbar.AddExpandingSpace();
            toolbar.IsTopBorderVisible = true;
            toolbar.IsBottomBorderVisible = true;

            toolbar.AddMoreActionsButton(
                () =>
                {
                });

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            grid.Add(toolbar, 0, 0);
            grid.Add(treeView, 0, 1);         
            
            Content = grid;
        }

        public void AddNewChildItem()
        {
            var item = new Alternet.UI.TreeViewItem();
            item.Text = "item " + Alternet.UI.LogUtils.GenNewId();
            item.SvgImage = Alternet.UI.KnownColorSvgImages.ImgLogo;
            treeView.Control.AddChild(treeView.Control.SelectedItem, item, true);
        }

        public void AddNewItem()
        {
            var item = new Alternet.UI.TreeViewItem();
            item.Text = "item " + Alternet.UI.LogUtils.GenNewId();
            item.SvgImage = Alternet.UI.KnownColorSvgImages.ImgLogo;
            treeView.Control.Add(item, true);
        }

        public void RenameSelectedItem()
        {
            var item = treeView.Control.SelectedItem;
            if (item is null)
                return;
            item.Text += "a";
        }
    }
}
