using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Shows list with registered developer tools actions.
    /// </summary>
    internal partial class SelectDevToolsActionPage : ContentPage
    {
        private readonly StackLayout layout;
        private readonly StackLayout buttonLayout;
        private readonly Button buttonOk;
        private readonly Button buttonCancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectDevToolsActionPage"/> class.
        /// </summary>
        public SelectDevToolsActionPage()
        {
            List<Person> people = new()
            {
                new ("Abigail", new DateTime(1975, 1, 15), Color.Aqua),
                new ("Bob", new DateTime(1976, 2, 20), Color.Black),
                new ("Yvonne", new DateTime(1987, 1, 10), Color.Purple),
                new ("Zachary", new DateTime(1988, 2, 5), Color.Red),
            };

            layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            buttonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };

            Microsoft.Maui.Controls.ListView listView = new()
            {
                // Source of data items.
                ItemsSource = people,

                // Define template for displaying each item.
                // (Argument of DataTemplate constructor is called for
                // each item; it must return a Cell derivative.)
                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Microsoft.Maui.Controls.Label nameLabel = new();
                    nameLabel.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, "Name");

                    Microsoft.Maui.Controls.Label birthdayLabel = new();
                    birthdayLabel.SetBinding(
                        Microsoft.Maui.Controls.Label.TextProperty,
                        new Binding(
                            "Birthday",
                            BindingMode.OneWay,
                            null,
                            null,
                            "Born {0:d}"));

                    BoxView boxView = new();
                    boxView.SetBinding(BoxView.ColorProperty, "FavoriteColor");

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                                {
                                    boxView,
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            nameLabel,
                                            birthdayLabel,
                                        },
                                    },
                                },
                        },
                    };
                }),
            };

            layout.Add(listView);
            layout.Add(buttonLayout);

            buttonOk = new()
            {
                Text = "Ok",
            };

            buttonCancel = new()
            {
                Text = "Cancel",
            };

            buttonOk.Clicked += ButtonOk_Clicked;
            buttonCancel.Clicked += ButtonCancel_Clicked;

            buttonLayout.Add(buttonOk);
            buttonLayout.Add(buttonCancel);

            Content = layout;
        }

        private async void ButtonCancel_Clicked(object? sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void ButtonOk_Clicked(object? sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public class Person
        {
            public Person(string name, DateTime birthday, Color favoriteColor)
            {
                this.Name = name;
                this.Birthday = birthday;
                this.FavoriteColor = favoriteColor;
            }

            public string Name { get; private set; }

            public DateTime Birthday { get; private set; }

            public Color FavoriteColor { get; private set; }
        }
    }
}
