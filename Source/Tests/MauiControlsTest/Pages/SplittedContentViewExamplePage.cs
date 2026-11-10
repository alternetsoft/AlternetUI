using Alternet.Maui;
using Alternet.Maui.Extensions;

using Microsoft.Maui.Controls;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AllQuickStarts
{
    public partial class SplittedContentViewExamplePage : ContentPage
    {
        private readonly SplittedContentView sampleView;
        private readonly BoxView firstBoxView = new ();
        private readonly BoxView secondBoxView = new ();

        public SplittedContentViewExamplePage()
        {
            var firstLabel = new Label
            {
                Text = "First",
                TextColor = Microsoft.Maui.Graphics.Colors.Black,
                Margin = new Thickness(10, 10, 10, 10),
            };

            var secondLabel = new Label
            {
                Text = "Second",
                TextColor = Microsoft.Maui.Graphics.Colors.Black,
                Margin = new Thickness(10, 10, 10, 10),
            };

            var firstGrid = new Grid();
            var secondGrid = new Grid();

            firstGrid.Children.Add(firstBoxView);
            firstGrid.Children.Add(firstLabel);
            secondGrid.Children.Add(secondBoxView);
            secondGrid.Children.Add(secondLabel);

            sampleView = new SplittedContentView();
            sampleView.FirstContent = firstGrid;
            sampleView.SecondContent = secondGrid;            

            var isDark = Alternet.UI.App.IsDark;

            var firstColor = Alternet.Drawing.ThemedColors.Blue.GetColor(isDark).ToMaui();
            var secondColor = Alternet.Drawing.ThemedColors.Green.GetColor(isDark).ToMaui();

            firstBoxView.Color = firstColor;
            secondBoxView.Color = secondColor;

            var toolbar = new SimpleToolBarView();

            toolbar.AddButton(
                "Toggle orientation",
                null,
                null,
                () =>
                {
                    sampleView.IsVertical = !sampleView.IsVertical;
                });

            toolbar.AddButton(
                "Toggle visibility",
                null,
                null,
                () =>
                {
                    sampleView.IsFixedPaneVisible = !sampleView.IsFixedPaneVisible;
                });

            toolbar.AddButton(
                "Toggle fixed",
                null,
                null,
                () =>
                {
                    sampleView.IsFirstPaneFixed = !sampleView.IsFirstPaneFixed;
                });

            toolbar.AddExpandingSpace();
            toolbar.IsTopBorderVisible = true;
            toolbar.IsBottomBorderVisible = true;

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            grid.Add(toolbar, 0, 0);
            grid.Add(sampleView, 0, 1);

            Content = grid;
        }
    }
}
