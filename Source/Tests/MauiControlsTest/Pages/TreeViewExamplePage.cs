using Alternet.Maui;

using Microsoft.Maui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public partial class TreeViewExamplePage : ContentPage
    {
        private readonly Alternet.Maui.SimpleTreeView treeView = new ();

        private int? selectedIndex;
        private Alternet.UI.TreeControlItem? selectedItem;

        public TreeViewExamplePage()
        {
            Alternet.UI.ListControlUtils.AddTestItems(treeView.RootItem, 10, ItemInitialize);

            void ItemInitialize(Alternet.UI.TreeControlItem item)
            {
            }

            var menuFlyout = new MenuFlyout();
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Add",
                Command = new Command(() =>
                {
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Remove",
                Command = new Command(() =>
                {
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Clear",
                Command = new Command(() =>
                {
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Rename",
                Command = new Command(() =>
                {
                }),
            });

            /*
            collectionView = CreateSampleCollectionView(tapGesture);
            collectionView.ItemsSource = SampleItems;
            */

            FlyoutBase.SetContextFlyout(treeView, menuFlyout);

            /*
            collectionView.SetBinding(CollectionView.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this));
            collectionView.SelectionChanged += OnSelectionChanged;
            collectionView.SelectionMode = SelectionMode.Single;
            */

            var toolbar = new SimpleToolBarView();

            toolbar.AddButton(
                "Add",
                "Add new item",
                Alternet.UI.KnownSvgImages.ImgAdd,
                () =>
                {
                });

            toolbar.AddSeparator();

            toolbar.AddButton(
                "Remove",
                "Remove current item",
                Alternet.UI.KnownSvgImages.ImgRemove,
                () =>
                {
                });

            toolbar.AddButton(
                "Clear",
                "Clear items",
                Alternet.UI.KnownSvgImages.ImgRemoveAll,
                () =>
                {
                });

            toolbar.AddButton(
                "Rename",
                "Rename current item",
                Alternet.UI.KnownSvgImages.ImgGear,
                () =>
                {
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

            treeView.TreeChanged();
        }

        public int? SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                }
            }
        }

        public static CollectionView CreateSampleCollectionView(GestureRecognizer? tapGesture = null)
        {
            var collectionView = new CollectionView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new()
                    {
                        Padding = 5,
                    };

                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    Label nameLabel = new()
                    {
                        Margin = 5,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };

                    nameLabel.SetBinding(Label.TextProperty, static (Item item) => item.Name);

                    grid.Add(nameLabel);

                    if (tapGesture is not null)
                        grid.GestureRecognizers.Add(tapGesture);

                    return grid;
                })
            };

            return collectionView;
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Item selectedItem)
            {
                /*
                SelectedIndex = SampleItems.IndexOf(selectedItem);
                */
            }
            else
            {
                SelectedIndex = null;
            }
        }

        public Alternet.UI.TreeControlItem? SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        public class Item : INotifyPropertyChanged
        {
            private string? name;

            public string? Name
            {
                get => name;
                set
                {
                    if (name != value)
                    {
                        name = value;
                        OnPropertyChanged(nameof(Name));
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
