using Alternet.Maui;

using Microsoft.Maui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public partial class TreeViewExamplePage : ContentPage
    {
        private readonly SimpleTreeView treeView = new ();

        public TreeViewExamplePage()
        {
            Alternet.UI.ListControlUtils.AddTestItems(treeView.RootItem, 10, ItemInitialize);

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
                    treeView.RemoveSelectedItem(true);
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Clear",
                Command = new Command(() =>
                {
                    treeView.Clear();
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
                    treeView.RemoveSelectedItem(true);
                });

            toolbar.AddButton(
                "Clear",
                "Clear items",
                Alternet.UI.KnownSvgImages.ImgRemoveAll,
                () =>
                {
                    treeView.Clear();
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

            toolbar.AddButton(
                null,
                "More actions",
                Alternet.UI.KnownSvgImages.ImgMoreActions,
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
            treeView.AddChild(treeView.SelectedItem, item, true);
        }

        public void AddNewItem()
        {
            var item = new Alternet.UI.TreeViewItem();
            item.Text = "item " + Alternet.UI.LogUtils.GenNewId();
            item.SvgImage = Alternet.UI.KnownColorSvgImages.ImgLogo;
            treeView.Add(item, true);
        }

        public void RenameSelectedItem()
        {
            var item = treeView.SelectedItem;
            if (item is null)
                return;
            item.Text += "a";
        }
    }
}
