using Alternet.Maui;

using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public class CollectionViewExamplePage : ContentPage
    {
        private Item? selectedItem;
        private int? selectedIndex;
        private CollectionView collectionView;
        private BoxView underline;

        private ObservableCollection<Item> items = new()
            {
                new Item { Name = "Alice", Title = "Developer", Description = "Loves coding and coffee" },
                new Item { Name = "Bob", Title = "Designer", Description = "Passionate about UI/UX" },
                new Item { Name = "Carol", Title = "Manager", Description = "Focuses on team productivity" },
                new Item { Name = "Alice", Title = "Developer", Description = "Loves coding and coffee" },
                new Item { Name = "Bob", Title = "Designer", Description = "Passionate about UI/UX" },
                new Item { Name = "Carol", Title = "Manager", Description = "Focuses on team productivity" },
                new Item { Name = "Alice", Title = "Developer", Description = "Loves coding and coffee" },
                new Item { Name = "Bob", Title = "Designer", Description = "Passionate about UI/UX" },
                new Item { Name = "Carol", Title = "Manager", Description = "Focuses on team productivity" },
            };

        public void CollectionAdd()
        {
            var newItem = new Item
            {
                Name = "Carol",
                Title = "Manager",
                Description = "Focuses on team productivity"
            };

            items.Add(newItem);
        }

        public void CollectionRemove()
        {
            if (SelectedIndex is null)
                return;
            items.RemoveAt(SelectedIndex.Value);
        }

        public void CollectionClear()
        {
            items.Clear();
        }

        public void CollectionRename()
        {
            if (SelectedItem is null)
                return;
            SelectedItem.Name = SelectedItem.Name + "1";
        }

        public CollectionViewExamplePage()
        {
            var menuFlyout = new MenuFlyout();
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Add",
                Command = new Command(() =>
                {
                    CollectionAdd();
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Remove",
                Command = new Command(() =>
                {
                    CollectionRemove();
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Clear",
                Command = new Command(() =>
                {
                    CollectionClear();
                }),
            });
            menuFlyout.Add(new MenuFlyoutItem
            {
                Text = "Rename",
                Command = new Command(() =>
                {
                    CollectionRename();
                }),
            });

            var tapGesture = new TapGestureRecognizer
            {
                Buttons = ButtonsMask.Secondary,
            };
            tapGesture.Tapped += (sender, e) =>
            {
                if (sender is View tappedFrame && tappedFrame.BindingContext is Item item)
                {
                    collectionView!.SelectedItem = item;
                }
            };

            collectionView = new CollectionView
            {
                ItemsSource = items,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid { Padding = 5 };
                    
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50)});
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    Label nameLabel = new()
                    {
                        Margin=5,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };

                    nameLabel.SetBinding(Label.TextProperty, static (Item item) => item.Name);

                    Label titleLabel = new()
                    {
                        Margin = 5,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    titleLabel.SetBinding(Label.TextProperty, static (Item item) => item.Title);

                    Label descriptionLabel = new()
                    {
                        Margin = 5,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    descriptionLabel.SetBinding(Label.TextProperty, static (Item item) => item.Description);

                    Grid.SetColumn(titleLabel, 1);
                    Grid.SetColumn(descriptionLabel, 2);

                    grid.Add(nameLabel);
                    grid.Add(titleLabel);
                    grid.Add(descriptionLabel);

                    grid.GestureRecognizers.Add(tapGesture);

                    return grid;
                })
            };

            FlyoutBase.SetContextFlyout(collectionView, menuFlyout);

            collectionView.SetBinding(CollectionView.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this));
            collectionView.SelectionChanged += OnSelectionChanged;
            collectionView.SelectionMode = SelectionMode.Single;

            var toolbar = new SimpleToolBarView();

            toolbar.AddButton(
                "Add",
                "Add new item",
                Alternet.UI.KnownSvgImages.ImgAdd,
                () =>
                {
                    CollectionAdd();
                });

            toolbar.AddSeparator();

            toolbar.AddButton(
                "Remove",
                "Remove current item",
                Alternet.UI.KnownSvgImages.ImgRemove,
                () =>
                {
                    CollectionRemove();
                });

            toolbar.AddButton(
                "Clear",
                "Clear items",
                Alternet.UI.KnownSvgImages.ImgRemoveAll,
                () =>
                {
                    CollectionClear();
                });

            toolbar.AddButton(
                "Rename",
                "Rename current item",
                Alternet.UI.KnownSvgImages.ImgGear,
                () =>
                {
                    CollectionRename();
                });

            toolbar.AddExpandingSpace();

            toolbar.AddButton(
                null,
                "More actions",
                Alternet.UI.KnownSvgImages.ImgMoreActions,
                () =>
                {
                });

            underline = new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = toolbar.GetSeparatorColor(),
            };
            
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            grid.Add(toolbar, 0, 0);
            grid.Add(underline, 0, 1);            
            grid.Add(collectionView, 0, 2);

            toolbar.SystemColorsChanged += (s, e) =>
            {
                underline.BackgroundColor = toolbar.GetSeparatorColor();
            };

            Content = grid;
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

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Item selectedItem)
            {
                SelectedIndex = items.IndexOf(selectedItem);
            }
            else
            {
                SelectedIndex = null;
            }
        }

        public Item? SelectedItem
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
            private string? title;
            private string? description;

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

            public string? Title
            {
                get => title;
                set
                {
                    if (title != value)
                    {
                        title = value;
                        OnPropertyChanged(nameof(Title));
                    }
                }
            }

            public string? Description
            {
                get => description;
                set
                {
                    if (description != value)
                    {
                        description = value;
                        OnPropertyChanged(nameof(Description));
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
