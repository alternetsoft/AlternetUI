using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace AllQuickStarts
{
    public class CollectionViewExamplePage : ContentPage
    {
        public CollectionViewExamplePage()
        {
            var items = new ObservableCollection<Item>
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

            var collectionView = new CollectionView
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

                    return grid;
                })
            };

            // Set the content of the page
            Content = new Grid
            {
                Children = { collectionView }
            };
        }

        // Data model
        public class Item
        {
            public string? Name { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
        }
    }
}
