using Alternet.Maui;

using Microsoft.Maui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public partial class CollectionViewExamplePage : ContentPage
    {
        public static ObservableCollection<Item> SampleItems2 = new()
        {
            new Item
            {
                Name = "TaskManager",
                Title = "Task Management Tool",
                Description = "Helps organize and prioritize daily tasks efficiently."
            },
            new Item
            {
                Name = "BudgetPlanner",
                Title = "Financial Budget Planner",
                Description = "Assists in managing and tracking personal finances."
            },
            new Item
            {
                Name = "WeatherApp",
                Title = "Weather Forecast App",
                Description = "Provides accurate weather updates and alerts."
            },
            new Item
            {
                Name = "CodeEditor",
                Title = "Lightweight Code Editor",
                Description = "An editor for writing and editing code with syntax highlighting."
            },
            new Item
            {
                Name = "RecipeHelper",
                Title = "Recipe Management Tool",
                Description = "Organizes recipes and generates shopping lists."
            }
        };

        public static ObservableCollection<Item> SampleItems = new()
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

        private readonly CollectionView collectionView;

        private Item? selectedItem;
        private int? selectedIndex;

        public void CollectionAdd()
        {
            var newItem = new Item
            {
                Name = "Carol",
                Title = "Manager",
                Description = "Focuses on team productivity"
            };

            SampleItems.Add(newItem);
        }

        public void CollectionRemove()
        {
            if (SelectedIndex is null)
                return;
            SampleItems.RemoveAt(SelectedIndex.Value);
        }

        public void CollectionClear()
        {
            SampleItems.Clear();
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

            collectionView = CreateSampleCollectionView(tapGesture);
            collectionView.ItemsSource = SampleItems;

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
            grid.Add(collectionView, 0, 1);

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

                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    Label nameLabel = new()
                    {
                        Margin = 5,
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

                    if (tapGesture is not null)
                        grid.GestureRecognizers.Add(tapGesture);

                    return grid;
                })
            };

            return collectionView;
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection is null || e.CurrentSelection.Count == 0)
            {
                SelectedIndex = null;
                return;
            }

            if (e.CurrentSelection[0] is Item selectedItem)
            {
                SelectedIndex = SampleItems.IndexOf(selectedItem);
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

        public partial class Item : INotifyPropertyChanged
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
